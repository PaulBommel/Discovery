using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Config
{
    [DebuggerDisplay($"{{{nameof(Amount)}}}x {{{nameof(Commodity)}}}")]
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type", 
                     UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToBaseType)]
    [JsonDerivedType(typeof(AffiliationCommodityQuantity), "affiliated")]
    public record CommodityQuantity(string Commodity, int Amount, int? GroupId = null);

    [DebuggerDisplay($"{{{nameof(Amount)}}}x {{{nameof(Commodity)}}} ({{{nameof(Faction)}}})")]
    public record AffiliationCommodityQuantity(string Commodity, int Amount, string Faction)
    : CommodityQuantity(Commodity, Amount);
}
