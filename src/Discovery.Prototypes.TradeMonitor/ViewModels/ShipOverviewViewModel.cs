using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Model;

    using System.Linq;

    internal sealed class ShipOverviewViewModel : AbstractViewModel, IDocumentViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs IsActiveArgs = new(nameof(IsActive));

        #endregion

        private bool _isActive = true;

        #endregion

        #region Constructors

        public ShipOverviewViewModel()
        {
            Ships = [];
        }

        #endregion

        #region Properties

        public string Title { get; } = "Ship overview";

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    firePropertyChanged(IsActiveArgs);
                }
            }
        }

        public ObservableCollection<ShipOverviewModel> Ships { get; }

        #endregion

        #region Methods

        public static ShipOverviewViewModel FromRoutes(TradeRouteProvider routeProvider)
        {
            var groups = routeProvider.TradeRoutes.GroupBy(route => route.Ship);
            var viewmodel = new ShipOverviewViewModel();
            foreach (var group in groups)
                viewmodel.Ships.Add(new(group.Key, group.Count()));
            return viewmodel;
        }

        #endregion
    }
}
