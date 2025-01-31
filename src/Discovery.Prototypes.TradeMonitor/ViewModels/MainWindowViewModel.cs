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
    using Discovery.TradeMonitor;

    using System.Net.Http;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private TradeRouteProvider _routeProvider;
        private readonly TradeMonitor _tradeMonitor;
        private readonly Timer _refreshTimer;

        public MainWindowViewModel()
            : this(new DarkstatHttpClientFactory())
        {
        }

        public MainWindowViewModel(IHttpClientFactory httpClientFactory)
        {
            _tradeMonitor = new TradeMonitor(httpClientFactory);
            _routeProvider = new TradeRouteProvider();
            _routeProvider.Load("Routes.json");
            Expanders = TradeExpanderViewModel.FromRoutes(_tradeMonitor, _routeProvider).ToArray();
            _refreshTimer = new(state => Refresh(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public TradeExpanderViewModel[] Expanders { get; }

        public void Refresh()
        {
            foreach (var expander in Expanders)
                expander.Refresh();
        }
    }
}
