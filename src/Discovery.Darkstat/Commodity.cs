using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public record Commodity
    {
        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("price_base")]
        public long PriceBase { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("combinable")]
        public bool IsStackable { get; init; }

        [JsonPropertyName("volume")]
        public double Volume { get; init; }

        [JsonPropertyName("ship_class")]
        public long? ShipClass { get; init; }

        [JsonPropertyName("name_id")]
        public long NameId { get; init; }

        [JsonPropertyName("infocard_id")]
        public long InfocardId { get; init; }

        [JsonPropertyName("infocard_key")]
        public string InfocardKey { get; init; }

        [JsonPropertyName("price_best_base_buys_for")]
        public long PriceBestBaseBuysFor { get; init; }

        [JsonPropertyName("price_best_base_sells_for")]
        public long PriceBestBaseSellsFor { get; init; }

        [JsonPropertyName("proffit_margin")]
        public long ProfitMargin { get; init; }

        [JsonPropertyName("mass")]
        public long Mass { get; init; }
    }
}
