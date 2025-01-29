using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Darkstat.RouteQueryClient
{
    public readonly record struct RouteData
    {
        [JsonPropertyName("route")]
        public required Route Route { get; init; }
        [JsonPropertyName("error")]
        public string Error { get; init; }
        [JsonPropertyName("time")]
        public RouteTiming? Time { get; init; }
    }

    public readonly record struct Route
    {
        [JsonPropertyName("from")]
        public required string Origin { get; init; }
        [JsonPropertyName("to")]
        public required string Destination { get; init; }
    }


    public readonly record struct RouteTiming
    {
        [TimespanToSecondsConverter]
        [JsonPropertyName("transport")]
        public required TimeSpan Transport { get; init; }
        [TimespanToSecondsConverter]
        [JsonPropertyName("frigate")]
        public required TimeSpan Frigate { get; init; }
        [TimespanToSecondsConverter]
        [JsonPropertyName("freighter")]
        public required TimeSpan Freighter { get; init; }
    }
}
