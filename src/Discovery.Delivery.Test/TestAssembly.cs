using Discovery.Darkstat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery.Test
{
    [TestClass]
    public static class TestAssembly
    {
        public static class Tesseract
        {
            public const string DataPath = "./tessdata";
            public const string Language = "eng";
        }
        public static string[] Locations { get; private set; }
        public static string[] Commodities { get; private set; }
        public static string[] ShipInfos { get; private set; }

        public const string LocationWordSuffix = "location_phrases";
        public const string CommoditiesWordSuffix = "commodity_phrases";
        public const string ShipInfosWordSuffix = "shipInfos_phrases";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            var darkstat = new DarkstatClient(new DarkstatHttpClientFactory());
            var locationTask = darkstat.GetLocations(testContext.CancellationTokenSource.Token);
            var commoditiesTask = darkstat.GetCommoditiesAsync(testContext.CancellationTokenSource.Token);
            var shipInfosTask = darkstat.GetShipInfosAsync(testContext.CancellationTokenSource.Token);
            Task.WhenAll(locationTask, commoditiesTask, shipInfosTask).Wait(testContext.CancellationTokenSource.Token);
            Locations = [.. locationTask
                           .Result
                           .Select(location => location.Name)
                            .Where(location => !string.IsNullOrWhiteSpace(location))
                           .Distinct()];
            Commodities = [.. commoditiesTask
                             .Result
                             .Select(commodity => commodity.Name)
                             .Where(commodity => !string.IsNullOrWhiteSpace(commodity))
                             .Distinct()];
            ShipInfos = [.. shipInfosTask
                           .Result
                           .Select(commodity => commodity.Name)
                           .Where(commodity => !string.IsNullOrWhiteSpace(commodity))
                           .Distinct()];

            #region Files

            File.WriteAllLinesAsync(GetPath(LocationWordSuffix), Locations, testContext.CancellationTokenSource.Token);
            File.WriteAllLinesAsync(GetPath(CommoditiesWordSuffix), Commodities, testContext.CancellationTokenSource.Token);
            File.WriteAllLinesAsync(GetPath(ShipInfosWordSuffix), ShipInfos, testContext.CancellationTokenSource.Token);

            #endregion
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            File.Delete(GetPath(LocationWordSuffix));
            File.Delete(GetPath(CommoditiesWordSuffix));
            File.Delete(GetPath(ShipInfosWordSuffix));
        }

        private static string GetPath(string suffix)
            => Path.Combine(Tesseract.DataPath, $"{Tesseract.Language}.{suffix}");

    }
}
