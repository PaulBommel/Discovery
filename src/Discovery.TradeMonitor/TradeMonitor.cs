using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor
{
    public sealed class TradeMonitor(IHttpClientFactory httpClientFactory)
    {
        private readonly SimulationDataSourceProvider _simulationProvider = new(httpClientFactory);

        public async Task<SimulationResult[]> GetTradeSimulations(TradeRoute[] routes, CancellationToken token = default)
        {
            var dataSource = await _simulationProvider.GetDataSource(routes, token);
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
