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

    public static class TradeRouteExtensions
    {
        public static async Task<TradeRoute> ShowConfiguratorDialog(this TradeRoute route, IDarkstatClient client, CancellationToken token = default)
        {
            var locationTask = GetLocations(client, token);
            var shipTask = client.GetShipInfosAsync(token);
            await Task.WhenAll(locationTask, shipTask);
            var dialog = new ConfiguratorView() 
            { 
                DataContext = new ConfiguratorViewModel(route, shipTask.Result, locationTask.Result.ToArray()) 
            };
            if (dialog.ShowDialog() == true)
            {

            }
            return route;
        }

        internal static async Task<IEnumerable<ILocation>> GetLocations(IDarkstatClient client, CancellationToken token = default)
        {
            var npcTask = client.GetNpcBasesAsync(new() { NicknameFilter = null }, token);
            var pobTask = client.GetPlayerBasesAsync(token);
            var oreTask = client.GetMiningZonesAsync(token);
            await Task.WhenAll(npcTask, pobTask, oreTask);
            var npcs = npcTask.Result.Cast<ILocation>();
            var pobs = pobTask.Result.Cast<ILocation>();
            var zones = oreTask.Result.Cast<ILocation>();
            return npcs.Union(pobs).Union(zones);
        }
    }
}
