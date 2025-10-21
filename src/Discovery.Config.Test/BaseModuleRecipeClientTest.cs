using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;
using System.Text.Json;
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

        [TestMethod]
        [DataRow("base_recipe_modules.json")]
        public async Task TestForModuleRecipeChanges(string jsonFile)
        {
            var client = new BaseModuleRecipeClient(new PublicHttpClientFactory());
            var file = new CacheFile<BaseModuleRecipe>(jsonFile);
            if(file.Exists)
            {

                await Assert.That.AssertAsyncSequencesEqual(
                    client.EnumerateRecipesAsync(TestContext.CancellationToken),
                    file.ReadAsync(TestContext.CancellationToken),
                    (assert, e, a) => assert.AreEqual(e, a), 
                    TestContext.CancellationToken
                );
            }
            else
            {
                await file.WriteAsync(client.EnumerateRecipesAsync(TestContext.CancellationToken), TestContext.CancellationToken);
                Assert.Inconclusive($"{jsonFile} did not exist, the file '{Path.GetFullPath(jsonFile)}' was created.");
            }
        }

        #endregion

        #endregion
    }
}
