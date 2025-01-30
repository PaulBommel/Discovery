using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public sealed class CommodityQueryClient(IHttpClientFactory clientFactory) : ICommodityQueryClient
    {
        public Task<Commodity[]> GetCommoditiesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<Commodity[]>("/api/commodities", token);
        }
    }
}
