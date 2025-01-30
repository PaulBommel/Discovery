using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.PobQueryClient
{
    public class PobQueryClient(IHttpClientFactory clientFactory) : IPlayerBaseQueryClient
    {
        public Task<PobData[]> GetPlayerBasesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<PobData[]>("/api/pobs", token);
        }
    }
}
