using System;
using System.ComponentModel;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using Discovery.Darkstat;

    using System.Windows.Input;

    using TradeMonitor;
    using ILocation = Darkstat.ILocation;
    public sealed class ConfiguratorViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs NameArgs = new(nameof(Name));
        private static readonly PropertyChangedEventArgs CategoryArgs = new(nameof(Category));
        private static readonly PropertyChangedEventArgs SaveCommandArgs = new(nameof(SaveCommand));

        #endregion

        private string _name;
        private string _category;
        private ICommand _saveCommand;

        #endregion

        #region Constructors

        [Obsolete("Xaml Designer only", true)]
        public ConfiguratorViewModel()
        {
            Ship = new();
            Trades = new();
        }

        public ConfiguratorViewModel(Darkstat.ShipInfo[] shipInfoSource, ILocation[] locationSource)
        {
            Ship = new ShipViewModel(shipInfoSource);
            Trades = new(locationSource);
        }

        public ConfiguratorViewModel(ShipViewModel ship, TradesViewModel trades)
        {
            Ship = ship;
            Trades = trades;
        }

        #endregion

        #region Properties

        public ShipViewModel Ship { get; }

        public TradesViewModel Trades { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    firePropertyChanged(NameArgs);
                }
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    firePropertyChanged(CategoryArgs);
                }
            }
        }

        public ICommand SaveCommand
        {
            get => _saveCommand;
            set
            {
                if(_saveCommand != value)
                {
                    _saveCommand = value;
                    firePropertyChanged(SaveCommandArgs);
                }
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
