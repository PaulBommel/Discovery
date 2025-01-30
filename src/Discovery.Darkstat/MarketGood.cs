using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Nickname)}}}")]
    public readonly record struct MarketGoodResponse
    {
        [JsonPropertyName("market_goods")]
        public MarketGood[] MarketGoods { get; init; }

        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("error")]
        public object Error { get; init; }
    }

    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public readonly record struct MarketGood
    {
        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("ship_nickname")]
        public string ShipNickname { get; init; }

        [JsonPropertyName("nickname_hash")]
        public long? NicknameHash { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("price_base")]
        public long PriceBase { get; init; }

        [JsonPropertyName("hp_type")]
        public string HpType { get; init; }

        [JsonPropertyName("category")]
        public required Category Category { get; init; }

        [JsonPropertyName("level_required")]
        public long LevelRequired { get; init; }

        [JsonPropertyName("rep_required")]
        public double RepRequired { get; init; }

        [JsonPropertyName("price_base_buys_for")]
        public long? PriceBaseBuysFor { get; init; }

        [JsonPropertyName("price_base_sells_for")]
        public long PriceBaseSellsFor { get; init; }

        [JsonPropertyName("volume")]
        public double Volume { get; init; }

        [JsonPropertyName("ship_class")]
        public long ShipClass { get; init; }

        [JsonPropertyName("base_sells")]
        public bool BaseSells { get; init; }

        [JsonPropertyName("is_server_override")]
        public bool IsServerOverride { get; init; }

        [JsonPropertyName("base_nickname")]
        public string BaseNickname { get; init; }

        [JsonPropertyName("base_name")]
        public string BaseName { get; init; }

        [JsonPropertyName("system_name")]
        public string SystemName { get; init; }

        [JsonPropertyName("region_name")]
        public string RegionName { get; init; }

        [JsonPropertyName("faction_name")]
        public string FactionName { get; init; }

        [OptionalVector3JsonConverter]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("base_pos")]
        public Vector3? Position { get; init; }

        [JsonPropertyName("sector_coord")]
        public string SectorCoord { get; init; }
    }
}
