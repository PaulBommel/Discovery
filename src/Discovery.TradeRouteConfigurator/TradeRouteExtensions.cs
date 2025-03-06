using System;
using System.Threading.Tasks;

namespace Discovery.TradeRouteConfigurator
{
    using Darkstat;
    using TradeMonitor;
    using Views;
    using ViewModels;
    using System.Threading;
    using System.Linq;
    using System.Collections.Generic;
    using System.Windows.Xps.Packaging;
    using System.Diagnostics;

    public static class TradeRouteExtensions
    {
        public static async Task<TradeRoute> ShowConfiguratorDialog(this TradeRoute route, IDarkstatClient client, CancellationToken token = default)
        {
            var locationTask = GetLocations(client, token);
            var shipTask = client.GetShipInfosAsync(token);
            await Task.WhenAll(locationTask, shipTask);
            var viewModel = new ConfiguratorViewModel(route.Ship.ToShipViewModel(shipTask.Result), route.ToTradesViewModel([.. locationTask.Result]))
            {
                Name = route.Name,
                Category = route.Category
            };
            var dialog = new ConfiguratorView() 
            { 
                DataContext = viewModel 
            };
            viewModel.SaveCommand = new DelegateCommand(p => dialog.DialogResult = true);
            if (dialog.ShowDialog() == true)
            {
                route = new TradeRoute()
                {
                    Name = viewModel.Name,
                    Category = viewModel.Category,
                    Ship = viewModel.Ship.ToShipInfo(),
                    Trades = [.. viewModel.Trades.ToTradeOnStation()]
                };
            }
            return route;
        }

        public static ShipViewModel ToShipViewModel(this Discovery.TradeMonitor.ShipInfo shipinfo, Darkstat.ShipInfo[] shipInfoSource)
            => new(shipInfoSource)
            {
                SelectedShipClass = (ShipClass?)shipinfo.ShipClass,
                ShipName = shipinfo.Name,
                CargoCapacity = shipinfo.CargoCapacity
            };

        public static Discovery.TradeMonitor.ShipInfo ToShipInfo(this ShipViewModel viewmodel)
            => new(viewmodel.ShipName, viewmodel.CargoCapacity, (int)viewmodel.SelectedShipClass );

        public static TradesViewModel ToTradesViewModel(this TradeRoute route, ILocation[] locations)
        {
            var viewModel = new TradesViewModel(locations);
            if(route.Trades is not null)
                foreach(var trade in route.Trades)
                {
                    var location = locations.Single(l => l.Name == trade.Station.Name);
                    location.GetAvailableGoods(out var buyCommodities, out var sellCommodities);
                    viewModel.Trades.Add(new(trade, buyCommodities, sellCommodities));
                }
            return viewModel;
        }

        public static IEnumerable<ITradeOnStation> ToTradeOnStation(this TradesViewModel viewmodel)
        {
            foreach(var trade in viewmodel.Trades)
            {
                yield return trade.ToTradeOnStation();
            }
        }

        internal static async Task<IEnumerable<ILocation>> GetLocations(IDarkstatClient client, CancellationToken token = default)
        {
            var npcTask = client.GetNpcBasesAsync(new() { NicknameFilter = null }, token);
            var pobTask = client.GetPlayerBasesAsync(token);
            var oreTask = client.GetMiningZonesAsync(new() { NicknameFilter = null }, token);
            await Task.WhenAll(npcTask, pobTask, oreTask);
            var npcs = npcTask.Result.Cast<ILocation>();
            var pobs = pobTask.Result.Cast<ILocation>();
            var zones = oreTask.Result.Cast<ILocation>();
            return npcs.Union(pobs).Union(zones);
        }

        internal static void GetAvailableGoods(this ILocation location, out Models.Commodity[] buyCommodities, out Models.Commodity[] sellCommodities)
        {
            switch(location)
            {
                case NpcBase npc:
                    buyCommodities = [.. npc.MarketGoods.Where(g => g.BaseSells).Select(good => new Models.Commodity() { Name = good.Name, NickName = good.Nickname })];
                    sellCommodities = [.. npc.MarketGoods.Where(g => g.PriceBaseBuysFor.HasValue).Select(good => new Models.Commodity() { Name = good.Name, NickName = good.Nickname })];
                    break;
                case PlayerBase pob:
                    buyCommodities = [.. pob.ShopItems.Where(g => g.SellPrice.HasValue).Select(good => new Models.Commodity() { Name = good.Name, NickName = good.Nickname })];
                    sellCommodities = [.. pob.ShopItems.Select(good => new Models.Commodity() { Name = good.Name, NickName = good.Nickname })];
                    break;
                case MiningZone zone:
                    buyCommodities = [new Models.Commodity() { Name = zone.MiningInfo.MinedGood.Name, NickName = zone.MiningInfo.MinedGood.Nickname }];
                    sellCommodities = null;
                        break;
                default:
                    buyCommodities = null;
                    sellCommodities = null;
                    break;
            };
        }
    }
}
