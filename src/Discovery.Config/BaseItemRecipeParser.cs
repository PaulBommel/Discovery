using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Discovery.Config
{
    internal static class BaseItemRecipeParser
    {
        public static IEnumerable<BaseItemRecipe> ParseFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            return [.. Parse(reader)];
        }

        public static IEnumerable<BaseItemRecipe> ParseString(string content)
        {
            using var reader = new StringReader(content);
            return [.. Parse(reader)];
        }

        public static IEnumerable<BaseItemRecipe> ParseStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return [.. Parse(reader)];
        }

        private static IEnumerable<BaseItemRecipe> Parse(TextReader reader)
        {
            BaseItemRecipeBuilder current = null;

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

                    current = new BaseItemRecipeBuilder();
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
                    case "shortcut_number": if (int.TryParse(value, out var shortcut)) current.ShortcutNumber = shortcut; break;
                    case "craft_type": current.CraftType = value; break;
                    case "cooking_rate": if (int.TryParse(value, out var rate)) current.CookingRate = rate; break;
                    case "required_level": if (int.TryParse(value, out var level)) current.RequiredLevel = level; break;
                    case "restricted": current.Restricted = value.Equals("true", StringComparison.OrdinalIgnoreCase); break;

                    case "consumed": current.AddConsumed(value); break;
                    case "consumed_dynamic": current.AddConsumedDynamic(value); break;
                    case "consumed_dynamic_alt": current.AddConsumedDynamicAlt(value); break;

                    case "produced_item": current.AddProducedItem(value); break;
                    case "produced_affiliation": current.AddProducedAffiliation(value); break;

                    case "affiliation_bonus": current.AddAffiliationBonus(value); break;
                }
            }

            if (current != null)
                yield return current.Build();
        }
    }
}
