using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Discovery.Config.Test
{
    [TestClass]
    public class BaseModuleRecipeClientTest
    {
        #region Members

        #endregion

        #region Constructors

        public BaseModuleRecipeClientTest()
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
            var client = new BaseModuleRecipeClient(new PublicHttpClientFactory());
            var recipes = await client.GetRecipesAsync(TestContext.CancellationTokenSource.Token);
            foreach (var recipe in recipes)
            {
                Assert.IsNotNull(recipe);
                Assert.IsFalse(string.IsNullOrWhiteSpace(recipe.InfoText), $"{recipe.InfoText} is empty.");
                TestContext.WriteLine($"Module: {recipe.InfoText}");


                TestContext.WriteLine(Environment.NewLine);
            }
        }

        [TestMethod]
        public async Task EnumerateRecipesAsyncTest()
        {
            var client = new BaseModuleRecipeClient(new PublicHttpClientFactory());
            await foreach (var recipe in client.EnumerateRecipesAsync(TestContext.CancellationTokenSource.Token))
            {
                Assert.IsNotNull(recipe);
                Assert.IsFalse(string.IsNullOrWhiteSpace(recipe.InfoText), $"{recipe.InfoText} is empty.");
                TestContext.WriteLine($"Module: {recipe.InfoText}");


                TestContext.WriteLine(Environment.NewLine);
            }
        }

        #endregion

        #endregion
    }
}
