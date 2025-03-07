using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Discovery.TradeMonitor;
    using TradeRouteConfigurator;

    public sealed class TradeResultViewModel(string Header, SimulationResult Result, bool IsExpanded, ICommand ConfigureCommand)
    {
        public string Header { get; } = Header;
        public SimulationResult Result { get; } = Result;
        public bool IsExpanded { get; set; } = IsExpanded;
        public ICommand ConfigureCommand { get; } = ConfigureCommand;
    }

    public sealed class TradeCategoryViewModel : AbstractViewModel, IDocumentViewModel, INotifyPropertyChanged
    {
        #region Members

        public static readonly PropertyChangedEventArgs IsActiveArgs = new(nameof(IsActive));
        public static readonly PropertyChangedEventArgs TradeResultsArgs = new(nameof(TradeResults));

        private readonly TradeMonitor _monitor;
        private bool _isActive = true;
        private TradeResultViewModel[] _tradeResults = null;

        #endregion

        #region Constructors

        public TradeCategoryViewModel(string title, TradeMonitor monitor, TradeRouteProvider routeProvider)
        {
            Title = title;
            _monitor = monitor;
            RouteProvider = routeProvider;
        }

        #endregion

        #region Properties

        public TradeRouteProvider RouteProvider { get; }

        public string Title { get; }

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

        public TradeResultViewModel[] TradeResults
        {
            get => _tradeResults;
            set
            {
                _tradeResults = value;
                firePropertyChanged(TradeResultsArgs);
            }
        }

        #endregion

        #region Methods

        public static IEnumerable<TradeCategoryViewModel> FromRoutes(TradeMonitor monitor, TradeRouteProvider routesProvider)
        {
            var groups = routesProvider.TradeRoutes.GroupBy(route => route.Category);
            foreach (var group in groups)
            {
                yield return new(group.Key, monitor, new(routesProvider.TradeRoutes, [.. group]));
            }
        }

        public async void Refresh()
        {
            var results = await _monitor.GetTradeSimulations(RouteProvider.Routes);
            var viewmodels = new TradeResultViewModel[results.Length];
            for (int i = 0; i < results.Length; ++i)
            {
                var result = results[i];
                var route = RouteProvider.Routes.Single(r => r.Name == result.RouteName);
                var header = route.Name;
                viewmodels[i] = new(header, result, true, CreateTradeRouteConfigCommand(route));
            }
            TradeResults = viewmodels;
        }

        private ICommand CreateTradeRouteConfigCommand(TradeRoute route)
            => new DelegateCommand(async p =>
            {
                var tradeRouteIndex = RouteProvider.TradeRoutes.IndexOf(route);
                var routeIndex = Array.IndexOf(RouteProvider.Routes, route);
                var newRoute = await route.ShowConfiguratorDialog(_monitor.Client);
                if(route != newRoute)
                {
                    RouteProvider.Routes[routeIndex] = newRoute;
                    RouteProvider.TradeRoutes[tradeRouteIndex] = newRoute;
                }
            });

        #endregion
    }
}
