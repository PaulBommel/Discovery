using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Discovery.TradeMonitor.Test
{
    using Darkstat;
    [TestClass]
    public class SimulationDataSourceTest
    {
        #region Members

        #endregion

        #region Constructors

        public SimulationDataSourceTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static TradeRoute[] TradeRoutes { get; set; }

        private static SimulationDataSourceProvider DataSourceProvider { get; } = new(new DarkstatClient(new DarkstatHttpClientFactory()));

        public static IEnumerable<object[]> Routes
        {
            get
            {
                foreach (var route in TradeRoutes)
                    yield return new object[] { route };
            }
        }

        #endregion

        #region Methods

        #region MsTest

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            string json = File.ReadAllText("SimulationDataSourceTest.json");
            TradeRoutes = JsonSerializer.Deserialize<TradeRoute[]>(json);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TradeRoutes = null;
        }

        [TestInitialize()]
        public void TestInitialize()
        {

        }
        [TestCleanup]
        public void TestCleanup()
        {

        }

        #endregion

        #region Tests

        [DataTestMethod]
        [DynamicData(nameof(Routes))]
        public async Task GetBuyPriceTest(TradeRoute route)
        {
            var dataSource = await DataSourceProvider.GetDataSourceAsync([route]);
            Assert.IsNotNull(dataSource);
            foreach (var trade in route.Trades)
            {
                if (trade.Buy is not null && trade is not TradeOnMiningZone)
                {
                    foreach (var cargo in trade.Buy)
                        Assert.AreNotEqual(0, dataSource.GetBuyPrice(trade, cargo.Name));
                }
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(Routes))]
        public async Task GetSellPriceTest(TradeRoute route)
        {
            var dataSource = await DataSourceProvider.GetDataSourceAsync([route]);
            Assert.IsNotNull(dataSource);
            foreach (var trade in route.Trades)
            {
                if (trade.Sell is not null)
                {
                    foreach (var cargo in trade.Sell)
                        Assert.AreNotEqual(0, dataSource.GetSellPrice(trade, cargo.Name));
                }
            }
        }

        #endregion

        #endregion
    }
}
