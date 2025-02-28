using Discovery.Darkstat;
using Discovery.TradeMonitor;
using Discovery.TradeRouteConfigurator.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.TradeRouteConfigurator.ViewModels
{
    using Commodity = Models.Commodity;
    public sealed class CargoInShipViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs CommodityArgs = new(nameof(Commodity));
        private static readonly PropertyChangedEventArgs QuantityArgs = new(nameof(Quantity));

        #endregion

        private Commodity _commodity;

        #endregion

        #region Constructors

        public CargoInShipViewModel(Commodity commodity, int quantity)
        {
            Commodity = commodity;
            Quantity = quantity;
        }

        public CargoInShipViewModel(CargoInShip cargoInShip)
        {
            CargoInShip = cargoInShip;
        }

        #endregion

        #region Properties

        public CargoInShip CargoInShip { get; private set; }

        public Commodity Commodity
        {
            get => _commodity;
            set
            {
                if (_commodity != value)
                {
                    _commodity = value;
                    CargoInShip = CargoInShip with { Name = value.Name, Nickname = value.NickName };
                    firePropertyChanged(CommodityArgs);
                }
            }
        }

        public long Quantity
        {
            get => CargoInShip.Count;
            set
            {
                if (CargoInShip.Count != value)
                {
                    CargoInShip = CargoInShip with { Count = value };
                    firePropertyChanged(QuantityArgs);
                }
            }
        }

        #endregion
    }
}
