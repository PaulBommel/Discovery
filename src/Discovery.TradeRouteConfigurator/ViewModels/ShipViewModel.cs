using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using Darkstat;
    public sealed class ShipViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs ShipNameArgs = new(nameof(ShipName));
        private static readonly PropertyChangedEventArgs CargoCapacityArgs = new(nameof(CargoCapacity));
        private static readonly PropertyChangedEventArgs SelectedShipClassArgs = new(nameof(SelectedShipClass));
        private static readonly PropertyChangedEventArgs SelectedShipInfoArgs = new(nameof(SelectedShipInfo));

        #endregion

        private string _shipName = string.Empty;
        private long _cargoCapacity = 0;
        private ShipClass? _selectedShipClass = null;
        private ShipInfo _selectedShipInfo = null;

        #endregion

        #region Constructors

        [Obsolete("Xaml Designer only", true)]
        public ShipViewModel()
        {
            ShipClassSource = Enum.GetValues<ShipClass>();
        }

        public ShipViewModel(ShipInfo[] shipInfoSource)
        {
            ShipClassSource = Enum.GetValues<ShipClass>();
            ShipInfoSource = shipInfoSource;
        }

        #endregion

        #region Properties
        public string ShipName
        {
            get => _shipName;
            set
            {
                if (_shipName != value)
                {
                    _shipName = value;
                    firePropertyChanged(ShipNameArgs);
                }
            }
        }

        public long CargoCapacity
        {
            get => _cargoCapacity;
            set
            {
                if (_cargoCapacity != value)
                {
                    _cargoCapacity = value;
                    firePropertyChanged(CargoCapacityArgs);
                }
            }
        }

        public ShipClass[] ShipClassSource { get; }

        public ShipClass? SelectedShipClass
        {
            get => _selectedShipClass;
            set
            {
                if (_selectedShipClass != value)
                {
                    _selectedShipClass = value;
                    firePropertyChanged(SelectedShipClassArgs);
                }
            }
        }

        public ShipInfo[] ShipInfoSource { get; }

        public ShipInfo SelectedShipInfo
        {
            get => _selectedShipInfo;
            set
            {
                if (_selectedShipInfo != value)
                {
                    _selectedShipInfo = value;
                    firePropertyChanged(SelectedShipInfoArgs);
                    if (value is not null)
                    {
                        CargoCapacity = value.HoldSize;
                        SelectedShipClass = value.ShipClass;
                        if (string.IsNullOrWhiteSpace(ShipName))
                            ShipName = value.Name;
                    }
                }
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
