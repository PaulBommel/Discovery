using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{

    #region Records

    public readonly record struct AnalyseParameter
    {
        public AnalyseParameter(Bitmap bitmap)
        {
            Bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
        }

        /// <summary>
        /// Defines the language the ocr engine should use
        /// </summary>
        public string Language { get; } = "eng";

        public required string TesseractDataPath { get; init; }

        /// <summary>
        /// Returns the picture, which needs to be analysed
        /// </summary>
        public Bitmap Bitmap { get; }
        public AnalyseRegion NavigationRegion { get; init; }
        public AnalyseRegion ChatRegion { get; init; }
        public AnalyseRegion CargoRegion { get; init; }
        public AnalyseRegion CreditsRegion { get; init; }
        public AnalyseRegion ShiptypeRegion { get; init; }
    }

    /// <summary>
    /// Defines an Rectangle on the Picture as part picture to analyse per OCR
    /// </summary>
    public readonly record struct AnalyseRegion()
    {
        /// <summary>
        /// Defines the part picture bounds, which needs to be analysed
        /// </summary>
        public Rectangle Bounds { get; init; }
        /// <summary>
        /// Returns a separate word file for this region
        /// </summary>
        public string WordsFile { get; init; }
        /// <summary>
        /// Returns separate words for this region
        /// </summary>
        public string[] Words { get; init; }
    }

    #endregion
}
