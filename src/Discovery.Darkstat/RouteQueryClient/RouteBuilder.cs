using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Darkstat.RouteQueryClient
{
    using NpcQueryClient;
    using OreFieldQueryClient;
    using PobQueryClient;

    using System.Linq;
    using System.Text.RegularExpressions;

    public class RouteBuilder
    {
        private readonly NpcQueryClient _npcQueryClient;
        private readonly OreFieldQueryClient _oreFieldClient;
        private readonly PobQueryClient _pobQueryClient;

        public RouteBuilder(IHttpClientFactory clientFactory)
            : this(new(clientFactory),
                   new(clientFactory),
                   new(clientFactory))
        {
            
        }

        public RouteBuilder(NpcQueryClient npcQueryClient,
                            OreFieldQueryClient oreFieldClient,
                            PobQueryClient pobQueryClient)
        {
            _npcQueryClient = npcQueryClient;
            _oreFieldClient = oreFieldClient;
            _pobQueryClient = pobQueryClient;
        }

        public async Task<Route> GetRouteByNameRegexAsync(string origin,
                                                         string destination,
                                                         CancellationToken token = default)
        {
            string originNickname = string.Empty, destinationNickname = string.Empty;
            foreach (var poi in await _npcQueryClient.GetAsync(token))
            {
                if (Regex.IsMatch(poi.Name, origin) && string.IsNullOrWhiteSpace(originNickname))
                    originNickname = poi.Nickname;
                if (Regex.IsMatch(poi.Name, destination) && string.IsNullOrWhiteSpace(destinationNickname))
                    destinationNickname = poi.Nickname;
                if (!(string.IsNullOrWhiteSpace(originNickname) || string.IsNullOrWhiteSpace(destinationNickname)))
                    break;
            }
            if (string.IsNullOrWhiteSpace(originNickname) || string.IsNullOrWhiteSpace(destinationNickname))
                foreach (var poi in await _oreFieldClient.GetAsync(token))
                {
                    if (Regex.IsMatch(poi.Name, origin) && string.IsNullOrWhiteSpace(originNickname))
                        originNickname = poi.Nickname;
                    if (Regex.IsMatch(poi.Name, destination) && string.IsNullOrWhiteSpace(destinationNickname))
                        destinationNickname = poi.Nickname;
                    if (!(string.IsNullOrWhiteSpace(originNickname) || string.IsNullOrWhiteSpace(destinationNickname)))
                        break;
                }
            if (string.IsNullOrWhiteSpace(originNickname) || string.IsNullOrWhiteSpace(destinationNickname))
                foreach (var poi in await _pobQueryClient.GetAsync(token))
                {
                    if (Regex.IsMatch(poi.Name, origin) && string.IsNullOrWhiteSpace(originNickname))
                        originNickname = poi.Nickname;
                    if (Regex.IsMatch(poi.Name, destination) && string.IsNullOrWhiteSpace(destinationNickname))
                        destinationNickname = poi.Nickname;
                    if (!(string.IsNullOrWhiteSpace(originNickname) || string.IsNullOrWhiteSpace(destinationNickname)))
                        break;
                }
            if (string.IsNullOrWhiteSpace(originNickname))
                originNickname = origin;
            if (string.IsNullOrWhiteSpace(destinationNickname))
                destinationNickname = destination;
            return new() { Origin = originNickname, Destination = destinationNickname };
        }

        public Task<Route> GetRouteByNameAsync(string origin,
                                              string destination,
                                              CancellationToken token = default)
                => GetRouteByNameRegexAsync($"^{origin}$", $"^{destination}$", token);
    }

    public static class RouteBuilderExtensions
    {
        public static Route RouteTo(this IPointOfInterest origin, IPointOfInterest destination)
        {
            return new()
            {
                Origin = origin.Nickname,
                Destination = destination.Nickname 
            };
        }
    }
}
