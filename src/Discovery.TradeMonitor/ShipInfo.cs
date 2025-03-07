using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Discovery.TradeMonitor
{
    [DebuggerDisplay($"{{{nameof(Name)}}}")]
    public readonly record struct ShipInfo(string Name,
                                           long CargoCapacity, 
                                           [field:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
                                           int? ShipClass);
}
