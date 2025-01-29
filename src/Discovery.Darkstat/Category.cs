using System.Text.Json.Serialization;

namespace Discovery.Darkstat
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        [JsonStringEnumMemberName("cloakingdevice")]
        Cloakingdevice,
        [JsonStringEnumMemberName("commodity")]
        Commodity,
        [JsonStringEnumMemberName("countermeasure")]
        Countermeasure,
        [JsonStringEnumMemberName("countermeasuredropper")]
        Countermeasuredropper,
        [JsonStringEnumMemberName("engine")]
        Engine,
        [JsonStringEnumMemberName("gun")]
        Gun,
        [JsonStringEnumMemberName("mine")]
        Mine,
        [JsonStringEnumMemberName("minedropper")]
        MineDropper,
        [JsonStringEnumMemberName("munition")]
        Munition,
        [JsonStringEnumMemberName("repairkit")]
        Repairkit,
        [JsonStringEnumMemberName("scanner")]
        Scanner,
        [JsonStringEnumMemberName("shieldbattery")]
        Shieldbattery,
        [JsonStringEnumMemberName("shieldgenerator")]
        Shieldgenerator,
        [JsonStringEnumMemberName("ship")]
        Ship,
        [JsonStringEnumMemberName("thruster")]
        Thruster,
        [JsonStringEnumMemberName("tractor")]
        Tractor
    };
}
