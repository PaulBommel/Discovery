using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class CommodityQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public CommodityQueryClientTest()
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

        [TestMethod]
        public async Task GetCommoditiesAsyncTest()
        {
            var client = TestAssembly.Darkstat.CommodityQueryClient;
            var commodities = await client.GetCommoditiesAsync();
            foreach (var commodity in commodities)
            {
                Assert.IsNotNull(commodity);
            }
        }

        #endregion

        #endregion
    }
}
