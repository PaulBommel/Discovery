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
    /// <summary>
    /// Gathers a timestamp from the chat area
    /// </summary>
    internal class TimeDataProvider : IRegionDataProvider<DateTime?>
    {
        public TimeDataProvider(AnalyseRegion region)
        {
            Region = region;
        }

        public AnalyseRegion Region { get; }
        public string Charset => Charsets.Time;

        public DateTime? GetData(TesseractEngine engine, Bitmap bitmap)
        {
            using (var cropped = bitmap.Clone(Region.Bounds, bitmap.PixelFormat))
            {
                using (var pix = cropped.ToPix())
                {
                    using (var page = engine.Process(pix))
                    {
                        var match = Regex.Match(page.GetText()?.Trim(), @"\b(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})\b");

                        if (match.Success)
                        {
                            string timestampStr = match.Groups[1].Value;

                            if (DateTime.TryParseExact(timestampStr, "yyyy-MM-dd HH:mm:ss",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.AssumeUniversal,
                                out DateTime parsedTimestamp))
                            {
                                return parsedTimestamp;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
