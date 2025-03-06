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

    public sealed class TradeExpanderViewModel : INotifyPropertyChanged
    {
        #region Members

        public static readonly PropertyChangedEventArgs IsExpandedArgs = new(nameof(IsExpanded));
        public static readonly PropertyChangedEventArgs TradeResultsArgs = new(nameof(TradeResults));

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private readonly TradeMonitor _monitor;
        private bool _isExpanded = true;
        private TradeResultViewModel[] _tradeResults = null;

        #endregion

        #region Constructors

        public TradeExpanderViewModel(TradeMonitor monitor, string origin, TradeRouteProvider routeProvider)
        {
            _monitor = monitor;
            Origin = origin;
            RouteProvider = routeProvider;
        }

        #endregion

        #region Properties

        public TradeRouteProvider RouteProvider { get; }

        public string Origin { get; }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    PropertyChanged?.Invoke(this, IsExpandedArgs);
                }
            }
        }

        public TradeResultViewModel[] TradeResults
        {
            get => _tradeResults;
            set
            {
                _tradeResults = value;
                PropertyChanged?.Invoke(this, TradeResultsArgs);
            }
        }

        #endregion

        #region Methods

        public static IEnumerable<TradeExpanderViewModel> FromRoutes(TradeMonitor monitor, TradeRouteProvider routesProvider)
        {
            var origins = routesProvider.TradeRoutes.Select(route => route.Trades[0].Station).Distinct();
            foreach (var origin in origins)
            {
                var routePerOrigin = routesProvider.TradeRoutes.Where(route => route.Trades[0].Station == origin).ToArray();
                yield return new(monitor, origin.Name, new(routesProvider.TradeRoutes, routePerOrigin));
            }
        }

        public async void Refresh()
        {
            var results = await _monitor.GetTradeSimulations(RouteProvider.Routes);
            var viewmodels = new TradeResultViewModel[results.Length];
            for (int i = 0; i < results.Length; ++i)
            {
                var route = RouteProvider.Routes[i];
                var header = route.Name;
                viewmodels[i] = new(header, results[i], results[i].StockLimit?.Limit != 0, CreateTradeRouteConfigCommand(route));
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
