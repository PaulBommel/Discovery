using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discovery.Darkstat.Test
{
    //[TestClass]
    public class RouteQueryClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public RouteQueryClientTest()
        {

        }

        #endregion

        #region Properties

        public TestContext TestContext { get; set; }

        public static IEnumerable<object[]> Sample1
        {
            get {
                yield return new object[] {
                    new Route[]{
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Ruhrpott Mining Facility", "Uncut Diamonds").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Arnsberg Research Institute", "Oder Shipyard").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Arnsberg Research Institute", "Susquehanna Station").Result
                    }
                };
            }
        }

        public static IEnumerable<object[]> ErrorSample
        {
            get
            {
                yield return new object[] {
                    new Route[]{
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Ruhrpott Mining Facility", "Ruhrpott Mining Facility").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Arnsberg Research Institute", "Oder Shipyard").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("Arnsberg Research Institute", "error").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("error", "Oder Shipyard").Result,
                        TestAssembly.Darkstat.RouteBuilder.GetRouteByNameAsync("error", "error").Result
                    }, 
                    2,
                    new string[]
                    {
                        null,
                        null,
                        "destination is not found",
                        "source is not found",
                        "both source and destination are not found"
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
        [DynamicData(nameof(Sample1))]
        public async Task GetTimings_AllOk_Test(Route[] routes)
        {
            var client = TestAssembly.Darkstat.RouteQueryClient;
            var timings = await client.GetTimingsAsync(routes);
            Assert.IsNotNull(timings);
            Assert.AreEqual(routes.Length, timings.Length);
            foreach (var entry in timings)
                Assert.IsNull(entry.Error);
        }

        [TestMethod]
        [DynamicData(nameof(Sample1))]
        public async Task GetTimingDictionary_AllOk_Test(Route[] routes)
        {
            var client = TestAssembly.Darkstat.RouteQueryClient;
            var timings = await client.GetTimingDictionaryAsync(routes);
            Assert.IsNotNull(timings);
            Assert.AreEqual(routes.Length, timings.Count);
        }

        [DataTestMethod]
        [DynamicData(nameof(ErrorSample))]
        public async Task GetTimings_Errors_Test(Route[] routes,
                                                 int expectedSuccessfulRoutes,
                                                 string[] errors)
        {
            var client = TestAssembly.Darkstat.RouteQueryClient;
            var timings = await client.GetTimingsAsync(routes);
            Assert.IsNotNull(timings);
            Assert.AreEqual(routes.Length, timings.Length);
            Assert.IsFalse(routes.Length > timings.Length);
            Assert.AreEqual(expectedSuccessfulRoutes, timings.Where(t => t.Time.HasValue).Count());
            Assert.AreEqual(expectedSuccessfulRoutes, timings.Length - errors.Where(e => e != null).Count());
            for (int i = 0; i < routes.Length; ++i)
                Assert.AreEqual(errors[i], timings[i].Error);
        }

        [TestMethod]
        [DynamicData(nameof(ErrorSample))]
        public async Task GetTimingDictionary_Errors_Test(Route[] routes,
                                                          int expectedSuccessfulRoutes,
                                                          string[] errors)
        {
            var client = TestAssembly.Darkstat.RouteQueryClient;
            var timings = await client.GetTimingDictionaryAsync(routes);
            Assert.IsNotNull(timings);
            Assert.AreNotEqual(routes.Length, timings.Count);
            Assert.AreEqual(expectedSuccessfulRoutes, timings.Count);
        }

        #endregion

        #endregion
    }
}
