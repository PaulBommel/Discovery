using Discovery.Darkstat;
using Discovery.TradeMonitor;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using Commodity = Models.Commodity;
    public sealed class TradesViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs SelectedTradeArgs = new(nameof(SelectedTrade));

        #endregion

        private TradeViewModel _selectedTrade;

        #endregion

        #region Constructors

        [Obsolete("Xaml Designer only", true)]
        public TradesViewModel()
        {
            Locations = new();
            AddTradeCommand = new DelegateCommand(AddTrade, CanAddTrade);
            RemoveTradeCommand = new DelegateCommand(RemoveTrade, CanRemoveTrade);
            Trades.Add(new(new TradeOnNpcBase() { Station = "Planet Amiens", Sell = [new("Aluminium", 4000)], Buy = [new("Criminal Cells (Gallia)", 16)] }, [new() { Name = "Criminal Cells (Gallia)", NickName = string.Empty }], [new() { Name = "Aluminium", NickName = string.Empty }]));
            Trades.Add(new(new TradeOnNpcBase() { Station = "Mecklenburg Prison", Sell = [new("Criminal Cells (Gallia)", 16)], Buy = [new("Hydrogen", 4000)] }, [new() { Name = "Hydrogen", NickName = string.Empty }], [new() { Name = "Criminal Cells (Gallia)", NickName = string.Empty }]));
            Trades.Add(new(new TradeOnPlayerBase() { Station = "Breuninger Depot", Buy = [new("Aluminium", 4000)], Sell = [new("Hydrogen", 4000)] }, [new() { Name = "Aluminium", NickName = string.Empty }], [new() { Name = "Hydrogen", NickName = string.Empty }]));
            SelectedTrade = Trades[1];
        }
        public TradesViewModel(ILocation[] locationSource)
        {
            Locations = new(locationSource);
            AddTradeCommand = new DelegateCommand(AddTrade, CanAddTrade);
            RemoveTradeCommand = new DelegateCommand(RemoveTrade, CanRemoveTrade);
        }

        #endregion

        #region Properties

        public LocationSelectionViewModel Locations { get; }

        public ObservableCollection<TradeViewModel> Trades { get; } = [];

        public TradeViewModel SelectedTrade
        {
            get => _selectedTrade;
            set
            {
                if (_selectedTrade != value)
                {
                    _selectedTrade = value;
                    firePropertyChanged(SelectedTradeArgs);
                }
            }
        }

        public ICommand AddTradeCommand { get; }
        public ICommand RemoveTradeCommand { get; }

        #endregion

        #region Methods

        private bool CanAddTrade(object parameter)
        {
            if (parameter is ILocation location)
                return location is not null;
            return false;
        }

        private void AddTrade(object parameter)
        {
            if (parameter is ILocation location)
            {
                switch(location)
                {
                    case NpcBase npc:
                        {
                            var trade = new TradeMonitor.TradeOnNpcBase() 
                            { 
                                Station = new() { Name = npc.Name, Nickname = npc.Nickname },
                                Buy = [],
                                Sell = []
                            };
                            Commodity[] buyCommodities = [.. npc.MarketGoods.Where(g => g.BaseSells).Select(good => new Commodity() { Name = good.Name, NickName = good.Nickname })];
                            Commodity[] sellCommodities = [.. npc.MarketGoods.Where(g => g.PriceBaseBuysFor.HasValue).Select(good => new Commodity() { Name = good.Name, NickName = good.Nickname })];
                            Trades.Add(new(trade, buyCommodities, sellCommodities));
                        }
                        break;
                    case PlayerBase pob:
                        {
                            var trade = new TradeMonitor.TradeOnPlayerBase() 
                            { 
                                Station = new() { Name = pob.Name, Nickname = pob.Nickname },
                                Buy = [],
                                Sell = []
                            };
                            Commodity[] buyCommodities = [.. pob.ShopItems.Where(g => g.SellPrice.HasValue).Select(good => new Commodity() { Name = good.Name, NickName = good.Nickname })];
                            Commodity[] sellCommodities = [.. pob.ShopItems.Select(good => new Commodity() { Name = good.Name, NickName = good.Nickname })];

                            Trades.Add(new(trade, buyCommodities, sellCommodities));
                        }
                        break;
                    case MiningZone zone:
                        {
                            var trade = new TradeMonitor.TradeOnMiningZone() 
                            { 
                                MiningZone = new() { Name = zone.Name, Nickname = zone.Nickname },
                                Buy = []
                            };
                            var good = zone.MiningInfo.MinedGood;
                            Commodity[] buyCommodities = [new Commodity() { Name = good.Name, NickName = good.Nickname }];
                            Trades.Add(new(trade, buyCommodities, null));
                        }
                        break;
                }
            }
        }

        private bool CanRemoveTrade(object parameter)
        {
            if (parameter is TradeViewModel trade)
                return trade is not null && Trades.Contains(trade);
            return false;
        }

        private void RemoveTrade(object parameter)
        {
            if (parameter is TradeViewModel trade)
                Trades.Remove(trade);
        }

        #endregion
    }
}
