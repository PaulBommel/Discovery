using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace Discovery.Delivery.DataProviders
{
    internal class DestinationDataProvider : IRegionDataProvider<string>
    {
        public DestinationDataProvider(AnalyseRegion region)
        {
            Region = region;
        }

        public AnalyseRegion Region { get; }
        public string Charset { get; } = Charsets.UpercaseLetters + Charsets.LowercaseLetters + Charsets.Numbers + "-> ";

        public string GetData(TesseractEngine engine, Bitmap source)
        {
            var text = ExtractYellowHighlightedText(engine, source, Region.Bounds);

            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var match = Regex.Match(text, @"(\d+(\.\d+)?[Kk]?)\s+(.+)");
            if (match.Success)
                return match.Groups[3].Value.Trim();
            return text;
        }

        public static string ExtractYellowHighlightedText(TesseractEngine engine, Bitmap source, Rectangle region)
        {
            using (var cropped = source.Clone(region, source.PixelFormat))
            {
                // 1. Finde alle gelben Pixel-Y-Koordinaten
                var yellowYPositions = new List<int>();

                for (int y = 0; y < cropped.Height; y++)
                {
                    for (int x = 0; x < cropped.Width; x++)
                    {
                        Color pixel = cropped.GetPixel(x, y);
                        if (pixel.IsSelectionPixel())
                        {
                            yellowYPositions.Add(y);
                        }
                    }
                }

                if (yellowYPositions.Count == 0)
                    return null; // Keine gelben Pixel gefunden

                // 2. Clusterung der Y-Werte nach Nähe (innerhalb von max. 2 Pixeln Abstand)
                var clusteredLines = ClusterYCoordinates([.. yellowYPositions.Distinct()], tolerance: 2);

                // 3. Wähle den größten Cluster (meiste gelbe Pixel)
                var selectedCluster = clusteredLines.OrderByDescending(c => c.Count).FirstOrDefault();

                if (selectedCluster == null || selectedCluster.Count == 0)
                    return null;

                int minY = selectedCluster.Min();
                int maxY = selectedCluster.Max();

                // 4. Extrahiere Zeilen-Bitmap
                var lineRect = new Rectangle(0, minY-5, cropped.Width, maxY - minY + 10);
                using (var lineImage = cropped.Clone(lineRect, cropped.PixelFormat))
                using (var pix = lineImage.ToPix())
                using (var page = engine.Process(pix))
                {
                    return page.GetText()?.Trim();
                }
            }
        }

        private static List<List<int>> ClusterYCoordinates(List<int> yCoords, int tolerance = 2)
        {
            var clusters = new List<List<int>>();
            yCoords.Sort();

            var currentCluster = new List<int> { yCoords[0] };

            for (int i = 1; i < yCoords.Count; i++)
            {
                if (yCoords[i] - yCoords[i - 1] <= tolerance)
                {
                    currentCluster.Add(yCoords[i]);
                }
                else
                {
                    clusters.Add(new List<int>(currentCluster));
                    currentCluster.Clear();
                    currentCluster.Add(yCoords[i]);
                }
            }

            if (currentCluster.Count > 0)
                clusters.Add(currentCluster);

            return clusters;
        }
    }
}
