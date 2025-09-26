using FuzzySharp;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using Tesseract;

namespace Discovery.Delivery.DataProviders
{
    /// <summary>
    /// Gathers the shiptype from a bitmap
    /// </summary>
    internal class ShiptypeDataProvider : IRegionDataProvider<string>
    {
        public ShiptypeDataProvider(AnalyseRegion region)
        {
            Region = region;
        }

        public AnalyseRegion Region { get; }
        public string Charset => Charsets.Shiptype;
        
        [SupportedOSPlatform("windows")]
        public string GetData(TesseractEngine engine, Bitmap source)
        {
            if (!string.IsNullOrWhiteSpace(Region.WordsFile))
            {
                engine.SetVariable("user_words_suffix", Region.WordsFile);
            }

            var text = string.Empty;

            using (var cropped = source.Clone(Region.Bounds, source.PixelFormat))
            {
                using (var pix = cropped.ToPix())
                {
                    engine.SetVariable("tessedit_char_whitelist", Charset);
                    using (var page = engine.Process(pix))
                    {
                        text = page.GetText();                        
                    }
                }
            }

            return GetFuzzyShipType(text);
        }

        private string GetFuzzyShipType(string text)
        {
            if (Region.Words is not null && Region.Words.Length > 0)
            {
                return Process.ExtractOne(text, Region.Words).Value;
            }
            return text;
        }
    }
}
