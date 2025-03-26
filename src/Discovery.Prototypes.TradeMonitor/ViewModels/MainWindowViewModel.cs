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

    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using TradeRouteConfigurator;

    public class MainWindowViewModel : AbstractViewModel
    {

        #region Members

        #region static

        private static readonly PropertyChangedEventArgs DocumentsArgs = new(nameof(Documents));

        #endregion

        private IDarkstatClient _client;
        private TradeRouteProvider _routeProvider;
        private readonly TradeMonitor _tradeMonitor;
        private readonly Timer _refreshTimer;

        private ObservableCollection<IDocumentViewModel> _documents;

        #endregion

        public MainWindowViewModel()
        {
            var isDesignMode = (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);
            if (isDesignMode)
            {
                _client = new DarkstatClient(new DarkstatHttpClientFactory());
                _tradeMonitor = new TradeMonitor(_client);
            }
            Menu = new()
            {
                AddNewTradeRouteCommand = new DelegateCommand(AddNewTradeRoute),
                SaveCommand = new DelegateCommand(Save),
                ShowShipOverviewCommand = new DelegateCommand(ShowShipOverview, CanShowShipOverview)
            };
            Documents = [];
        }

        public MainWindowViewModel(IDarkstatClient client)
            : this()
        {
            _client = client;
            _tradeMonitor = new TradeMonitor(client);
            LoadFileAsync("Routes.json");
            _refreshTimer = new(state => Refresh(), null, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(5));
        }

        public ObservableCollection<IDocumentViewModel> Documents
        {
            get => _documents;
            set
            {
                if (_documents != value)
                {
                    _documents = value;
                    firePropertyChanged(DocumentsArgs);
                }
            }
        }

        public MenuViewModel Menu { get; }

        public void Refresh()
        {
            if(Documents is not null)
                foreach (var tradeCategory in Documents.OfType<TradeCategoryViewModel>())
                    tradeCategory.Refresh();
        }

        private async void LoadFileAsync(string fileName, CancellationToken token = default)
        {
            var provider = new TradeRouteProvider();
            await provider.LoadAsync(fileName, token);
            //await provider.ExtendRoutesAsync(new(_client));
            //await provider.SaveAsync(fileName, token);
            CollectionChangedEventManager.RemoveHandler(_routeProvider?.TradeRoutes, OnTradeRoutesChanged);
            _routeProvider = provider;
            CollectionChangedEventManager.AddHandler(_routeProvider.TradeRoutes, OnTradeRoutesChanged);
            OnTradeRoutesChanged(this, new(NotifyCollectionChangedAction.Reset));
            Refresh();
        }

        private void OnTradeRoutesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach(var document in TradeCategoryViewModel.FromRoutes(_tradeMonitor, _routeProvider))
            {
                document.Refresh();
                var existingDocument = Documents.FirstOrDefault(d => d.Title == document.Title);
                if(existingDocument is null)
                {
                    Documents.Add(document);
                }
                else
                {
                    var index = Documents.IndexOf(existingDocument);
                    Documents[index] = document;
                }
            }
        }

        private async void AddNewTradeRoute(object parameter)
        {
            var route = await new TradeRoute().ShowConfiguratorDialog(_client);
            _routeProvider.TradeRoutes.Add(route);
        }

        private async void Save(object parameter)
            => await _routeProvider.SaveAsync("Routes.json");

        private bool CanShowShipOverview(object parameter)
        {
            return !Documents.OfType<ShipOverviewViewModel>().Any();
        }

        private void ShowShipOverview(object parameter)
        {
            Documents.Add(ShipOverviewViewModel.FromRoutes(_routeProvider));
        }
    }
}
