using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Discovery.Config
{

    public static class RecipeParser
    {
        /// <summary>
        /// Parst Rezepte aus einem Dateipfad.
        /// </summary>
        public static List<Recipe> ParseFile(string filePath)
        {
            using var reader = new StreamReader(filePath);
            return Parse(reader);
        }

        /// <summary>
        /// Parst Rezepte aus einem String.
        /// </summary>
        public static List<Recipe> ParseString(string content)
        {
            using var reader = new StringReader(content);
            return Parse(reader);
        }

        /// <summary>
        /// Parst Rezepte aus einem Stream.
        /// </summary>
        public static List<Recipe> ParseStream(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return Parse(reader);
        }

        /// <summary>
        /// Kernlogik: verarbeitet einen Text-Reader.
        /// </summary>
        private static List<Recipe> Parse(TextReader reader)
        {
            var recipes = new List<Recipe>();
            Recipe current = null;
            int altGroupCounter = 1;

            string rawLine;
            while ((rawLine = reader.ReadLine()) != null)
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";") || line.StartsWith("#"))
                    continue;

                if (line.StartsWith("[recipe]", StringComparison.OrdinalIgnoreCase))
                {
                    if (current != null)
                        recipes.Add(current);

                    current = new Recipe();
                    altGroupCounter = 1;
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
                    case "nickname":
                        current.Nickname = value;
                        break;

                    case "infotext":
                        current.InfoText = value;
                        break;

                    case "shortcut_number":
                        if (int.TryParse(value, out var shortcut))
                            current.ShortcutNumber = shortcut;
                        break;

                    case "craft_type":
                        current.CraftType = value;
                        break;

                    case "cooking_rate":
                        if (int.TryParse(value, out var rate))
                            current.CookingRate = rate;
                        break;

                    case "required_level":
                        if (int.TryParse(value, out var level))
                            current.RequiredLevel = level;
                        break;

                    case "restricted":
                        current.Restricted = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;

                    case "consumed":
                        {
                            var fields = value.Split(',');
                            if (fields.Length >= 2 &&
                                int.TryParse(fields[1], out var amount))
                            {
                                current.Inputs.Add(new CommodityQuantity(fields[0].Trim(), amount));
                            }
                            break;
                        }

                    case "consumed_dynamic_alt":
                        {
                            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                            if (fields.Length == 0)
                                break;

                            // A: amount, commodity1, commodity2, ...
                            if (int.TryParse(fields[0], out var sharedAmount))
                            {
                                foreach (var commodity in fields.Skip(1))
                                {
                                    current.Inputs.Add(new CommodityQuantity(commodity, sharedAmount, altGroupCounter));
                                }
                                altGroupCounter++;
                            }
                            // B: commodity, amount
                            else if (fields.Length >= 2 && int.TryParse(fields[1], out var amount))
                            {
                                current.Inputs.Add(new CommodityQuantity(fields[0], amount, altGroupCounter));
                                altGroupCounter++;
                            }
                            break;
                        }
                    case "consumed_dynamic":
                        {
                            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                            // expect pair commodity, amount
                            for (int i = 0; i + 1 < fields.Length; i += 2)
                            {
                                string commodity = fields[i];
                                if (int.TryParse(fields[i + 1], out var amount))
                                {
                                    current.Inputs.Add(new CommodityQuantity(commodity, amount, altGroupCounter));
                                }
                            }
                            altGroupCounter++;
                            break;
                        }
                    case "produced_item":
                        {
                            var fields = value.Split(',');
                            if (fields.Length >= 2 &&
                                int.TryParse(fields[1], out var amount))
                            {
                                current.Outputs.Add(new CommodityQuantity(fields[0].Trim(), amount));
                            }
                            break;
                        }

                    case "produced_affiliation":
                        {
                            var fields = value.Split(',');
                            for (int i = 0; i + 2 < fields.Length; i += 3)
                            {
                                string commodity = fields[i].Trim();
                                if (int.TryParse(fields[i + 1], out var amount))
                                {
                                    string faction = fields[i + 2].Trim();
                                    current.Outputs.Add(new AffiliationCommodityQuantity(commodity, amount, faction));
                                }
                            }
                            break;
                        }

                    case "affiliation_bonus":
                        {
                            var fields = value.Split(',');
                            if (fields.Length >= 2 &&
                                double.TryParse(fields[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var bonus))
                            {
                                current.AffiliationBonuses[fields[0].Trim()] = bonus;
                            }
                            break;
                        }
                }
            }

            if (current != null)
                recipes.Add(current);

            return recipes;
        }
    }
}
