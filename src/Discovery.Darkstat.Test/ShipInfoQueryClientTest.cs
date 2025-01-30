using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class ShipInfoQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public ShipInfoQueryClientTest()
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
        public async Task GetShipInfosAsyncTest()
        {
            var client = TestAssembly.Darkstat.ShipInfoQueryClient;
            var shipInfos = await client.GetShipInfosAsync();
            foreach (var shipInfo in shipInfos)
            {
                Assert.IsNotNull(shipInfo);
            }
        }

        #endregion

        #endregion
    }
}
