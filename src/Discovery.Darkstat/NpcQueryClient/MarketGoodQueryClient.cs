using Discovery.Darkstat.RouteQueryClient;

using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.NpcQueryClient
{
    public sealed class MarketGoodQueryClient(IHttpClientFactory clientFactory) : IMarkedGoodQueryClient
    {
        public async Task<MarketGoodData[]> GetMarketGoodsPerBasesAsync(string[] baseNicknames, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/npc_bases/market_goods", baseNicknames, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MarketGoodData[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

        public async Task<MarketGoodData[]> GetCommoditiesPerNicknameAsync(string[] commodityNicknames, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/commodities/market_goods", commodityNicknames, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MarketGoodData[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }
    }
}
