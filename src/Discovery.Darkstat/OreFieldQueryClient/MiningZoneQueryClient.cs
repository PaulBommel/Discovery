using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.OreFieldQueryClient
{
    public class MiningZoneQueryClient(IHttpClientFactory clientFactory) : IMiningZoneQueryClient
    {
        public Task<MiningZone[]> GetMiningZonesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<MiningZone[]>("/api/mining_operations", token);
        }
    }
}
