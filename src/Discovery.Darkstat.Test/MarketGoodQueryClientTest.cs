using Discovery.Darkstat.RouteQueryClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class MarketGoodQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public MarketGoodQueryClientTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

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
        [DataRow(["ga07_02_base", "rh01_06_base"])]
        public async Task GetMarketGoodsPerBasesAsyncTest(string[] baseNicknames)
        {
            var client = TestAssembly.Darkstat.MarketGoodQueryClient;
            var data = await client.GetMarketGoodsPerBasesAsync(baseNicknames);
            Assert.IsNotNull(data);
            Assert.AreEqual(baseNicknames.Length, data.Length);
            foreach (var entry in data)
                Assert.IsNull(entry.Error);
        }

        [DataTestMethod]
        [DataRow(["commodity_ship_part_propulsion", "commodity_copper", "commodity_robotic_hardware"])]
        public async Task GetCommoditiesPerNicknameAsyncTest(string[] commodityNicknames)
        {
            var client = TestAssembly.Darkstat.MarketGoodQueryClient;
            var data = await client.GetCommoditiesPerNicknameAsync(commodityNicknames);
            Assert.IsNotNull(data);
            Assert.AreEqual(commodityNicknames.Length, data.Length);
            foreach (var entry in data)
                Assert.IsNull(entry.Error);
        }

        #endregion

        #endregion
    }
}
