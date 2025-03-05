using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor
{

    public sealed class SimulationModel(SimulationDataSource dataSource)
    {
        public async Task<SimulationResult> RunAsync(TradeRoute route, CancellationToken token = default)
        {
            var times = await dataSource.GetTravelTimesAsync(route, token);
            var routeResults = new RouteResult[route.Trades.Length];
            for(int i = 0; i < route.Trades.Length; ++i)
            {
                var origin = route.Trades[i];
                var destination = route.Trades[(i + 1) % route.Trades.Length];
                var purchase = GetCargoPurchase(origin);
                var sale = GetCargoSale(destination);
                var tradeProfit = sale?.Revenue - purchase?.Cost ?? 0;
                var timeKey = dataSource.GetRoute(origin, destination);
                var travelTime = times[timeKey] ?? TimeSpan.Zero;
                routeResults[i] = new RouteResult()
                {
                    Purchase = purchase,
                    Sale = sale,
                    Profit = tradeProfit,
                    TravelTime = travelTime,
                    CreditsPerSecond = tradeProfit / travelTime.TotalSeconds
                };
                
            }
            var cost = routeResults.Sum(route => route.Purchase?.Cost ?? 0);
            var revenue = routeResults.Sum(route => route.Sale?.Revenue ?? 0);
            var profit = revenue - cost;
            StockLimit stockLimit = null;
            foreach(var result in routeResults)
            {
                if (result.Purchase.Limit.HasValue && result.Purchase.Limit.Value < (stockLimit?.Limit ?? int.MaxValue))
                    stockLimit = new() { Limit = result.Purchase.Limit.Value, Station = result.Purchase.Origin, Reason = "purchase" };
                if (result.Sale.Limit.HasValue && result.Sale.Limit.Value < (stockLimit?.Limit ?? int.MaxValue))
                    stockLimit = new() { Limit = result.Sale.Limit.Value, Station = result.Sale.Destination, Reason = "sale" };
            }
            return new SimulationResult()
            {
                RouteResults = routeResults,
                TotalCost = cost,
                TotalProfit = profit,
                TotalRevenue = revenue,
                TravelTime = TimeSpan.FromSeconds(routeResults.Sum(route => route.TravelTime.TotalSeconds)),
                CreditsPerSecond = profit / routeResults.Sum(route => route.TravelTime.TotalSeconds),
                Ship = route.Ship,
                StockLimit = stockLimit
            };
        }

        

        private CargoPurchase GetCargoPurchase(ITradeOnStation trade)
        {
            if (trade.Buy is null)
                return new() { Origin = trade.Station.Name };
            int? limit = dataSource.GetCargoPurchaseLimit(trade);
            var prices = new long[trade.Buy.Length];
            var count = new long[trade.Buy.Length];
            long cost = 0;
            for(int i = 0; i < trade.Buy.Length; ++i)
            {
                count[i] = trade.Buy[i].Count;
                prices[i] = dataSource.GetBuyPrice(trade, trade.Buy[i].Nickname);
                cost += prices[i] * count[i];
            }
            return new()
            {
                Commodities = trade.Buy.Select(cargo => cargo.Name).ToArray(),
                Limit = limit,
                Cost = cost,
                Count = count,
                Origin = trade.Station.Name,
                Prices = prices
            };
        }
        private CargoSale GetCargoSale(ITradeOnStation trade)
        {
            if (trade.Sell is null)
                return new() { Destination = trade.Station.Name };
            int? limit = dataSource.GetCargoSaleLimit(trade);
            var prices = new long[trade.Sell.Length];
            var count = new long[trade.Sell.Length];
            long revenue = 0;
            for (int i = 0; i < trade.Sell.Length; ++i)
            {
                count[i] = trade.Sell[i].Count;
                prices[i] = dataSource.GetSellPrice(trade, trade.Sell[i].Nickname);
                revenue += prices[i] * count[i];
            }
            return new()
            {
                Commodities = trade.Sell.Select(cargo => cargo.Name).ToArray(),
                Limit = limit,
                Revenue = revenue,
                Count = count,
                Destination = trade.Station.Name,
                Prices = prices
            };
        }
    }

    public readonly record struct SimulationResult(long TotalCost,
                                                   long TotalRevenue,
                                                   long TotalProfit,
                                                   TimeSpan TravelTime,
                                                   double CreditsPerSecond,
                                                   RouteResult[] RouteResults,
                                                   ShipInfo Ship,
                                                   StockLimit StockLimit);

    public record class StockLimit
    {
        public required int Limit { get; init; }
        public required string Station { get; init; }
        public required string Reason { get; init; }
    }

    public readonly record struct RouteResult(CargoPurchase Purchase,
                                              CargoSale Sale,
                                              long Profit,
                                              TimeSpan TravelTime,
                                              double CreditsPerSecond);

    public record class CargoPurchase
    {
        public required string Origin { get; init; }
        public string[] Commodities { get; init; }
        public long[] Count { get; init; }
        public long[] Prices { get; init; }
        public long Cost { get; init; }
        public int? Limit { get; init; }
    }

    public record class CargoSale()
    {
        public required string Destination { get; init; }
        public string[] Commodities { get; init; }
        public long[] Count { get; init; }
        public long[] Prices { get; init; }
        public long Revenue { get; init; }
        public int? Limit { get; init; }
    }


}
