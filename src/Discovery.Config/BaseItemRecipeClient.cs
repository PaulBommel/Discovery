using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public sealed class BaseItemRecipeClient(IHttpClientFactory clientFactory)
    {
        public async Task<IEnumerable<BaseItemRecipe>> GetRecipesAsync(CancellationToken token = default,
                                                                       string uri = "base_recipe_items.cfg")
        {
            var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(uri, token);
            response.EnsureSuccessStatusCode();

            var str = await response.Content.ReadAsStringAsync(token);
            return BaseItemRecipeParser.ParseString(str);
        }
    }
}
