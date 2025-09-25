using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{
    public record AnalyseResult
    {
        public Rectangle[] Regions { get; init; }
        public DeliveryRecord Record { get; init; }
    }
}
