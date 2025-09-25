using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Tesseract;
using FuzzySharp;

namespace Discovery.Delivery.DataProviders
{
    /// <summary>
    /// Gathers the Cargo from a bitmap
    /// </summary>
    internal class CargoDataProvider : IRegionDataProvider<IEnumerable<ItemQuantityRecord>>
    {
        public CargoDataProvider(AnalyseRegion region)
        {
            Region = region;
        }
        public AnalyseRegion Region { get; }

        public IEnumerable<ItemQuantityRecord> GetData(TesseractEngine engine, Bitmap source)
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

            using (var cropped = source.Clone(Region.Bounds, source.PixelFormat)
                                       .ScaleImage(1.5f))
            {
                using (var pix = cropped.ToPix())
                {
                    engine.SetVariable("tessedit_char_whitelist", Charsets.UpercaseLetters + Charsets.LowercaseLetters + Charsets.Numbers);
                    using (var page = engine.Process(pix))
                    {
                        var text = page.GetText();

                        var m = Regex.Match(text, @"^\s*(.*?)\s+(\d+)\s*$");
                        if (m.Success)
                        {
                            string itemName = GetFuzzyItem(m.Groups[1].Value.Trim());
                            if (TryGetAmount(m.Groups[2].Value.Trim(), out var amount))
                            {
                                yield return new()
                                {
                                    Commodity = itemName,
                                    Amount = amount
                                };
                            }
                        }
                    }
                }
            }

            if (wordFile.HasValue)
                wordFile.Value.Dispose();
        }

        private string GetFuzzyItem(string text)
        {
            if(Region.Words is not null && Region.Words.Length > 0)
            {
                return Process.ExtractOne(text, Region.Words).Value;
            }
            return text;
        }

        private bool TryGetAmount(string text, out int result)
        {
            text = text.Replace('O', '0')
                       .Replace('o', '0')
                       .Replace('I', '1')
                       .Replace('l', '1')
                       .Replace('B', '8')
                       .Replace('S', '5');
            return int.TryParse(text, out result);
        }
    }
}
