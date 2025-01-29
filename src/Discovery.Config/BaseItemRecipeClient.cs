using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public sealed class BaseItemRecipeClient(IHttpClientFactory clientFactory)
    {
        public async Task<BaseItemRecipe[]> GetRecipesAsync(CancellationToken token = default)
        {
            var client = clientFactory.CreateClient();
            var response = await client.GetStringAsync("base_recipe_items.cfg", token);
            var split = response.Split(Environment.NewLine + Environment.NewLine);
            var recipes = split
                         .Where(s => s.Contains("[recipe]"))
                         .Select(GetRecipe)
                         .ToArray();
            return recipes;
        }

        private BaseItemRecipe GetRecipe(string input)
        {
            //var regex = Regex.Matches(input, @"(?<key>[\w]+)? = (?<simplevalue>[\w\s\d]+$)?(?:(?<item_name>[\w]+),\s+(?<item_count>\d+$))?(?:(?<affiliation_nickname>[\w]+),\s+(?<affiliation_bonus>[\d\.]+))?", RegexOptions.Multiline);
            var matches = Regex.Matches(input, @"(?<key>[\w]+)? = (?<value>[^\r\n]*)", RegexOptions.Multiline);
            var recipe = new BaseItemRecipe();
            foreach(Match match in matches)
            {
                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;
                switch (key)
                {
                    case "nickname":
                        recipe = recipe with { Nickname = value };
                        break;
                    case "produced_item":
                        {
                            var m = Regex.Match(value, @"(?<item_name>[\w]+),\s+(?<item_count>\d+$)");
                            var items = recipe.ProducedItems?.ToList() ?? [];
                            items.Add(new() { Item = m.Groups["item_name"].Value, Quantity = int.Parse(m.Groups["item_count"].Value) });
                            recipe = recipe with { ProducedItems = [.. items] };
                        }
                        break;
                    case "produced_affiliation":
                        {
                            var s = value.Split(", ");
                            var producedItemsByAffiliation = new ItemQuantityAffiliation()
                            {
                                ProducedItems = new() { Item = s[0], Quantity = int.Parse(s[1]) },
                                 FactionNickname = s[2],
                                  ProducedItemsWithFaction = new() { Item = s[3], Quantity = int.Parse(s[4]) }
                            };
                            var items = recipe.ProducedByAffiliation?.ToList() ?? [];
                            items.Add(producedItemsByAffiliation);
                            recipe = recipe with { ProducedByAffiliation = [.. items] };
                        }
                        break;
                    case "infotext":
                        recipe = recipe with { Infotext = value };
                        break;
                    case "shortcut_number":
                        recipe = recipe with { Shortcut = int.Parse(value) };
                        break;
                    case "craft_type":
                        recipe = recipe with { CraftType = value };
                        break;
                    case "cooking_rate":
                        recipe = recipe with { CookingRate = int.Parse(value) };
                        break;
                    case "reqlevel":
                        recipe = recipe with { RequiredLevel = int.Parse(value) };
                        break;
                    case "consumed":
                        {
                            var m = Regex.Match(value, @"(?<item_name>[\w]+),\s+(?<item_count>\d+$)");
                            var consumedGroups = recipe.ConsumeGroups?.ToList() ?? [];
                            consumedGroups.Add([new() { Item = m.Groups["item_name"].Value, Quantity = int.Parse(m.Groups["item_count"].Value) }]);
                            recipe = recipe with { ConsumeGroups = [.. consumedGroups] };
                        }
                        break;
                    case "affiliation_bonus":
                        {
                            var m = Regex.Match(value, @"(?<affiliation_nickname>[\w]+),\s+(?<affiliation_bonus>[\d\.]+)");
                            var boni = recipe.AffiliationBoni?.ToList() ?? [];
                            boni.Add(new() { FactionNickname = m.Groups["affiliation_nickname"].Value, Factor = double.Parse(m.Groups["affiliation_bonus"].Value, CultureInfo.InvariantCulture) });
                            recipe = recipe with { AffiliationBoni = [.. boni] };
                        }
                        break;
                    case "consumed_dynamic_alt":
                        {
                            var s = value.Split(", ");
                            var quantity = int.Parse(s[0]);
                            var consumedGroups = recipe.ConsumeGroups?.ToList() ?? [];
                            var group = new ItemQuantity[s.Length - 1];
                            for(int i = 1; i < s.Length; ++i)
                            {
                                group[i - 1] = new() { Item = s[i], Quantity = quantity };
                            }
                            consumedGroups.Add(group);
                            recipe = recipe with { ConsumeGroups = [.. consumedGroups] };
                        }
                        break;
                    case "consumed_dynamic":
                        {
                            var submatches = Regex.Matches(value, @"(?<item_name>\w+),\s(?<item_count>\d+)");
                            var consumeGroup = new ItemQuantity[submatches.Count];
                            for (int i = 0; i < submatches.Count; ++i)
                                consumeGroup[i] = new()
                                {
                                    Item = submatches[i].Groups["item_name"].Value,
                                    Quantity = int.Parse(submatches[i].Groups["item_count"].Value)
                                };
                            var consumeList = recipe.ConsumeGroups?.ToList() ?? [];
                            consumeList.Add(consumeGroup);
                            recipe = recipe with { ConsumeGroups = [.. consumeList] };
                        }
                        break;
                    case "catalyst":
                        {
                            var m = Regex.Match(value, @"(?<catalyst>[\w]+),\s+(?<catalyst_count>\d+)");
                            recipe = recipe with 
                            { 
                                Catalyst = new() { Item = m.Groups["catalyst"].Value, Quantity = int.Parse(m.Groups["catalyst_count"].Value) } 
                            };
                        }
                        break;
                    case "loop_production":
                        recipe = recipe with { LoopProduction = int.Parse(value) != 0 };
                        break;
                    case "restricted":
                        recipe = recipe with { IsRestricted = value == "true" };
                        break;
                    default:
                        Console.WriteLine(match.Groups["key"].Value);
                        break;
                }
            }
            return recipe;
        }
    }
}
