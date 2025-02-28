using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.NpcQueryClient
{
    public class NpcQueryClient(IHttpClientFactory clientFactory) : INpcBaseQueryClient
    {
        [Obsolete]
        public Task<NpcBase[]> GetNpcBasesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<NpcBase[]>("/api/npc_bases", token);
        }

        public async Task<NpcBase[]> GetNpcBasesAsync(BaseQueryParameter parameter, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/npc_bases", parameter, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<NpcBase[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }
    }
}
