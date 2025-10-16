using Discovery.Darkstat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery.Test
{
    [TestClass]
    public class SampleTesting
    {
        #region Properties

        public TestContext TestContext { get; set; }

        public DeliveryProofEngine Engine { get; set; } = new();

        private AnalyseParameterBuilder ParameterBuilder { get; set; }

        public static IEnumerable<object[]> Routes
        {
            get
            {
                yield return new object[]
                {
                    "samples/sample.png", new DeliveryRecord()
                    {
                        Cargo =
                        [
                            new ItemQuantityRecord() { Amount = 5000, Commodity = "Ablative Armor Plating" }
                        ],
                        Credits = 5_820_208,
                        Destination = "Briancon Station",
                        ShipType = "Rheinland Train",
                        SourceType = "Png",
                        Time = DateTime.ParseExact("2025-04-07 19:41:15", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                    }
                }; 
                yield return new object[]
                {
                    "samples/screen516.BMP", new DeliveryRecord()
                    {
                        Cargo =
                        [
                            new ItemQuantityRecord() { Amount = 10, Commodity = "Crew" },
                            new ItemQuantityRecord() { Amount = 13, Commodity = "Criminal Cells (Rheinland)" },
                            new ItemQuantityRecord() { Amount = 10, Commodity = "Daumann Side Arms" },
                            new ItemQuantityRecord() { Amount = 4, Commodity = "LWB Pilot" },
                            new ItemQuantityRecord() { Amount = 6, Commodity = "Military Salvage" },
                        ],
                        Credits = 2_055_719,
                        Destination = "Mecklenburg Prison",
                        ShipType = "Oasis",
                        SourceType = "Bmp",
                        Time = DateTime.ParseExact("2025-09-23 22:05:08", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
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
            ParameterBuilder = new AnalyseParameterBuilder()
            {
                TesseractDataPath = TestAssembly.Tesseract.DataPath
            }.WithLanguage(TestAssembly.Tesseract.Language)
            .WithNavigationRegion(wordFile: TestAssembly.LocationWordSuffix, words:TestAssembly.Locations)
            .WithCargoRegion(wordFile: TestAssembly.CommoditiesWordSuffix, words:TestAssembly.Commodities)
            .WithShiptypeRegion(wordFile: TestAssembly.ShipInfosWordSuffix, words:TestAssembly.ShipInfos);
        }
        [TestCleanup]
        public void TestCleanup()
        {

        }

        #endregion

        #region Tests

        [TestMethod]
        [DynamicData(nameof(Routes))]
        [SupportedOSPlatform("windows")]
        public void DeliveryOcrSampleTesting(string imageFile, DeliveryRecord expectedRecord)
        {
            using (var bmp = new Bitmap(imageFile))
            {
                ParameterBuilder.WithBitmap(bmp);
                var result = Engine.Analyse(ParameterBuilder.Build());
                Assert.That.AreEqual(expectedRecord, result.Record);
            }
        }

        #endregion

        #endregion
    }
}
