﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Discovery.Prototypes.TradeMonitor
{
    using Discovery.TradeMonitor;
    public sealed class TradeResultViewModel(string Header, SimulationResult Result, bool IsExpanded)
    {
        public string Header { get; } = Header;
        public SimulationResult Result { get; } = Result;
        public bool IsExpanded { get; set; } = IsExpanded;
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

        public TradeExpanderViewModel(TradeMonitor monitor, string origin, TradeRoute[] routes)
        {
            _monitor = monitor;
            Origin = origin;
            Routes = routes;
        }

        #endregion

        #region Properties

        public TradeRoute[] Routes { get; }

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

        public static IEnumerable<TradeExpanderViewModel> FromRoutes(TradeMonitor monitor, TradeRoute[] routes)
        {
            var origins = routes.Select(route => route.Trades[0].Station).Distinct();
            foreach (var origin in origins)
            {
                var routePerOrigin = routes.Where(route => route.Trades[0].Station == origin).ToArray();
                yield return new(monitor, origin.Name, routePerOrigin);
            }
        }

        public async void Refresh()
        {
            var results = await _monitor.GetTradeSimulations(Routes);
            var viewmodels = new TradeResultViewModel[results.Length];
            for(int i = 0; i < results.Length; ++i)
            {
                var header = string.Join(" -> ", Routes[i].Trades.Select(t => t.Station.Name).Union([Routes[i].Trades[0].Station.Name]));
                viewmodels[i] = new(header, results[i], results[i].StockLimit?.Limit != 0);
            }
            TradeResults = viewmodels;
        }

        #endregion
    }
}
