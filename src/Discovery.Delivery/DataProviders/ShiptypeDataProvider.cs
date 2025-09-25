using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

using Tesseract;
using FuzzySharp;

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
        public string Charset { get; } = Charsets.UpercaseLetters + Charsets.LowercaseLetters + Charsets.Numbers + "\" ";

        public string GetData(TesseractEngine engine, Bitmap source)
        {
            TemporaryWordsFile? wordFile = null;

            if (!string.IsNullOrWhiteSpace(Region.WordsFile))
            {
                if (File.Exists(Region.WordsFile))
                {
                    engine.SetVariable("user_words_suffix", Region.WordsFile);
                }
                else
                {
                    wordFile = new TemporaryWordsFile(Region.Words, Region.WordsFile, engine);
                }
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

            if (wordFile.HasValue)
                wordFile.Value.Dispose();

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
