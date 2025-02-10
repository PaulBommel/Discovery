using Discovery.TradeMonitor;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    internal sealed class TradeRouteTreeViewModel
    {
        #region Members

        #endregion

        #region Constructors

        public TradeRouteTreeViewModel()
        {
            Routes = TradeRouteProvider.GetDhcRoutes().ToArray();
        }

        #endregion

        #region Properties

        public TradeRoute[] Routes { get; }

        #endregion

        #region Methods

        #endregion
    }
}
