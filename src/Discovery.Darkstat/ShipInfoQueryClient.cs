using Discovery.Darkstat.NpcQueryClient;
using Discovery.Darkstat.PobQueryClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public sealed class ShipInfoQueryClient(IHttpClientFactory clientFactory) : IShipInfoQueryClient
    {
        public Task<ShipInfo[]> GetShipInfosAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<ShipInfo[]>("/api/ships", token);
        }
    }
}
