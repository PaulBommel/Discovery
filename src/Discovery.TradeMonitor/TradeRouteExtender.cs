using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor
{
    using Darkstat;

    using System.Linq;

    public sealed class TradeRouteExtender(IDarkstatClient client)
    {
        public async Task<TradeRoute> ExtendAsync(TradeRoute route)
        {
            var npcBases = await client.GetNpcBasesAsync();
            var miningZones = await client.GetMiningZonesAsync();
            var playerBases = await client.GetPlayerBasesAsync();
            var commodities = await client.GetCommoditiesAsync();
            return route with { Trades = route.Trades.Extend(npcBases, miningZones, playerBases, commodities)?.ToArray() };
        }
    }

    public static class TradeRouteExtensions
    {
        public static IEnumerable<ITradeOnStation> Extend(this IEnumerable<ITradeOnStation> trades,
                                                          NpcBase[] npcBases,
                                                          MiningZone[] miningZones,
                                                          PlayerBase[] playerBases,
                                                          Commodity[] commodities)
        {
            foreach (var trade in trades)
                yield return trade.Extend(npcBases, miningZones, playerBases, commodities);
        }

        public static ITradeOnStation Extend(this ITradeOnStation trade,
                                             NpcBase[] npcBases,
                                             MiningZone[] miningZones,
                                             PlayerBase[] playerBases,
                                             Commodity[] commodities)
            => trade switch
            {
                TradeOnMiningZone miningZone => miningZone.Extend(miningZones, commodities),
                TradeOnNpcBase npcBase => npcBase.Extend(npcBases, commodities),
                TradeOnPlayerBase playerBase => playerBase.Extend(playerBases, commodities),
                _ => throw new ArgumentException()
            };

        public static TradeOnMiningZone Extend(this TradeOnMiningZone trade,
                                               MiningZone[] zones,
                                               Commodity[] commodities)
        {
            var location = trade.MiningZone with { Nickname = zones.Single(zone => zone.Name == trade.MiningZone.Name).Nickname };
            return trade with 
            { 
                MiningZone = location, 
                Buy = trade.Buy?.Extend(commodities)?.ToArray() 
            };
        }

        public static TradeOnNpcBase Extend(this TradeOnNpcBase trade,
                                            NpcBase[] bases,
                                            Commodity[] commodities)
        {
            var location = trade.Station with { Nickname = bases.Single(b => b.Name == trade.Station.Name).Nickname };
            return trade with
            {
                Station = location,
                Buy = trade.Buy?.Extend(commodities)?.ToArray(),
                Sell = trade.Sell?.Extend(commodities)?.ToArray()
            };
        }

        public static TradeOnPlayerBase Extend(this TradeOnPlayerBase trade,
                                               PlayerBase[] bases,
                                               Commodity[] commodities)
        {
            var location = trade.Station with { Nickname = bases.Single(b => b.Name == trade.Station.Name).Nickname };
            return trade with
            {
                Station = location,
                Buy = trade.Buy?.Extend(commodities)?.ToArray(),
                Sell = trade.Sell?.Extend(commodities)?.ToArray()
            };
        }

        public static IEnumerable<CargoInShip> Extend(this CargoInShip[] cargos,
                                                      Commodity[] commodities)
        {
            if (cargos is null)
                yield break;
            foreach (var cargo in cargos)
                yield return cargo with { Nickname = commodities.Single(c => c.Name == cargo.Name).Nickname };
        }
    }
}
