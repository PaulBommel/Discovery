using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public record PlayerBase : IPointOfInterest
    {
        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("pos")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Pos { get; init; }

        [JsonPropertyName("level")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Level { get; init; }

        [JsonPropertyName("money")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Money { get; init; }

        [JsonPropertyName("health")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Health { get; init; }

        [JsonPropertyName("defense_mode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? DefenseMode { get; init; }

        [JsonPropertyName("system_nickname")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SystemNickname { get; init; }

        [JsonPropertyName("system_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SystemName { get; init; }

        [JsonPropertyName("faction_nickname")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FactionNickname { get; init; }

        [JsonPropertyName("faction_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string FactionName { get; init; }

        [JsonPropertyName("forum_thread_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Uri ForumThreadUrl { get; init; }

        [OptionalVector3JsonConverter]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("base_pos")]
        public Vector3? Position { get; init; }

        [JsonPropertyName("sector_coord")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SectorCoord { get; init; }

        [JsonPropertyName("region_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RegionName { get; init; }

        [JsonPropertyName("shop_items")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ShopItem[] ShopItems { get; init; }

        [JsonPropertyName("infocard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Infocard[] Infocard { get; init; }

        [JsonPropertyName("cargospace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? CargoSpace { get; init; }
    }

    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public record ShopItem
    {
        [JsonPropertyName("id")]
        public required long Id { get; init; }

        [JsonPropertyName("quantity")]
        public long? Quantity { get; init; }

        [JsonPropertyName("price")]
        public long? Price { get; init; }

        [JsonPropertyName("sell_price")]
        public long? SellPrice { get; init; }

        [JsonPropertyName("min_stock")]
        public long? MinStock { get; init; }

        [JsonPropertyName("max_stock")]
        public long? MaxStock { get; init; }

        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("category")]
        public Category? Category { get; init; }
    }
}
