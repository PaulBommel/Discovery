using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.OreFieldQueryClient
{
    public class OreFieldQueryClient(IHttpClientFactory clientFactory) : IOreFieldQueryClient
    {
        public Task<OreFieldData[]> GetOreFieldsAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<OreFieldData[]>("/api/mining_operations", token);
        }
    }
}
