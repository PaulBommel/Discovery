using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Prototypes.TradeMonitor.Model
{
    using Discovery.TradeMonitor;
    public class ShipOverviewModel(ShipInfo ship, int routes)
    {
        public ShipInfo Ship { get; } = ship;
        public int Routes { get; } = routes;
    }
}
