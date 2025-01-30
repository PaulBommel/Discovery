using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.NpcQueryClient
{
    public class NpcQueryClient(IHttpClientFactory clientFactory) : INpcBaseQueryClient
    {
        public Task<NpcBase[]> GetNpcBasesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<NpcBase[]>("/api/npc_bases", token);
        }
    }
}
