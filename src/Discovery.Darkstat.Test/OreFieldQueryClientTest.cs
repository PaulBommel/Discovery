using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class OreFieldQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public OreFieldQueryClientTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> CompareData
        {
            get
            {
                yield return new object[] { TestAssembly.Darkstat.OreFieldQueryClient, TestAssembly.DarkstatStaging.OreFieldQueryClient };
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
            var client = TestAssembly.Darkstat.OreFieldQueryClient;
            var fields = await client.GetAsync();
            foreach (var field in fields)
            {
                if (field.IsReachhable == true)
                    Assert.That.HasPlausibleData(field);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(CompareData))]
        public async Task CompareClientsAsyncTest(OreFieldQueryClient.OreFieldQueryClient expectedClient, OreFieldQueryClient.OreFieldQueryClient actualClient)
        {
            var expectedTask = expectedClient.GetAsync();
            var actualTask = actualClient.GetAsync();
            await Task.WhenAll(expectedTask, actualTask);
            Assert.AreEqual(expectedTask.Result.Length, actualTask.Result.Length);
            var expected = expectedTask.Result.OrderBy(r => r.Nickname).ToArray();
            var actual = actualTask.Result.OrderBy(r => r.Nickname).ToArray();
            for (int i = 0; i < expected.Length; ++i)
                Assert.That.AreEqual(expected[i], actual[i]);
        }

        #endregion

        #endregion
    }
}
