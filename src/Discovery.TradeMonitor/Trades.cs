using System.Linq;
using System.Text.Json.Serialization;

namespace Discovery.TradeMonitor
{
    public readonly record struct TradeRoute(ShipInfo Ship,
                                             ITradeOnStation[] Trades,
                                             string Name,
                                             string Category)
    {
        public string GetDefaultName()
            => string.Join(" -> ", Trades.Select(t => t.Station.Name).Union([Trades[0].Station.Name]));
    }

    public readonly record struct Location
    {
        public required string Name { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Nickname { get; init; }

        public static implicit operator Location(string value) 
            => new() { Name = value };
    }

    [JsonPolymorphic(IgnoreUnrecognizedTypeDiscriminators = true,
                     TypeDiscriminatorPropertyName = "DestinationType",
                     UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
    [JsonDerivedType(typeof(TradeOnPlayerBase), "Pob")]
    [JsonDerivedType(typeof(TradeOnNpcBase), "Npc")]
    [JsonDerivedType(typeof(TradeOnMiningZone), "Mining")]
    public interface ITradeOnStation
    {
        Location Station { get; }
        CargoInShip[] Buy { get; }
        CargoInShip[] Sell { get; }
    }

    public readonly record struct CargoInShip
    {
        [JsonConstructor]
        public CargoInShip(string name, string nickname, long count)
        {
            Name = name;
            Nickname = nickname;
            Count = count;
        }
        public CargoInShip(string name, long count)
            : this(name, null, count) { }

        public string Name { get; init; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Nickname { get; init; }
        public long Count { get; init; }
    }

    public readonly record struct TradeOnPlayerBase(Location Station,
                                                    [field:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                                    CargoInShip[] Buy,
                                                    [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                                    CargoInShip[] Sell) : ITradeOnStation;

    public readonly record struct TradeOnNpcBase(Location Station,
                                                 [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                                 CargoInShip[] Buy,
                                                 [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                                 CargoInShip[] Sell) : ITradeOnStation;
    public readonly record struct TradeOnMiningZone(Location MiningZone,
                                                    [field: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                                    CargoInShip[] Buy) : ITradeOnStation
    {
        [JsonIgnore]
        Location ITradeOnStation.Station => MiningZone;

        [JsonIgnore]
        CargoInShip[] ITradeOnStation.Sell => null;
    }
}
