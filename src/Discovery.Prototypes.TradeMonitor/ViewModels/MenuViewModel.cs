using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    public sealed class MenuViewModel : AbstractViewModel
    {
        #region Members

        #endregion

        #region Constructors

        public MenuViewModel()
        {

        }

        #endregion

        #region Properties

        public ICommand AddNewTradeRouteCommand { get; init; }
        public ICommand SaveCommand { get; init; }

        #endregion

        #region Methods

        #endregion
    }
}
