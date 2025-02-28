using Discovery.Darkstat;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    public sealed class LocationSelectionViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs SourceArgs = new(nameof(Source));
        private static readonly PropertyChangedEventArgs SelectedArgs = new(nameof(Selected));
        private static readonly PropertyChangedEventArgs IsRegionFilterArgs = new(nameof(IsRegionFilter));
        private static readonly PropertyChangedEventArgs RegionFilterArgs = new(nameof(RegionFilter));

        #endregion

        private readonly ILocation[] _locationSource;
        private ILocation[] _source = null;
        private ILocation _selected = null;
        private bool _isRegionFilter = false;
        private string _regionFilter = string.Empty;

        #endregion

        #region Constructors

        public LocationSelectionViewModel()
        {
        }

        public LocationSelectionViewModel(ILocation[] locationSource)
        {
            _locationSource = locationSource;
            Regions = _locationSource.Select(l => l.RegionName).Distinct().ToArray();
            _source = locationSource;
        }

        #endregion

        #region Properties

        public ILocation[] Source
        {
            get => _source;
            set
            {
                if(_source != value)
                {
                    _source = value;
                    firePropertyChanged(SourceArgs);
                }
            }
        }

        public ILocation Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    firePropertyChanged(SelectedArgs);
                }
            }
        }

        public bool IsRegionFilter
        {
            get => _isRegionFilter;
            set
            {
                if (_isRegionFilter != value)
                {
                    _isRegionFilter = value;
                    firePropertyChanged(IsRegionFilterArgs);
                    Filter();
                }
            }
        }

        public string[] Regions { get; }

        public string RegionFilter
        {
            get => _regionFilter;
            set
            {
                if(_regionFilter != value)
                {
                    _regionFilter = value;
                    firePropertyChanged(RegionFilterArgs);
                    Filter();
                }
            }
        }


        #endregion

        #region Methods

        private void Filter()
        {
            if (IsRegionFilter)
            {
                if (string.IsNullOrEmpty(_regionFilter))
                    Source = _locationSource;
                else
                    Source = [.. _locationSource.Where(l => l.RegionName == RegionFilter)];
            }
            else
                Source = _locationSource;
        }

        #endregion
    }
}
