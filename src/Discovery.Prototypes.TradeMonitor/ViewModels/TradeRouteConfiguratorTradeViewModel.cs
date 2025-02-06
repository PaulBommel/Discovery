using Discovery.Darkstat;
using Discovery.TradeMonitor;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    public sealed class TradeRouteConfiguratorTradeViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs LocationArgs = new(nameof(Location));

        #endregion

        private ILocation _location;

        #endregion

        #region Constructors

        public TradeRouteConfiguratorTradeViewModel(ILocation[] locations)
        {
            Locations = locations;
            Buy = [];
            AddNewBuyCommand = new DelegateCommand(AddNewBuy, CanAddNewBuy);
            Sell = [];
            AddNewBuyCommand = new DelegateCommand(AddNewSell, CanAddNewSell);
        }

        #endregion

        #region Properties

        public ILocation[] Locations { get; }
        public ILocation Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    firePropertyChanged(LocationArgs);
                }
            }
        }

        public ObservableCollection<CargoInShipViewModel> Buy { get; }

        public ICommand AddNewBuyCommand { get; }

        public ObservableCollection<CargoInShipViewModel> Sell { get; }
        public ICommand AddNewSellCommand { get; }



        #endregion

        #region Methods
        private bool CanAddNewBuy(object parameter)
            => Location is not null;

        private void AddNewBuy(object parameter)
            => Buy.Add(new());

        private bool CanAddNewSell(object parameter)
            => Location is not null && Location is not MiningZone;

        private void AddNewSell(object parameter)
            => Sell.Add(new());

        #endregion
    }
}
