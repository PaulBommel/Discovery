using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor
{
    using Darkstat;
    using Darkstat.NpcQueryClient;
    using Darkstat.OreFieldQueryClient;
    using Darkstat.PobQueryClient;
    using Darkstat.RouteQueryClient;

    internal sealed class SimulationDataSourceProvider(IDarkstatClient client)
    {
        private readonly IDarkstatClient _client = client;

        public async Task<SimulationDataSource> GetDataSourceAsync(TradeRoute[] routes, CancellationToken token = default)
        {
            var transportedCommodities = (from route in routes
                                          from trade in route.Trades
                                          where trade is TradeOnNpcBase
                                          where trade.Buy is not null
                                          from buy in trade.Buy
                                          select buy.Name)
                                         .Union(
                                          from route in routes
                                          from trade in route.Trades
                                          where trade is TradeOnNpcBase
                                          where trade.Sell is not null
                                          from sell in trade.Sell
                                          select sell.Name)
                                         .Distinct()
                                         .ToArray();

            var npcTrades = (from route in routes
                             from trade in route.Trades
                             where trade is TradeOnNpcBase
                             select trade.Station)
                            .Distinct()
                            .ToArray();



            var npcTask = _client.GetNpcBasesAsync(new() { NicknameFilter = null }, token);
            var pobTask = _client.GetPlayerBasesAsync(token);
            var oreTask = _client.GetMiningZonesAsync(new() { NicknameFilter = null }, token);
            var commoditiesTask = _client.GetCommoditiesAsync(token);
            await Task.WhenAll(npcTask, pobTask, oreTask, commoditiesTask);
            /*var station_nicknames = (from npc in npcTask.Result
                                     where npcTrades.Contains(npc.Name)
                                     select npc.Nickname).ToArray();*/
            var commodityNicknames = (from commodity in commoditiesTask.Result
                                      where transportedCommodities.Contains(commodity.Name)
                                      select commodity.Nickname)
                                     .ToArray();
            var marketGoods = await _client.GetCommoditiesPerNicknameAsync(commodityNicknames, token);
            return new(npcTask.Result, pobTask.Result, oreTask.Result, _client);
        }
    }

    public sealed class SimulationDataSource(NpcBase[] NpcData,
                                             PlayerBase[] PobData,
                                             MiningZone[] OreFieldData,
                                             IRouteQueryClient RouteQueryClient)
    {
        public async Task<Dictionary<Route, TimeSpan?>> GetTravelTimesAsync(TradeRoute route, CancellationToken token = default)
        {
            var routes = GetRoutes(route).ToArray();
            var timings = await RouteQueryClient.GetTimingsAsync(routes);
            return timings.ToDictionary(t => t.Route, t => t.Time?.Transport);
        }

        public Route GetRoute(ITradeOnStation origin, ITradeOnStation destination)
        {
            var nicknameOrigin = origin.Station.Nickname;
            if (string.IsNullOrWhiteSpace(nicknameOrigin))
                nicknameOrigin = GetNickName(origin);
            var nicknameDestination = destination.Station.Nickname;
            if (string.IsNullOrWhiteSpace(nicknameDestination))
                nicknameDestination = GetNickName(destination);
            return new() { Origin = nicknameOrigin, Destination = nicknameDestination };
        }

        public IEnumerable<Route> GetRoutes(TradeRoute route)
        {
            for (int i = 0; i < route.Trades.Length; ++i)
            {
                yield return GetRoute(route.Trades[i], route.Trades[(i + 1) % route.Trades.Length]);
            }
        }

        public string GetNickName(ITradeOnStation trade)
            => trade switch
            {
                TradeOnMiningZone ore => OreFieldData.Single(field => field.Name == ore.MiningZone).Nickname,
                TradeOnNpcBase npc => NpcData.Single(station => station.Name == npc.Station.Name).Nickname,
                TradeOnPlayerBase npc => PobData.Single(station => station.Name == npc.Station.Name).Nickname,
                _ => throw new KeyNotFoundException()
            };

        public long GetBuyPrice(ITradeOnStation trade, string commodity)
            => trade switch
            {
                TradeOnMiningZone _ => 0,
                TradeOnNpcBase npc => (from station in NpcData
                                       where station.Nickname == trade.Station.Nickname
                                       from good in station.MarketGoods
                                       where good.Nickname == commodity
                                       select good.PriceBaseSellsFor).FirstOrDefault(),
                TradeOnPlayerBase pob => (from station in PobData
                                          where station.Nickname == trade.Station.Nickname
                                          from good in station.ShopItems
                                          where good.Nickname == commodity
                                          select good.Price).FirstOrDefault() ?? 0,
                _ => throw new KeyNotFoundException()
            };

        public long GetSellPrice(ITradeOnStation trade, string commodity)
            => trade switch
            {
                TradeOnNpcBase npc => (from station in NpcData
                                       where station.Nickname == trade.Station.Nickname
                                       from good in station.MarketGoods
                                       where good.Nickname == commodity
                                       select good.PriceBaseSellsFor).FirstOrDefault(),
                TradeOnPlayerBase pob => (from station in PobData
                                          where station.Nickname == trade.Station.Nickname
                                          from good in station.ShopItems
                                          where good.Nickname == commodity
                                          select good.Price).FirstOrDefault() ?? 0,
                _ => throw new KeyNotFoundException()
            };

        public int? GetCargoPurchaseLimit(ITradeOnStation trade)
        {
            int? limit = null;
            if (trade is TradeOnPlayerBase pobTrade)
            {
                var pobData = PobData.SingleOrDefault(station => station.Name == trade.Station.Name);
                if (pobData is not null)
                {
                    var marketData = pobData.ShopItems;
                    if (marketData is not null)
                    {
                        foreach (var acquisition in trade.Buy)
                        {
                            var item = marketData.SingleOrDefault(i => i.Name == acquisition.Name);
                            if (item is null)
                                limit = 0;
                            else
                            {
                                int timesToLoad = (int)((item.Quantity - item.MinStock) / acquisition.Count);
                                limit = Math.Min(limit ?? int.MaxValue, timesToLoad);
                            }
                        }
                    }
                }
            }
            return limit;
        }
        public int? GetCargoSaleLimit(ITradeOnStation trade)
        {
            int? limit = null;
            if (trade is TradeOnPlayerBase pobTrade)
            {
                var pobData = PobData.SingleOrDefault(station => station.Name == trade.Station.Name);
                if (pobData is not null)
                {
                    var marketData = pobData.ShopItems;
                    if (marketData is not null)
                    {
                        foreach (var disposal in trade.Sell)
                        {
                            var item = marketData.SingleOrDefault(i => i.Name == disposal.Name);
                            if (item is null)
                                limit = 0;
                            else
                            {
                                int timesToUnload = (int)((item.MaxStock - item.Quantity) / disposal.Count);
                                limit = Math.Min(limit ?? int.MaxValue, timesToUnload);
                            }
                        }
                    }
                }
            }
            return limit;
        }
    }
}
