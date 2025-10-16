using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public static class BaseModuleRecipeParser
    {
        public static IEnumerable<BaseModuleRecipe> ParseFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            return [.. Parse(reader)];
        }

        public static IEnumerable<BaseModuleRecipe> ParseString(string content)
        {
            using var reader = new StringReader(content);
            return [.. Parse(reader)];
        }

        public static IEnumerable<BaseModuleRecipe> ParseStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return [.. Parse(reader)];
        }

        private static IEnumerable<BaseModuleRecipe> Parse(TextReader reader)
        {
            BaseModuleRecipeBuilder current = null;

            string rawLine;
            while ((rawLine = reader.ReadLine()) != null)
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[recipe]", StringComparison.OrdinalIgnoreCase))
                {
                    if (current != null)
                        yield return current.Build();

                    current = new BaseModuleRecipeBuilder();
                    continue;
                }

                if (current == null)
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim().ToLowerInvariant();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "nickname": current.Nickname = value; break;
                    case "infotext": current.InfoText = value; break;
                    case "craft_list": current.CraftLists.Add(value); break;
                    case "build_type": current.BuildType = value; break;
                    case "recipe_number": if (int.TryParse(value, out var rn)) current.RecipeNumber = rn; break;
                    case "module_class": if (int.TryParse(value, out var mc)) current.ModuleClass = mc; break;
                    case "cooking_rate": if (int.TryParse(value, out var rate)) current.CookingRate = rate; break;
                    case "reqlevel": if (int.TryParse(value, out var lvl)) current.RequiredLevel = lvl; break;
                    case "consumed": current.AddConsumed(value); break;
                    case "consumed_dynamic_alt": current.AddConsumedDynamicAlt(value); break;
                    case "credit_cost": if (int.TryParse(value, out var cost)) current.CreditCost = cost; break;
                    case "produced_item": current.ProducedItem = value; break;
                    case "cargo_storage": if (int.TryParse(value, out var cargo)) current.CargoStorage = cargo; break;
                }
            }

            if (current != null)
                yield return current.Build();
        }

        public static async IAsyncEnumerable<BaseModuleRecipe> ParseAsync(TextReader reader,
                                                                          [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            BaseModuleRecipeBuilder current = null;

            string rawLine;
            while ((rawLine = await reader.ReadLineAsync(cancellationToken)) != null)
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[recipe]", StringComparison.OrdinalIgnoreCase))
                {
                    if (current != null)
                        yield return current.Build();

                    current = new BaseModuleRecipeBuilder();
                    continue;
                }

                if (current == null)
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim().ToLowerInvariant();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "nickname": current.Nickname = value; break;
                    case "infotext": current.InfoText = value; break;
                    case "craft_list": current.CraftLists.Add(value); break;
                    case "build_type": current.BuildType = value; break;
                    case "recipe_number": if (int.TryParse(value, out var rn)) current.RecipeNumber = rn; break;
                    case "module_class": if (int.TryParse(value, out var mc)) current.ModuleClass = mc; break;
                    case "cooking_rate": if (int.TryParse(value, out var rate)) current.CookingRate = rate; break;
                    case "reqlevel": if (int.TryParse(value, out var lvl)) current.RequiredLevel = lvl; break;
                    case "consumed": current.AddConsumed(value); break;
                    case "consumed_dynamic_alt": current.AddConsumedDynamicAlt(value); break;
                    case "credit_cost": if (int.TryParse(value, out var cost)) current.CreditCost = cost; break;
                    case "produced_item": current.ProducedItem = value; break;
                    case "cargo_storage": if (int.TryParse(value, out var cargo)) current.CargoStorage = cargo; break;
                }
            }

            if (current != null)
                yield return current.Build();
        }
    }
}
