using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;

namespace Discovery.Delivery.DataProviders
{
    /// <summary>
    /// Gathers the credits from a bitmap
    /// </summary>
    internal class CreditsDataProvider : IRegionDataProvider<int?>
    {
        public CreditsDataProvider(AnalyseRegion region)
        {
            Region = region;
        }

        public AnalyseRegion Region { get; }
        public string Charset => Charsets.Credits;

        [SupportedOSPlatform("windows")]
        public int? GetData(TesseractEngine engine, Bitmap bitmap)
        {
            using (var cropped = bitmap.Clone(Region.Bounds, bitmap.PixelFormat))
            {
                using (var pix = cropped.ToPix())
                {
                    engine.SetVariable("tessedit_char_whitelist", Charset);
                    using (var page = engine.Process(pix))
                    {
                        string text = page.GetText()?.Trim();
                        if (string.IsNullOrWhiteSpace(text))
                            return null;

                        if (int.TryParse(text, out int value))
                            return value;


                        return null;
                    }
                }
            }
        }
    }
}
