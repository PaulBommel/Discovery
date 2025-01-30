using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.RouteQueryClient
{
    public class RouteQueryClient(IHttpClientFactory clientFactory) : IRouteQueryClient
    {
        public async Task<RouteData[]> GetTimingsAsync(Route[] routes, CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/graph/paths", routes, token);
            if(response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync(token))
                    return await JsonSerializer.DeserializeAsync<RouteData[]>(stream, JsonSerializerOptions.Default, token);
            }
            return [];
        }

        public async Task<IDictionary<Route, RouteTiming>> GetTimingDictionaryAsync(Route[] routes, CancellationToken token = default)
        {
            var data = await GetTimingsAsync(routes, token);
            if(data.Any())
            {
                var dictionary = new ConcurrentDictionary<Route, RouteTiming>(new RouteKeyComparer());
                data.AsParallel().ForAll(data =>
                {
                    if (data.Time.HasValue)
                        dictionary.AddOrUpdate(data.Route, data.Time.Value, (key, value) => value);
                });
                return dictionary;
            }
            return null;
        }

        private class RouteKeyComparer : IEqualityComparer<Route> {
            public bool Equals(Route x, Route y)
            => (x.Origin == y.Origin && x.Destination == y.Destination)
            || (x.Origin == y.Destination && x.Destination == y.Origin);

            public int GetHashCode([DisallowNull] Route obj)
                => obj.GetHashCode();
        }
    }
}
