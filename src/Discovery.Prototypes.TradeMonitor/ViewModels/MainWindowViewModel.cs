using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Discovery.Darkstat;
    using Discovery.Prototypes.TradeMonitor.Views;
    using Discovery.TradeMonitor;

    using System.Net.Http;
    using System.Windows.Input;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private IDarkstatClient _client;
        private TradeRouteProvider _routeProvider;
        private readonly TradeMonitor _tradeMonitor;
        private readonly Timer _refreshTimer;

        public MainWindowViewModel()
            : this(new DarkstatHttpClientFactory())
        {
        }

        public MainWindowViewModel(IHttpClientFactory httpClientFactory)
            : this(new DarkstatClient(httpClientFactory))
        {
        }

        public MainWindowViewModel(IDarkstatClient client)
        {
            _client = client;
            _tradeMonitor = new TradeMonitor(client);
            _routeProvider = new TradeRouteProvider();
            _routeProvider.Load("Routes.json");
            Expanders = TradeExpanderViewModel.FromRoutes(_tradeMonitor, _routeProvider).ToArray();
            _refreshTimer = new(state => Refresh(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            AddNewTradeRouteCommand = new DelegateCommand(AddNewTradeRoute);
        }

        public TradeExpanderViewModel[] Expanders { get; }

        public ICommand AddNewTradeRouteCommand { get; }

        public void Refresh()
        {
            foreach (var expander in Expanders)
                expander.Refresh();
        }

        private async void AddNewTradeRoute(object parameter)
        {
            var shipInfos = await _client.GetShipInfosAsync();
            var dialog = new TradeRouteConfiguratorMainView() { DataContext = new TradeRouteConfiguratorMainViewModel(shipInfos) };
            if (dialog.ShowDialog() == true)
            {

            }
        }
    }
}
