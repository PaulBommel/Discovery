using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Discovery.Config
{
    [DebuggerDisplay($"{{{nameof(InfoText)}}}")]
    public record BaseModuleRecipe
    {
        public string Nickname { get; init; }
        public string InfoText { get; init; }
        public string BuildType { get; init; }
        public ImmutableArray<string> CraftLists { get; init; }
        public int RecipeNumber { get; init; }
        public int ModuleClass { get; init; }
        public int CookingRate { get; init; }
        public int RequiredLevel { get; init; }
        public int CargoStorage { get; init; }
        public string ProducedItem { get; init; }
        public int CreditCost { get; init; }
        public ImmutableArray<CommodityQuantity> Inputs { get; init; } = ImmutableArray<CommodityQuantity>.Empty;
    }
}
