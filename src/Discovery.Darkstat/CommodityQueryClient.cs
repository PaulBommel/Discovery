using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public sealed class CommodityQueryClient(IHttpClientFactory clientFactory)
    {
        public Task<Commodity[]> GetAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<Commodity[]>("/api/commodities", token);
        }
    }
}
