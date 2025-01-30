using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.PobQueryClient
{
    public class PobQueryClient(IHttpClientFactory clientFactory) : IPlayerBaseQueryClient
    {
        public Task<PlayerBase[]> GetPlayerBasesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<PlayerBase[]>("/api/pobs", token);
        }
    }
}
