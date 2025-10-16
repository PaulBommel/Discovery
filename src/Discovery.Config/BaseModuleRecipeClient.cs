using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public sealed class BaseModuleRecipeClient(IHttpClientFactory clientFactory)
    {
        public async Task<IEnumerable<BaseModuleRecipe>> GetRecipesAsync(CancellationToken token = default,
                                                                       string uri = "base_recipe_modules.cfg")
        {
            var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(uri, token);
            response.EnsureSuccessStatusCode();

            var str = await response.Content.ReadAsStringAsync(token);
            return BaseModuleRecipeParser.ParseString(str);
        }

        public async IAsyncEnumerable<BaseModuleRecipe> EnumerateRecipesAsync([EnumeratorCancellation] CancellationToken token = default,
                                                                              string uri = "base_recipe_modules.cfg")
        {
            var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, token);
            response.EnsureSuccessStatusCode();
            
            await using var stream = await response.Content.ReadAsStreamAsync(token);
            using var reader = new StreamReader(stream);

            await foreach (var recipe in BaseModuleRecipeParser.ParseAsync(reader, token))
            {
                yield return recipe;
            }
        }
    }
}
