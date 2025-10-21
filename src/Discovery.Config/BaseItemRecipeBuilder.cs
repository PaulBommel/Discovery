using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Discovery.Config
{
    internal class BaseItemRecipeBuilder : AbstractBaseRecipeBuilder
    {
        private int _altGroupCounter = 1;

        public string Nickname { get; set; }
        public string InfoText { get; set; }
        public int ShortcutNumber { get; set; }
        public string CraftType { get; set; }
        public int CookingRate { get; set; }
        public int RequiredLevel { get; set; }
        public bool Restricted { get; set; }
        public List<CommodityQuantity> Outputs { get; } = [];
        public Dictionary<string, double> AffiliationBonuses { get; } = [];



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
            //default commodity, default amount, specific faction, specific commodity, specific amount
            if(fields.Length == 5)
            {
                string commodity = fields[0];
                if (int.TryParse(fields[1], out var amount))
                    Outputs.Add(new CommodityQuantity(commodity, amount));
                string faction = fields[2];
                commodity = fields[3];
                if (int.TryParse(fields[4], out amount)) 
                    Outputs.Add(new AffiliationCommodityQuantity(commodity, amount, faction));
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
    }
}
