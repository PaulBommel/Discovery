using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Model;
    internal sealed class ShipOverviewViewModel
    {
        #region Members

        #endregion

        #region Constructors

        public ShipOverviewViewModel()
        {
            Ships = [];
        }

        #endregion

        #region Properties

        public string Title { get; } = "Ship overview";

        public ObservableCollection<ShipOverviewModel> Ships { get; }

        #endregion

        #region Methods

        #endregion
    }
}
