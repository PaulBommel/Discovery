using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public readonly record struct MiningZone : IPointOfInterest
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("archetypes")]
        public string[] Archetypes { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("faction_nickname")]
        public required string FactionNickname { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("system_name")]
        public required string SystemName { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("system_nickname")]
        public required string SystemNickname { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("region_name")]
        public required string RegionName { get; init; }

        [JsonPropertyName("strid_name")]
        public long StridName { get; init; }

        [JsonPropertyName("infocard_id")]
        public long InfocardId { get; init; }

        [JsonPropertyName("infocard")]
        public Infocard[] Infocard { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("InfocardKey")]
        public string InfocardKey { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("file")]
        public string File { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("BGCS_base_run_by")]
        public string BgcsBaseRunBy { get; init; }

        [OptionalVector3JsonConverter]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pos")]
        public Vector3? Position { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("sector_coord")]
        public string SectorCoord { get; init; }

        [JsonPropertyName("is_transport_unreachable")]
        public bool IsTransportUnreachable { get; init; }

        [JsonPropertyName("mining_info")]
        public MiningInfo MiningInfo { get; init; }

        [JsonPropertyName("is_reachhable")]
        public bool IsReachhable { get; init; }
    }

    public readonly record struct MiningInfo
    {
        [JsonPropertyName("DynamicLootMin")]
        public long DynamicLootMin { get; init; }

        [JsonPropertyName("DynamicLootMax")]
        public long DynamicLootMax { get; init; }

        [JsonPropertyName("DynamicLootDifficulty")]
        public long DynamicLootDifficulty { get; init; }

        [JsonPropertyName("MinedGood")]
        public required MinedGood MinedGood { get; init; }
    }

    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public readonly record struct MinedGood
    {
        [JsonPropertyName("Name")]
        public required string Name { get; init; }

        [JsonPropertyName("Nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("HpType")]
        public string HpType { get; init; }

        [JsonPropertyName("Type")]
        public Category Type { get; init; }

        [JsonPropertyName("LevelRequired")]
        public long LevelRequired { get; init; }

        [JsonPropertyName("RepRequired")]
        public long RepRequired { get; init; }

        [JsonPropertyName("Infocard")]
        public string Infocard { get; init; }

        [JsonPropertyName("BaseSells")]
        public bool BaseSells { get; init; }

        [JsonPropertyName("PriceModifier")]
        public long PriceModifier { get; init; }

        [JsonPropertyName("PriceBase")]
        public long PriceBase { get; init; }

        [JsonPropertyName("PriceToBuy")]
        public long PriceToBuy { get; init; }

        [JsonPropertyName("PriceToSell")]
        public long PriceToSell { get; init; }

        [JsonPropertyName("Volume")]
        public double Volume { get; init; }

        [JsonPropertyName("ShipClass")]
        public long ShipClass { get; init; }

        [JsonPropertyName("IsServerSideOverride")]
        public bool IsServerSideOverride { get; init; }
    }
}
