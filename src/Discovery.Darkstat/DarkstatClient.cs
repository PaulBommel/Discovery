using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    public class DarkstatClient(IHttpClientFactory clientFactory) : IDarkstatClient
    {
        public async Task<MarketGoodResponse[]> GetMarketGoodsPerBasesAsync(string[] baseNicknames, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/npc_bases/market_goods", baseNicknames, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MarketGoodResponse[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

        public async Task<MarketGoodResponse[]> GetCommoditiesPerNicknameAsync(string[] commodityNicknames, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/commodities/market_goods", commodityNicknames, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MarketGoodResponse[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

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
                var str = await response.Content.ReadAsStringAsync(token);
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<MiningZone[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

        public Task<PlayerBase[]> GetPlayerBasesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<PlayerBase[]>("/api/pobs", token);
        }

        public async Task<RouteResponse[]> GetTimingsAsync(Route[] routes, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/graph/paths", routes, token);
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<RouteResponse[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

        public Task<Commodity[]> GetCommoditiesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<Commodity[]>("/api/commodities", token);
        }

        public Task<ShipInfo[]> GetShipInfosAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            return client.GetFromJsonAsync<ShipInfo[]>("/api/ships", token);
        }
    }
}
