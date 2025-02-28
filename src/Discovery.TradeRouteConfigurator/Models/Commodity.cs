using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.TradeRouteConfigurator.Models
{
    public readonly record struct Commodity
    {
        public string Name { get; init; }
        public string NickName { get; init; }
    }
}
