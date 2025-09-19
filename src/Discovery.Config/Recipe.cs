using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config
{
    [DebuggerDisplay($"{{{nameof(InfoText)}}}")]
    public class Recipe
    {
        public string Nickname { get; set; }
        public string InfoText { get; set; }
        public int ShortcutNumber { get; set; }
        public string CraftType { get; set; }
        public int CookingRate { get; set; }
        public int RequiredLevel { get; set; }
        public bool Restricted { get; set; }

        // Zutaten: Pflicht (GroupId = null) oder Alternativen (GroupId != null)
        public List<CommodityQuantity> Inputs { get; set; } = new();

        // Outputs: normal oder fraktionsabhängig
        public List<CommodityQuantity> Outputs { get; set; } = new();

        // Bonus pro Fraktion
        public Dictionary<string, double> AffiliationBonuses { get; set; } = new();

        /// <summary>
        /// Liefert ein Rezept-Ergebnis mit alternativen Zutaten (noch nicht expandiert).
        /// Default: erste Alternative jeder Gruppe wird gewählt.
        /// </summary>
        public RecipeResult GetResultForFaction(string faction, Func<IEnumerable<CommodityQuantity>, CommodityQuantity>? altSelector = null)
        {
            double bonus = AffiliationBonuses.TryGetValue(faction, out var factor) ? factor : 1.0;

            if (Restricted && !AffiliationBonuses.ContainsKey(faction))
                return null;

            var grouped = Inputs.GroupBy(i => i.GroupId);

            var inputs = new List<CommodityQuantity>();
            foreach (var group in grouped)
            {
                if (group.Key is null)
                {
                    // Pflichtzutaten
                    inputs.AddRange(group.Select(i =>
                        new CommodityQuantity(i.Commodity, (int)Math.Ceiling(i.Amount * bonus))));
                }
                else
                {
                    // Alternative Gruppe → Auswahlstrategie verwenden
                    var chosen = altSelector?.Invoke(group) ?? group.First();
                    inputs.Add(new CommodityQuantity(chosen.Commodity, (int)Math.Ceiling(chosen.Amount * bonus), chosen.GroupId));
                }
            }

            var outputs = Outputs
                .Where(o => o is not AffiliationCommodityQuantity aff || aff.Faction == faction)
                .Select(o => new CommodityQuantity(o.Commodity, o.Amount))
                .ToList();

            return new RecipeResult
            {
                Inputs = inputs,
                Outputs = outputs
            };
        }

        /// <summary>
        /// Liefert alle möglichen konkreten Varianten eines Rezepts für eine Fraktion.
        /// Jede Alternative wird expandiert → mehrere Ergebnisse.
        /// </summary>
        public IEnumerable<RecipeResult> GetAllResultsForFaction(string faction)
        {
            double bonus = AffiliationBonuses.TryGetValue(faction, out var factor) ? factor : 1.0;

            if (Restricted && !AffiliationBonuses.ContainsKey(faction))
                yield break;

            var grouped = Inputs.GroupBy(i => i.GroupId).ToList();
            var mandatory = grouped.Where(g => g.Key is null).SelectMany(g => g).ToList();
            var alternativeGroups = grouped.Where(g => g.Key is not null).ToList();

            IEnumerable<IEnumerable<CommodityQuantity>> ExpandAlternatives(int index)
            {
                if (index == alternativeGroups.Count)
                {
                    yield return Enumerable.Empty<CommodityQuantity>();
                    yield break;
                }

                foreach (var option in alternativeGroups[index])
                {
                    foreach (var rest in ExpandAlternatives(index + 1))
                    {
                        yield return new[] { option }.Concat(rest);
                    }
                }
            }

            foreach (var altCombo in ExpandAlternatives(0))
            {
                var inputs = mandatory
                    .Concat(altCombo)
                    .Select(i => new CommodityQuantity(
                        i.Commodity,
                        (int)Math.Ceiling(i.Amount * bonus),
                        i.GroupId))
                    .ToList();

                var outputs = Outputs
                    .Where(o => o is not AffiliationCommodityQuantity aff || aff.Faction == faction)
                    .Select(o => new CommodityQuantity(o.Commodity, o.Amount))
                    .ToList();

                yield return new RecipeResult
                {
                    Inputs = inputs,
                    Outputs = outputs
                };
            }
        }
    }

    public class RecipeResult
    {
        public List<CommodityQuantity> Inputs { get; set; } = new();
        public List<CommodityQuantity> Outputs { get; set; } = new();
    }
}
