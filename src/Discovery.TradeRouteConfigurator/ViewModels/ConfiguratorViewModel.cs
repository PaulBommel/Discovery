using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using TradeMonitor;
    using ILocation = Darkstat.ILocation;
    public sealed class ConfiguratorViewModel : AbstractViewModel
    {
        #region Members

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

        #endregion

        #region Methods

        #endregion
    }
}
