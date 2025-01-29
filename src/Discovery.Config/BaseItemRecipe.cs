using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Discovery.Config
{
    [DebuggerDisplay($"{{{nameof(Quantity)}}}x {{{nameof(Item)}}}")]
    public readonly record struct ItemQuantity
    {
        public string Item { get; init; }
        public int Quantity { get; init; }
    }

    public readonly record struct AffiliationBonus
    {
        public double Factor { get; init; }
        public string FactionNickname { get; init; }
    }

    public readonly record struct ItemQuantityAffiliation
    {
        public ItemQuantity ProducedItemsWithFaction { get; init; }
        public ItemQuantity ProducedItems { get; init; }
        public string FactionNickname { get; init; }
    }

    [DebuggerDisplay($"{{{nameof(Infotext)}}}")]
    public record BaseItemRecipe
    {
        public string Nickname { get; init; }
        public string Infotext { get; init; }
        public int Shortcut { get; init; }
        public string CraftType { get; init; }
        public int CookingRate { get; init; }
        public int RequiredLevel { get; init; }
        public ItemQuantity Catalyst { get; init; }
        public bool LoopProduction { get; init; }
        public bool IsRestricted { get; init; }
        public ItemQuantity[] ProducedItems { get; init; }
        public ItemQuantityAffiliation[] ProducedByAffiliation { get; init; }
        public AffiliationBonus[] AffiliationBoni { get; init; }

        public ItemQuantity[][] ConsumeGroups { get; set; }
    }
}
