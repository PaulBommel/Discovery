using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config
{
    internal abstract class AbstractBaseRecipeBuilder
    {
        private int _altGroupCounter = 1;

        public List<CommodityQuantity> Inputs { get; } = [];

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
    }
}
