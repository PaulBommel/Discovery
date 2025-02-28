using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.OreFieldQueryClient
{
    public class MiningZoneQueryClient(IHttpClientFactory clientFactory) : IMiningZoneQueryClient
    {
        [Obsolete]
        public Task<MiningZone[]> GetMiningZonesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<MiningZone[]>("/api/mining_operations", token);
        }

        public async Task<MiningZone[]> GetMiningZonesAsync(BaseQueryParameter parameter, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/mining_operations", parameter, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MiningZone[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }
    }
}
