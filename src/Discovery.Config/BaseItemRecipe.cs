using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Discovery.Config
{
    [DebuggerDisplay($"{{{nameof(InfoText)}}}")]
    public record BaseItemRecipe
    {
        public string Nickname { get; init; }
        public string InfoText { get; init; }
        public int ShortcutNumber { get; init; }
        public string CraftType { get; init; }
        public int CookingRate { get; init; }
        public int RequiredLevel { get; init; }
        public bool Restricted { get; init; }
        public ImmutableArray<CommodityQuantity> Inputs { get; init; } = ImmutableArray<CommodityQuantity>.Empty;
        public ImmutableArray<CommodityQuantity> Outputs { get; init; } = ImmutableArray<CommodityQuantity>.Empty;
        public ImmutableDictionary<string, double> AffiliationBonuses { get; init; } = ImmutableDictionary<string, double>.Empty;
    }
}
