using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{
    public readonly record struct DeliveryRecord
    {
        public string Destination { get; init; }
        public DateTime? Time { get; init; }
        public int? Credits { get; init; }
        public ItemQuantityRecord[] Cargo { get; init; }
        public string ShipType { get; init; }
        public string Source { get; init; }
        public string SourceType { get; init; }
    
    }

    [DebuggerDisplay($"{{{nameof(Amount)}}}x {{{nameof(Commodity)}}}")]
    public readonly record struct ItemQuantityRecord(string Commodity, int Amount);
}
