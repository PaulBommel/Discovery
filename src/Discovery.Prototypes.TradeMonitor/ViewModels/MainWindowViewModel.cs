using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Discovery.Darkstat;
    using Discovery.Prototypes.TradeMonitor.Views;
    using Discovery.TradeMonitor;
    using TradeRouteConfigurator;

    public class MainWindowViewModel : AbstractViewModel
    {

        #region Members

        #region static

        private static readonly PropertyChangedEventArgs ExpandersArgs = new(nameof(Expanders));

        #endregion

        private IDarkstatClient _client;
        private TradeRouteProvider _routeProvider;
        private readonly TradeMonitor _tradeMonitor;
        private readonly Timer _refreshTimer;

        private TradeExpanderViewModel[] _expanders;

        #endregion

        public MainWindowViewModel()
        {
            var isDesignMode = (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);
            if (isDesignMode)
            {
                _client = new DarkstatClient(new DarkstatHttpClientFactory());
                _tradeMonitor = new TradeMonitor(_client);
            }
            AddNewTradeRouteCommand = new DelegateCommand(AddNewTradeRoute);
        }

        public MainWindowViewModel(IDarkstatClient client)
            : this()
        {
            _client = client;
            _tradeMonitor = new TradeMonitor(client);
            LoadFileAsync("Routes.json");
            _refreshTimer = new(state => Refresh(), null, TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(5));
        }

        public TradeExpanderViewModel[] Expanders
        {
            get => _expanders;
            set
            {
                if (_expanders != value)
                {
                    _expanders = value;
                    firePropertyChanged(ExpandersArgs);
                }
            }
        }

        public ICommand AddNewTradeRouteCommand { get; }

        public void Refresh()
        {
            if(Expanders is not null)
                foreach (var expander in Expanders)
                    expander.Refresh();
        }

        private async void LoadFileAsync(string fileName, CancellationToken token = default)
        {
            var provider = new TradeRouteProvider();
            await provider.LoadAsync(fileName, token);
            await provider.ExtendRoutesAsync(new(_client));
            await provider.SaveAsync(fileName, token);
            _routeProvider = provider;
            Expanders = TradeExpanderViewModel.FromRoutes(_tradeMonitor, _routeProvider).ToArray();
            Refresh();
        }

        private async void AddNewTradeRoute(object parameter)
        {
            var route = await _routeProvider.TradeRoutes[0].ShowConfiguratorDialog(_client);
        }
    }
}
