using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public static class BaseItemRecipeExtensions
    {
        public static BaseItemRecipe GetResultForFaction(this BaseItemRecipe recipe, string faction, Func<IEnumerable<CommodityQuantity>, CommodityQuantity>? altSelector = null)
        {
            double bonus = recipe.AffiliationBonuses.TryGetValue(faction, out var factor) ? factor : 1.0;

            if (recipe.Restricted && !recipe.AffiliationBonuses.ContainsKey(faction))
                return null;

            var grouped = recipe.Inputs.GroupBy(i => i.GroupId);

            var inputs = new List<CommodityQuantity>();
            foreach (var group in grouped)
            {
                if (group.Key is null)
                {
                    inputs.AddRange(group.Select(i =>
                        new CommodityQuantity(i.Commodity, (int)Math.Ceiling(i.Amount * bonus))));
                }
                else
                {
                    var chosen = altSelector?.Invoke(group) ?? group.First();
                    inputs.Add(new CommodityQuantity(
                        chosen.Commodity,
                        (int)Math.Ceiling(chosen.Amount * bonus),
                        chosen.GroupId));
                }
            }

            var outputs = recipe.Outputs
                .Where(o => o is not AffiliationCommodityQuantity aff || aff.Faction == faction)
                .Select(o => new CommodityQuantity(o.Commodity, o.Amount))
                .ToImmutableArray();

            return recipe with
            {
                Inputs = inputs.ToImmutableArray(),
                Outputs = outputs,
                AffiliationBonuses = ImmutableDictionary<string, double>.Empty.Add(faction, bonus)
            };
        }

        public static IEnumerable<BaseItemRecipe>  GetAllResultsForFaction(this BaseItemRecipe recipe, string faction)
        {
            double bonus = recipe.AffiliationBonuses.TryGetValue(faction, out var factor) ? factor : 1.0;
            
            if (recipe.Restricted && !recipe.AffiliationBonuses.ContainsKey(faction))
                yield break;

            var grouped = recipe.Inputs.GroupBy(i => i.GroupId).ToList();
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
                    .ToImmutableArray();

                var outputs = recipe.Outputs
                    .Where(o => o is not AffiliationCommodityQuantity aff || aff.Faction == faction)
                    .Select(o => new CommodityQuantity(o.Commodity, o.Amount))
                    .ToImmutableArray();

                yield return recipe with
                {
                    Inputs = inputs,
                    Outputs = outputs,
                    AffiliationBonuses = ImmutableDictionary<string, double>.Empty.Add(faction, bonus)
                };
            }
        }
    }
}
