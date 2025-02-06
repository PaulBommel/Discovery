using Discovery.TradeMonitor;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Discovery.Prototypes.TradeMonitor.ViewModels
{
    public sealed class CargoInShipViewModel : AbstractViewModel
    {
        #region Members

        #region static

        private static readonly PropertyChangedEventArgs NameArgs = new(nameof(Name));
        private static readonly PropertyChangedEventArgs InternalNameArgs = new(nameof(InternalName));
        private static readonly PropertyChangedEventArgs QuantityArgs = new(nameof(Quantity));

        #endregion

        #endregion

        #region Constructors

        public CargoInShipViewModel()
        {
            CargoInShip = new("Commodity", 0);
        }

        public CargoInShipViewModel(CargoInShip cargoInShip)
        {
            CargoInShip = cargoInShip;
        }

        #endregion

        #region Properties

        public CargoInShip CargoInShip { get; private set; }

        public string Name 
        { 
            get => CargoInShip.Name; 
            set
            {
                if (CargoInShip.Name != value)
                {
                    CargoInShip = CargoInShip with { Name = value };
                    firePropertyChanged(NameArgs);
                }
            }
        }
        
        public string InternalName 
        { 
            get => CargoInShip.Nickname; 
            set
            {
                if (CargoInShip.Nickname != value)
                {
                    CargoInShip = CargoInShip with { Nickname = value };
                    firePropertyChanged(InternalNameArgs);
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
