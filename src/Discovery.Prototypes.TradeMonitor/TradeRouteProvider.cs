using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Prototypes.TradeMonitor
{
    using Discovery.TradeMonitor;

    using System.IO;
    using System.Text.Json;
    using System.Threading;
    using System.Windows.Controls;

    public class TradeRouteProvider
    {
        public TradeRouteProvider()
        {
        }

        public TradeRouteProvider(ObservableCollection<TradeRoute> tradeRoutes, TradeRoute[] routes)
        {
            TradeRoutes = tradeRoutes;
            Routes = routes;
        }

        public ObservableCollection<TradeRoute> TradeRoutes { get; } = [];
        public TradeRoute[] Routes { get; init; }

        public async Task LoadAsync(TradeRouteExtender extender)
        {
            var routes = GetTradeRoutes().ToArray();

            TradeRoutes.Clear();
            foreach (var route in routes)
                TradeRoutes.Add(await extender.ExtendAsync(route));
            await SaveAsync("Routes.json");
        }
        public Task SaveAsync(string filePath, CancellationToken token = default)
        {
            var json = JsonSerializer.Serialize(TradeRoutes);
            return File.WriteAllTextAsync(filePath, json, token);
        }
        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(TradeRoutes);
            File.WriteAllTextAsync(filePath, json);
        }
        public async Task LoadAsync(string filePath, CancellationToken token = default)
        {
            TradeRoute[] routes = null;
            using (var stream = File.OpenRead(filePath))
                routes = await JsonSerializer.DeserializeAsync<TradeRoute[]>(stream, JsonSerializerOptions.Default, token);
            if(routes is not null)
            {
                TradeRoutes.Clear();
                foreach (var route in routes)
                    TradeRoutes.Add(route);
            }
        }
        public void Load(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var routes = JsonSerializer.Deserialize<TradeRoute[]>(json);
            TradeRoutes.Clear();
            foreach (var route in routes)
                TradeRoutes.Add(route);
        }

        #region static

        private static readonly ShipInfo Train = new ShipInfo("Rheinland Train", 5000, 9);
        private static readonly ShipInfo Lucullus = new ShipInfo("Lucullus", 4000, 10);
        private static readonly ShipInfo HeavyTransport = new ShipInfo("Uruz", 4200, 8);
        public static IEnumerable<TradeRoute> GetTradeRoutes()
        {
            foreach (var route in GetKprRoutes())
                yield return route;
            foreach (var route in GetDhcRoutes())
                yield return route;
            foreach (var route in GetDseRoutes())
                yield return route;
        }
        public static IEnumerable<TradeRoute> GetKprRoutes()
        {
            yield return new TradeRoute()
            {
                Ship = Lucullus,
                Trades =
                [
                    new TradeOnNpcBase() { Station = "Planet Amiens", Sell = [ new("Aluminium", Lucullus.CargoCapacity) ], Buy = [ new("Criminal Cells (Gallia)", 16) ] },
                    new TradeOnNpcBase() { Station = "Mecklenburg Prison", Sell = [ new("Criminal Cells (Gallia)", 16) ], Buy = [ new("Hydrogen", Lucullus.CargoCapacity) ] },
                    new TradeOnPlayerBase() { Station = "Breuninger Depot", Buy = [ new("Aluminium", Lucullus.CargoCapacity) ], Sell = [ new("Hydrogen", Lucullus.CargoCapacity)] }
                ]
            };
        }

        public static IEnumerable<TradeRoute> GetDhcRoutes()
        {
            yield return new TradeRoute()
            {
                Ship = Train,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Ruhrpott Mining Facility", Sell = [ new("Uncut Diamonds", Train.CargoCapacity) ] },
                    new TradeOnMiningZone() { MiningZone = "Uncut Diamonds", Buy = [ new("Uncut Diamonds", Train.CargoCapacity) ] }
                ]
            };
            yield return new TradeRoute()
            {
                Ship = Train,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Ruhrpott Mining Facility", Sell = [ new("Silver Ore", Train.CargoCapacity) ] },
                    new TradeOnMiningZone() { MiningZone = "Silver Ore", Buy = [ new("Silver Ore", Train.CargoCapacity) ] }
                ]
            };
            yield return new TradeRoute()
            {
                Ship = Train,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Arnsberg Research Institute", Buy = [ new("Propulsion Systems", 140) ], Sell = [ new("Robotics", HeavyTransport.CargoCapacity) ] },
                    new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell = [ new("Propulsion Systems", 140) ] },
                    new TradeOnNpcBase() { Station = "Scottsdale Refinery", Buy = [ new("Copper", HeavyTransport.CargoCapacity) ] },
                    new TradeOnNpcBase() { Station = "Essen Station", Buy = [ new("Robotics", HeavyTransport.CargoCapacity) ], Sell = [ new("Copper", HeavyTransport.CargoCapacity) ] }
                ]
            };
        }
        public static IEnumerable<TradeRoute> GetDseRoutes()
        {
            yield return new TradeRoute()
            {
                Ship = HeavyTransport,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell = [ new("Oxygen", HeavyTransport.CargoCapacity) ] },
                    new TradeOnNpcBase() { Station = "Planet Curacao", Buy = [ new("Oxygen", HeavyTransport.CargoCapacity) ] }
                ]
            };
            yield return new TradeRoute()
            {
                Ship = HeavyTransport,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell = [ new("Water", HeavyTransport.CargoCapacity) ] },
                    new TradeOnNpcBase() { Station = "Planet Curacao", Buy = [ new("Water", HeavyTransport.CargoCapacity) ] }
                ]
            };
            yield return new TradeRoute()
            {
                Ship = HeavyTransport,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell = [ new("Synth Paste", HeavyTransport.CargoCapacity) ] },
                    new TradeOnNpcBase() { Station = "Planet Curacao", Buy = [ new("Synth Paste", HeavyTransport.CargoCapacity) ] }
                ]
            };
            yield return new TradeRoute()
            {
                Ship = HeavyTransport,
                Trades =
                [
                    new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell = 
                    [
                        new("Synth Paste", HeavyTransport.CargoCapacity/3),
                        new("Water", HeavyTransport.CargoCapacity/3),
                        new("Oxygen", HeavyTransport.CargoCapacity/3)
                    ]},
                    new TradeOnNpcBase() { Station = "Planet Curacao", Buy =
                    [
                        new("Synth Paste", HeavyTransport.CargoCapacity/3),
                        new("Water", HeavyTransport.CargoCapacity/3),
                        new("Oxygen", HeavyTransport.CargoCapacity/3)
                    ]}
                ]
            };
        }

        #endregion
    }
}
