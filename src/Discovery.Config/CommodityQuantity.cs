using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Config
{
    public record CommodityQuantity(string Commodity, int Amount, int? GroupId = null);

    public record AffiliationCommodityQuantity(string Commodity, int Amount, string Faction)
    : CommodityQuantity(Commodity, Amount);
}
