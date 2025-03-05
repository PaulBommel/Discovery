using System;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public readonly record struct NpcBase : ILocation
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
        [JsonPropertyName("faction_name")]
        public required string FactionName { get; init; }

        [JsonPropertyName("system_name")]
        public required string SystemName { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("system_nickname")]
        public required string SystemNickname { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("region_name")]
        public required string RegionName { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("strid_name")]
        public long? StridName { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("infocard_id")]
        public long? InfocardId { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("is_transport_unreachable")]
        public bool? IsTransportUnreachable { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("is_reachhable")]
        public bool? IsReachhable { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("market_goods")]
        public MarketGood[] MarketGoods { get; init; }
    }
}
