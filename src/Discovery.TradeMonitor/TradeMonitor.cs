using Discovery.Darkstat;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor
{
    using Darkstat;
    public sealed class TradeMonitor(IDarkstatClient client)
    {
        private readonly SimulationDataSourceProvider _simulationProvider = new(client);

        public async Task<SimulationResult[]> GetTradeSimulations(TradeRoute[] routes, CancellationToken token = default)
        {
            var dataSource = await _simulationProvider.GetDataSourceAsync(routes, token);
            var simulationModel = new SimulationModel(dataSource);
            var tasks = new Task<SimulationResult>[routes.Length];
            for (int i = 0; i < routes.Length; ++i)
            {
                if (token.IsCancellationRequested)
                    break;
                tasks[i] = simulationModel.RunAsync(routes[i], token);
            }
            await Task.WhenAll(tasks);
            return tasks.Select(task => task.Result)
                        .OrderBy(r => r.CreditsPerSecond)
                        .ToArray();
        }
    }
}
