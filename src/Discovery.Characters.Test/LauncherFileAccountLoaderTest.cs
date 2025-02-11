using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Linq;

namespace Discovery.Characters.Test
{
    [TestClass]
    public class LauncherFileAccountLoaderTest
    {
        #region Members

        #endregion

        #region Constructors

        public LauncherFileAccountLoaderTest()
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

        [DataTestMethod]
        [DataRow(@"")]
        public void GetLauncherAccountsTest(string fileName)
        {
            if (!File.Exists(fileName))
                Assert.Inconclusive($"File '{fileName}' not found.");
            var accounts = LauncherAccountLoader.GetLauncherAccounts(fileName).ToArray();
            Assert.IsNotNull(accounts);
            Assert.AreNotEqual(0, accounts.Length);
            foreach(var account in accounts)
            {
                Assert.IsNotNull(account);
                Assert.AreNotEqual(string.Empty, account.Name);
                Assert.AreNotEqual(string.Empty, account.Description);
                Assert.AreNotEqual(string.Empty, account.Category);
                Assert.AreNotEqual(string.Empty, account.Code);
                Assert.AreNotEqual(string.Empty, account.Signature);
            }
        }

        #endregion

        #endregion
    }
}
