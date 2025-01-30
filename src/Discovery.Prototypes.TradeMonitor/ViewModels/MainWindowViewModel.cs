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
    using Discovery.TradeMonitor;
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        private readonly TradeMonitor _tradeMonitor = new TradeMonitor(new DarkstatHttpClientFactory());
        private readonly Timer _refreshTimer;

        public MainWindowViewModel()
        {
            Expanders = TradeExpanderViewModel.FromRoutes(_tradeMonitor, TradeRouteProvider.GetTradeRoutes().ToArray()).ToArray();
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
