using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    using Darkstat;

    using System.Windows;
    using System.Windows.Input;

    public sealed class TradeRouteConfiguratorMainViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs ShipNameArgs = new(nameof(ShipName));
        private static readonly PropertyChangedEventArgs CargoCapacityArgs = new(nameof(CargoCapacity));
        private static readonly PropertyChangedEventArgs SelectedShipClassArgs = new(nameof(SelectedShipClass));
        private static readonly PropertyChangedEventArgs SelectedShipInfoArgs = new(nameof(SelectedShipInfoArgs));

        #endregion

        private string _shipName = string.Empty;
        private long _cargoCapacity = 0;
        private ShipClass? _selectedShipClass = null;
        private ShipInfo _selectedShipInfo = null;

        #endregion

        #region Constructors

        public TradeRouteConfiguratorMainViewModel()
        {
            OkCommand = new DelegateCommand(CreateRouteObject, CanCreateRouteObject);
            ShipClassSource = Enum.GetValues<ShipClass>();
            Trades = [];
        }

        public TradeRouteConfiguratorMainViewModel(ShipInfo[] shipInfos)
            : this()
        {
            ShipInfoSource = shipInfos;
        }

        #endregion

        #region Properties

        public ICommand OkCommand { get; }

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

        public ObservableCollection<TradeRouteConfiguratorTradeViewModel> Trades { get; }

        #endregion

        #region Methods

        public bool CanCreateRouteObject(object parameter)
            => parameter is Window;

        public void CreateRouteObject(object parameter)
        {
            if(parameter is Window window)
            {
                window.DialogResult = true;
            }
        }

        #endregion
    }
}
