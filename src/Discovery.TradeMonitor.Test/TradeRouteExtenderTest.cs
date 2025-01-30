using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace Discovery.TradeMonitor.Test
{
    using Darkstat;

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    [TestClass]
    public class TradeRouteExtenderTest
    {
        #region Members

        #endregion

        #region Constructors

        public TradeRouteExtenderTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> Routes
        {
            get
            {
                yield return new object[] 
                {
                    new TradeRoute()
                    {
                        Ship = new("Uruz", 4200, 8),
                        Trades =
                            [
                                new TradeOnPlayerBase() { Station = "Susquehanna Station", Sell =
                                [
                                    new("Synth Paste", 1400),
                                    new("Water", 1400),
                                    new("Oxygen", 1400)
                                ]},
                                new TradeOnNpcBase() { Station = "Planet Curacao", Buy =
                                [
                                    new("Synth Paste", 1400),
                                    new("Water", 1400),
                                    new("Oxygen", 1400)
                                ]}
                            ]
                    }
                };
            }
        }

        #endregion

        #region Methods

        #region MsTest

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
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
        public async Task ExtendTradeRouteTest(TradeRoute route)
        {
            var extender = new TradeRouteExtender(new DarkstatClient(new DarkstatHttpClientFactory()));
            var newRoute = await extender.ExtendAsync(route);
            Assert.IsNotNull(newRoute);
            Assert.IsNotNull(newRoute.Trades);
            foreach(var trade in newRoute.Trades)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(trade.Station.Nickname));
                if (trade.Buy is not null)
                {
                    Assert.AreNotEqual(0, trade.Buy.Length);
                    foreach (var cargo in trade.Buy)
                        Assert.IsFalse(string.IsNullOrWhiteSpace(cargo.Nickname));
                }
                if (trade.Sell is not null)
                {
                    Assert.AreNotEqual(0, trade.Sell.Length);
                    foreach (var cargo in trade.Sell)
                        Assert.IsFalse(string.IsNullOrWhiteSpace(cargo.Nickname));
                }
            }
        }

        #endregion

        #endregion
    }
}
