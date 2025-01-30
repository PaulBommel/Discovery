using Discovery.Darkstat.NpcQueryClient;
using Discovery.Darkstat.RouteQueryClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    [TestClass]
    public class NpcQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public NpcQueryClientTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> CompareData
        {
            get
            {
                yield return new object[] { TestAssembly.Darkstat.NpcQueryClient, TestAssembly.DarkstatStaging.NpcQueryClient };
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
        public async Task GetNpcBasesAsyncTest()
        {
            var client = TestAssembly.Darkstat.NpcQueryClient;
            var bases = await client.GetNpcBasesAsync();
            Assert.IsNotNull(bases);
            Assert.AreNotEqual(0, bases.Length);
            foreach (var npcBase in bases)
            {
                if (npcBase.IsReachhable == true)
                    Assert.That.HasPlausibleData(npcBase);
            }
        }

        [DataTestMethod]
        [DynamicData(nameof(CompareData))]
        public async Task CompareClientsAsyncTest(NpcQueryClient.NpcQueryClient expectedClient, NpcQueryClient.NpcQueryClient actualClient)
        {
            var expectedTask = expectedClient.GetNpcBasesAsync();
            var actualTask = actualClient.GetNpcBasesAsync();
            await Task.WhenAll(expectedTask, actualTask);
            Assert.AreEqual(expectedTask.Result.Length, actualTask.Result.Length);
            var nicknames = expectedTask.Result.Select(npcBase => npcBase.Nickname).ToArray();
            for (int i = 0; i < nicknames.Length; ++i)
            {
                var expected = expectedTask.Result.SingleOrDefault(npcBase => npcBase.Nickname == nicknames[i]);
                var actual = actualTask.Result.SingleOrDefault(npcBase => npcBase.Nickname == nicknames[i]);
                Assert.That.AreEqual(expected, actual);
            }
        }

        #endregion

        #endregion
    }
}
