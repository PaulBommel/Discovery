using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Discovery.Config.Playerbase
{
    using Darkstat;

    public class PlayerBase
    {
        private readonly BaseModuleCollection _Modules = new BaseModuleCollection();

        public string Nickname { get; set; }

        public string Name { get; set; }

        public string Pos { get; set; }

        public int Level
        {
            get => _Modules.Slots / 3;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("The Base level must be between 1 and 5");
                _Modules.Slots = value * 3;
            }
        }

        public long? Money { get; set; }

        public double? Health { get; set; }

        public long? DefenseMode { get; set; }

        public string SystemNickname { get; set; }

        public string SystemName { get; set; }

        public string Affiliation { get; set; }

        public Vector3? Position { get; set; }

        public ShopItem[] ShopItems { get; set; }

        public long? CargoSpace { get; set; }

        public BaseModuleCollection Modules => _Modules;
    }
}
