using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading.Tasks;

namespace Discovery.Config.Test
{
    [TestClass]
    public class BaseItemRecipeClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public BaseItemRecipeClientTest()
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
        public async Task GetRecipesAsyncTest()
        {
            var client = new BaseItemRecipeClient(new PublicHttpClientFactory());
            var recipes = await client.GetRecipesAsync();
            foreach (var recipe in recipes)
                Assert.IsNotNull(recipe);
        }

        #endregion

        #endregion
    }
}
