using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class PobQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public PobQueryClientTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> CompareData
        {
            get
            {
                yield return new object[] { TestAssembly.Darkstat.PobQueryClient, TestAssembly.DarkstatStaging.PobQueryClient };
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

        [TestMethod]
        public async Task GetAsyncTest()
        {
            var client = TestAssembly.Darkstat.PobQueryClient;
            var pobs = await client.GetAsync();
            Assert.IsNotNull(pobs);
            Assert.AreNotEqual(0, pobs.Length);
        }

        [DataTestMethod]
        [DynamicData(nameof(CompareData))]
        public async Task CompareClientsAsyncTest(PobQueryClient.PobQueryClient expectedClient, PobQueryClient.PobQueryClient actualClient)
        {
            var expectedTask = expectedClient.GetAsync();
            var actualTask = actualClient.GetAsync();
            await Task.WhenAll(expectedTask, actualTask);
            Assert.AreEqual(expectedTask.Result.Length, actualTask.Result.Length);
            for (int i = 0; i < expectedTask.Result.Length; ++i)
                Assert.That.AreEqual(expectedTask.Result[i], actualTask.Result[i]);
        }

        #endregion

        #endregion
    }
}
