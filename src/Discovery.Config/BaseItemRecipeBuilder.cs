using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Discovery.Config
{
    internal class BaseItemRecipeBuilder
    {
        private int _altGroupCounter = 1;

        public string Nickname { get; set; }
        public string InfoText { get; set; }
        public int ShortcutNumber { get; set; }
        public string CraftType { get; set; }
        public int CookingRate { get; set; }
        public int RequiredLevel { get; set; }
        public bool Restricted { get; set; }

        public List<CommodityQuantity> Inputs { get; } = new();
        public List<CommodityQuantity> Outputs { get; } = new();
        public Dictionary<string, double> AffiliationBonuses { get; } = new();

        public void AddConsumed(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (fields.Length >= 2 && int.TryParse(fields[1], out var amount))
            {
                Inputs.Add(new CommodityQuantity(fields[0], amount));
            }
        }

        public void AddConsumedDynamic(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int i = 0; i + 1 < fields.Length; i += 2)
            {
                string commodity = fields[i];
                if (int.TryParse(fields[i + 1], out var amount))
                {
                    Inputs.Add(new CommodityQuantity(commodity, amount, _altGroupCounter));
                }
            }
            _altGroupCounter++;
        }

        public void AddConsumedDynamicAlt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (fields.Length == 0) return;

            // "amount, commodity1, commodity2, ..."
            if (int.TryParse(fields[0], out var sharedAmount))
            {
                foreach (var commodity in fields.Skip(1))
                    Inputs.Add(new CommodityQuantity(commodity, sharedAmount, _altGroupCounter));
                _altGroupCounter++;
            }
            // "commodity, amount"
            else if (fields.Length >= 2 && int.TryParse(fields[1], out var amount))
            {
                Inputs.Add(new CommodityQuantity(fields[0], amount, _altGroupCounter));
                _altGroupCounter++;
            }
        }

        public void AddProducedItem(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (fields.Length >= 2 && int.TryParse(fields[1], out var amount))
            {
                Outputs.Add(new CommodityQuantity(fields[0], amount));
            }
        }

        public void AddProducedAffiliation(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var fields = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            for (int i = 0; i + 2 < fields.Length; i += 3)
            {
                string commodity = fields[i];
                if (int.TryParse(fields[i + 1], out var amount))
                {
                    string faction = fields[i + 2];
                    Outputs.Add(new AffiliationCommodityQuantity(commodity, amount, faction));
                }
            }
        }

        public void AddAffiliationBonus(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            var cleaned = value.Split(';', 2)[0].Trim();
            var fields = cleaned.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (fields.Length >= 2 && double.TryParse(fields[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var bonus))
            {
                AffiliationBonuses[fields[0]] = bonus;
            }
        }

        public BaseItemRecipe Build() => new BaseItemRecipe
        {
            Nickname = Nickname,
            InfoText = InfoText,
            ShortcutNumber = ShortcutNumber,
            CraftType = CraftType,
            CookingRate = CookingRate,
            RequiredLevel = RequiredLevel,
            Restricted = Restricted,
            Inputs = [.. Inputs],
            Outputs = [.. Outputs],
            AffiliationBonuses = AffiliationBonuses.ToImmutableDictionary()
        };
    }
}
