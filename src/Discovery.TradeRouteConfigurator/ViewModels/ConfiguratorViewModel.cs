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
            Route = new TradeRoute()
            {
                Ship = new ShipInfo("Lucullus", 4000, 10),
                Trades =
                [
                    new TradeOnNpcBase() { Station = "Planet Amiens", Sell = [ new("Aluminium", 4000) ], Buy = [ new("Criminal Cells (Gallia)", 16) ] },
                    new TradeOnNpcBase() { Station = "Mecklenburg Prison", Sell = [ new("Criminal Cells (Gallia)", 16) ], Buy = [ new("Hydrogen", 4000) ] },
                    new TradeOnPlayerBase() { Station = "Breuninger Depot", Buy = [ new("Aluminium", 4000) ], Sell = [ new("Hydrogen", 4000)] }
                ]
            };
            Ship = new();
            Trades = new();
        }

        public ConfiguratorViewModel(TradeRoute route, Darkstat.ShipInfo[] shipInfoSource, ILocation[] locationSource)
        {
            Route = route;
            Ship = new ShipViewModel(shipInfoSource);
            Trades = new(locationSource);
        }

        #endregion

        #region Properties

        private TradeRoute Route { get; }

        public ShipViewModel Ship { get; }

        public TradesViewModel Trades { get; }

        #endregion

        #region Methods

        #endregion
    }
}
