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

    internal sealed class SimulationDataSourceProvider(IHttpClientFactory httpClientFactory)
    {
        private readonly NpcQueryClient _npcQueryClient = new(httpClientFactory);
        private readonly MarketGoodQueryClient _marketGoodQueryClient = new(httpClientFactory);
        private readonly CommodityQueryClient _commodityQueryClient = new(httpClientFactory);
        private readonly MiningZoneQueryClient _oreFieldQueryClient = new(httpClientFactory);
        private readonly PobQueryClient _pobQueryClient = new(httpClientFactory);
        private readonly ShipInfoQueryClient _shipInfoQueryClient = new(httpClientFactory);
        private readonly RouteQueryClient _routeQueryClient = new(httpClientFactory);

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



            var npcTask = _npcQueryClient.GetNpcBasesAsync(token);
            var pobTask = _pobQueryClient.GetPlayerBasesAsync(token);
            var oreTask = _oreFieldQueryClient.GetMiningZonesAsync(token);
            var commoditiesTask = _commodityQueryClient.GetCommoditiesAsync(token);
            await Task.WhenAll(npcTask, pobTask, oreTask, commoditiesTask);
            /*var station_nicknames = (from npc in npcTask.Result
                                     where npcTrades.Contains(npc.Name)
                                     select npc.Nickname).ToArray();*/
            var commodityNicknames = (from commodity in commoditiesTask.Result
                                      where transportedCommodities.Contains(commodity.Name)
                                      select commodity.Nickname)
                                     .ToArray();
            var marketGoods = await _marketGoodQueryClient.GetCommoditiesPerNicknameAsync(commodityNicknames, token);
            return new(npcTask.Result, pobTask.Result, oreTask.Result, marketGoods, _routeQueryClient);
        }
    }

    public sealed class SimulationDataSource(NpcBase[] NpcData,
                                             PlayerBase[] PobData,
                                             MiningZone[] OreFieldData,
                                             MarketGoodResponse[] MarketGoods,
                                             RouteQueryClient RouteQueryClient)
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
                TradeOnNpcBase npc => NpcData.Single(station => station.Name == npc.Station).Nickname,
                TradeOnPlayerBase npc => PobData.Single(station => station.Name == npc.Station).Nickname,
                _ => throw new KeyNotFoundException()
            };

        public long GetBuyPrice(ITradeOnStation trade, string commodity)
            => trade switch
            {
                TradeOnMiningZone _ => 0,
                TradeOnNpcBase npc => (from data in MarketGoods
                                      from good in data.MarketGoods
                                      where good.BaseName == trade.Station
                                      where good.Name == commodity
                                      select good.PriceBaseSellsFor).FirstOrDefault(),
                TradeOnPlayerBase npc => PobData
                                        .SingleOrDefault(station => station.Name == npc.Station)?
                                        .ShopItems
                                        .SingleOrDefault(item => item.Name == commodity)?
                                        .Price ?? 0,
                _ => throw new KeyNotFoundException()
            };

        public long GetSellPrice(ITradeOnStation trade, string commodity)
            => trade switch
            {
                TradeOnNpcBase npc => (from data in MarketGoods
                                       from good in data.MarketGoods
                                       where good.BaseName == trade.Station
                                       where good.Name == commodity
                                       select good.PriceBaseBuysFor).FirstOrDefault() ?? 0,
                TradeOnPlayerBase npc => PobData
                                        .SingleOrDefault(station => station.Name == npc.Station)?
                                        .ShopItems
                                        .SingleOrDefault(item => item.Name == commodity)?
                                        .SellPrice ?? 0,
                _ => throw new KeyNotFoundException()
            };

        public int? GetCargoPurchaseLimit(ITradeOnStation trade)
        {
            int? limit = null;
            if (trade is TradeOnPlayerBase pobTrade)
            {
                var pobData = PobData.SingleOrDefault(station => station.Name == trade.Station);
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
                var pobData = PobData.SingleOrDefault(station => station.Name == trade.Station);
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
