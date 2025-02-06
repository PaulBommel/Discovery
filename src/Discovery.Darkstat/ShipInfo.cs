using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Darkstat
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public record ShipInfo
    {
        [JsonPropertyName("nickname")]
        public required string Nickname { get; init; }

        [JsonPropertyName("name")]
        public required string Name { get; init; }

        [JsonPropertyName("class")]
        public required ShipClass ShipClass { get; init; }

        [JsonPropertyName("type")]
        public required ShipType ShipType { get; init; }

        [JsonPropertyName("price")]
        public long Price { get; init; }

        [JsonPropertyName("armor")]
        public long Armor { get; init; }

        [JsonPropertyName("hold_size")]
        public long HoldSize { get; init; }

        [JsonPropertyName("nanobots")]
        public long Nanobots { get; init; }

        [JsonPropertyName("batteries")]
        public long Batteries { get; init; }

        [JsonPropertyName("mass")]
        public long Mass { get; init; }

        [JsonPropertyName("power_capacity")]
        public long PowerCapacity { get; init; }

        [JsonPropertyName("power_recharge_rate")]
        public long PowerRechargeRate { get; init; }

        [JsonPropertyName("cruise_speed")]
        public long CruiseSpeed { get; init; }

        [JsonPropertyName("linear_drag")]
        public double LinearDrag { get; init; }

        [JsonPropertyName("engine_max_force")]
        public long EngineMaxForce { get; init; }

        [JsonPropertyName("impulse_speed")]
        public double ImpulseSpeed { get; init; }

        [JsonPropertyName("thruster_speed")]
        public long[] ThrusterSpeed { get; init; }

        [JsonPropertyName("reverse_fraction")]
        public double ReverseFraction { get; init; }

        [JsonPropertyName("thrust_capacity")]
        public long ThrustCapacity { get; init; }

        [JsonPropertyName("thrust_recharge")]
        public long ThrustRecharge { get; init; }

        [JsonPropertyName("max_ansgular_speed")]
        public double MaxAnsgularSpeed { get; init; }

        [JsonPropertyName("angular_distance_from_0_to_halfsec")]
        public double AngularDistanceFrom0_ToHalfsec { get; init; }

        [JsonPropertyName("time_to_90_max_angular_speed")]
        public double TimeTo90_MaxAngularSpeed { get; init; }

        [JsonPropertyName("nudge_force")]
        public long NudgeForce { get; init; }

        [JsonPropertyName("strafe_force")]
        public double StrafeForce { get; init; }

        [JsonPropertyName("name_id")]
        public long NameId { get; init; }

        [JsonPropertyName("info_id")]
        public long InfoId { get; init; }

        [JsonPropertyName("equipment_slots")]
        public EquipmentSlot[] EquipmentSlots { get; init; }

        [JsonPropertyName("biggest_hardpoint")]
        public string[] BiggestHardpoint { get; init; }

        [JsonPropertyName("ship_packages")]
        public ShipPackage[] ShipPackages { get; init; }

        [JsonPropertyName("discovery_ship")]
        public DiscoveryShip DiscoveryShip { get; init; }
    }

    public readonly record struct DiscoveryShip
    {
        [JsonPropertyName("armor_mult")]
        public long ArmorMultiplier { get; init; }
    }

    public readonly record struct EquipmentSlot
    {
        [JsonPropertyName("SlotName")]
        public string SlotName { get; init; }

        [JsonPropertyName("AllowedEquip")]
        public string[] AllowedEquipment { get; init; }
    }

    public readonly record struct ShipPackage
    {
        [JsonPropertyName("Nickname")]
        public string Nickname { get; init; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ShipType
    {
        [JsonStringEnumMemberName("capital")]
        Capital,
        [JsonStringEnumMemberName("cruiser")]
        Cruiser,
        [JsonStringEnumMemberName("fighter")]
        Fighter,
        [JsonStringEnumMemberName("freighter")]
        Freighter,
        [JsonStringEnumMemberName("gunboat")]
        Gunboat,
        [JsonStringEnumMemberName("transport")]
        Transport 
    }; 
    
    public enum ShipClass
    {
        LightFighter = 0,
        HeavyFighter = 1,
        Freighter = 2,
        VeryHeavyFighter = 3,
        Bomber = 4,
        MediumTransport = 6,
        MediumTrain = 7,
        Battletransport = 8,
        Train = 9,
        Liner = 10,
        Gunboat = 11,
        Frigate = 12,
        Cruiser = 13,
        Miner = 14,
        BattleCruiser = 15,
        Battleship = 16,
        HeavyBattleship = 18,
        RepairShip = 19,
    };
}
