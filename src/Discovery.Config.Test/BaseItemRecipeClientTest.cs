using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
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
            var recipes = await client.GetRecipesAsync(TestContext.CancellationTokenSource.Token);
            foreach (var recipe in recipes)
            {
                Assert.IsNotNull(recipe);
                Assert.IsFalse(string.IsNullOrWhiteSpace(recipe.InfoText), $"{recipe.InfoText} is empty.");
                TestContext.WriteLine($"Item: {recipe.InfoText}");

                if (!recipe.AffiliationBonuses.IsEmpty)
                {
                    var faction = recipe.AffiliationBonuses.Keys.First();
                    var permutations = recipe.GetAllResultsForFaction(faction).ToArray();
                    TestContext.WriteLine($"Found {permutations.Length} permutations (with faction '{faction}').");
                }

                if (recipe.Restricted)
                    Assert.AreNotEqual(0, recipe.AffiliationBonuses.Count);

                TestContext.WriteLine(Environment.NewLine);
            }
        }

        [TestMethod]
        public async Task EnumerateRecipesAsyncTest()
        {
            var client = new BaseItemRecipeClient(new PublicHttpClientFactory());
            await foreach (var recipe in client.EnumerateRecipesAsync(TestContext.CancellationTokenSource.Token))
            {
                Assert.IsNotNull(recipe);
                Assert.IsFalse(string.IsNullOrWhiteSpace(recipe.InfoText), $"{recipe.InfoText} is empty.");
                TestContext.WriteLine($"Item: {recipe.InfoText}");

                if (!recipe.AffiliationBonuses.IsEmpty)
                {
                    var faction = recipe.AffiliationBonuses.Keys.First();
                    var permutations = recipe.GetAllResultsForFaction(faction).ToArray();
                    TestContext.WriteLine($"Found {permutations.Length} permutations (with faction '{faction}').");
                }

                if (recipe.Restricted)
                    Assert.AreNotEqual(0, recipe.AffiliationBonuses.Count);

                TestContext.WriteLine(Environment.NewLine);
            }
        }

        #endregion

        #endregion
    }
}
