using Discovery.Darkstat;
using Discovery.TradeMonitor;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using Commodity = Models.Commodity;
    public sealed class TradeViewModel
    {
        #region Members

        #endregion

        #region Constructors

        public TradeViewModel(ITradeOnStation tradeOnStation, Commodity[] buyCommodities, Commodity[] sellCommodities)
        {
            switch(tradeOnStation)
            {
                case TradeOnNpcBase trade:
                    Location = new Location() { Name = trade.Station.Name, Nickname = trade.Station.Nickname };
                    Buy = [.. trade.Buy.Select(cargo => new CargoInShipViewModel(cargo))];
                    Sell = [.. trade.Sell.Select(cargo => new CargoInShipViewModel(cargo))];
                    break;
                case TradeOnPlayerBase trade:
                    Location = new Location() { Name = trade.Station.Name, Nickname = trade.Station.Nickname };
                    Buy = [.. trade.Buy.Select(cargo => new CargoInShipViewModel(cargo))];
                    Sell = [.. trade.Sell.Select(cargo => new CargoInShipViewModel(cargo))];
                    break;
                case TradeOnMiningZone trade:
                    Location = new Location() { Name = trade.MiningZone.Name, Nickname = trade.MiningZone.Nickname };
                    Buy = [.. trade.Buy.Select(cargo => new CargoInShipViewModel(cargo))];
                    break;
            }
            BuyCommodities = buyCommodities;
            SellCommodities = sellCommodities;
            AddNewBuyCommand = new DelegateCommand(AddNewBuy, CanAddNewBuy);
            RemoveBuyCommand = new DelegateCommand(RemoveBuy, CanRemoveBuy);
            AddNewSellCommand = new DelegateCommand(AddNewSell, CanAddNewSell);
            RemoveSellCommand = new DelegateCommand(RemoveSell, CanRemoveSell);
        }

        #endregion

        #region Properties

        public Location Location { get; }
        public ObservableCollection<CargoInShipViewModel> Buy { get; }

        public ICommand AddNewBuyCommand { get; }

        public ICommand RemoveBuyCommand { get; }

        public ObservableCollection<CargoInShipViewModel> Sell { get; }

        public ICommand AddNewSellCommand { get; }

        public ICommand RemoveSellCommand { get; }

        public Commodity[] BuyCommodities { get; }
        public Commodity[] SellCommodities { get; }

        #endregion

        #region Methods

        private bool CanAddNewBuy(object parameter)
            => Buy is not null && BuyCommodities.Length > 0;

        private void AddNewBuy(object parameter)
            => Buy.Add(new(BuyCommodities.First(), 0));

        private bool CanRemoveBuy(object parameter)
            => Buy is not null && parameter is CargoInShipViewModel viewModel && Buy.Contains(viewModel);

        private void RemoveBuy(object parameter)
        {
            if (parameter is CargoInShipViewModel viewModel && Buy.Contains(viewModel))
                Buy.Remove(viewModel);
        }

        private bool CanAddNewSell(object parameter)
            => Sell is not null && SellCommodities.Length > 0;

        private void AddNewSell(object parameter)
            => Sell.Add(new(SellCommodities.First(), 0));

        private bool CanRemoveSell(object parameter)
            => Sell is not null && parameter is CargoInShipViewModel viewModel && Sell.Contains(viewModel);

        private void RemoveSell(object parameter)
        {
            if (parameter is CargoInShipViewModel viewModel && Sell.Contains(viewModel))
                Sell.Remove(viewModel);
        }

        #endregion
    }
}
