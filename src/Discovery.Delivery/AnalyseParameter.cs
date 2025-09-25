using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{
    #region Builder

    public class AnalyseParameterBuilder
    {

        public Bitmap Bitmap { get; private set; }
        public AnalyseRegion NavigationRegion { get; private set; }
        public AnalyseRegion ChatRegion { get; private set; }
        public AnalyseRegion CargoRegion { get; private set; }
        public AnalyseRegion CreditsRegion { get; private set; }
        public AnalyseRegion ShiptypeRegion { get; private set; }
        public string Language { get; private set; } = "eng";

        public required string TesseractDataPath { get; init; }

        public AnalyseParameterBuilder()
        {
            WithChatRegion(new Rectangle(10, 500, 400, 350));
            WithCargoRegion(new Rectangle(575, 300, 325, 420));
            WithCreditsRegion(new Rectangle(450, 250, 450, 50));
            WithShiptypeRegion(new Rectangle(1000, 325, 400, 50));
        }

        public AnalyseParameter Build()
        {
            Validate();

            return new(Bitmap)
            {
                TesseractDataPath = TesseractDataPath,
                CargoRegion = CargoRegion,
                ChatRegion = ChatRegion,
                CreditsRegion = CreditsRegion,
                NavigationRegion = NavigationRegion,
                ShiptypeRegion = ShiptypeRegion
            };
        }

        #region Validate

        public void Validate()
        {
            if (Bitmap is null)
                throw new InvalidOperationException("No bitmap");

            if (TesseractDataPath is null || !Directory.Exists(TesseractDataPath))
                throw new InvalidOperationException("Data path does not exist");

            #region Regions

            if (NavigationRegion.Words is not null && NavigationRegion.Words.Length > 0 && string.IsNullOrWhiteSpace(NavigationRegion.WordsFile))
            {
                NavigationRegion = NavigationRegion with { WordsFile = Path.Combine(TesseractDataPath, $"{Language}.nav-words") };
            }

            if (CargoRegion.Words is not null && CargoRegion.Words.Length > 0 && string.IsNullOrWhiteSpace(CargoRegion.WordsFile))
            {
                CargoRegion = CargoRegion with { WordsFile = Path.Combine(TesseractDataPath, $"{Language}.cargo-words") };
            }

            if (ChatRegion.Words is not null && ChatRegion.Words.Length > 0 && string.IsNullOrWhiteSpace(ChatRegion.WordsFile))
            {
                ChatRegion = ChatRegion with { WordsFile = Path.Combine(TesseractDataPath, $"{Language}.chat-words") };
            }

            if (CreditsRegion.Words is not null && CreditsRegion.Words.Length > 0 && string.IsNullOrWhiteSpace(CreditsRegion.WordsFile))
            {
                CreditsRegion = CreditsRegion with { WordsFile = Path.Combine(TesseractDataPath, $"{Language}.credits-words") };
            }

            if (ShiptypeRegion.Words is not null && ShiptypeRegion.Words.Length > 0 && string.IsNullOrWhiteSpace(ShiptypeRegion.WordsFile))
            {
                ShiptypeRegion = ShiptypeRegion with { WordsFile = Path.Combine(TesseractDataPath, $"{Language}.ship-words") };
            }

            #endregion
        }

        #endregion

        #region With

        public AnalyseParameterBuilder WithBitmap(Bitmap bitmap)
        {
            Bitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            return WithNavigationRegion(new Rectangle(10, bitmap.Size.Height - 300, 400, 200));
        }

        public AnalyseParameterBuilder WithLanguage(string language)
        {
            Language = language;
            return this;
        }

        public AnalyseParameterBuilder WithNavigationRegion(Rectangle? bounds = null, string wordFile = null, string[] words = null)
        {
            NavigationRegion = NavigationRegion with
            {
                Bounds = bounds ?? NavigationRegion.Bounds,
                WordsFile = wordFile ?? NavigationRegion.WordsFile,
                Words = words ?? NavigationRegion.Words
            };
            return this;
        }

        public AnalyseParameterBuilder WithChatRegion(Rectangle? bounds = null, string wordFile = null, string[] words = null)
        {
            ChatRegion = ChatRegion with
            {
                Bounds = bounds ?? ChatRegion.Bounds,
                WordsFile = wordFile ?? ChatRegion.WordsFile,
                Words = words ?? ChatRegion.Words
            };
            return this;
        }

        public AnalyseParameterBuilder WithCargoRegion(Rectangle? bounds = null, string wordFile = null, string[] words = null)
        {
            CargoRegion = CargoRegion with
            {
                Bounds = bounds ?? CargoRegion.Bounds,
                WordsFile = wordFile ?? CargoRegion.WordsFile,
                Words = words ?? CargoRegion.Words
            };
            return this;
        }

        public AnalyseParameterBuilder WithCreditsRegion(Rectangle? bounds = null, string wordFile = null, string[] words = null)
        {
            CreditsRegion = CreditsRegion with
            {
                Bounds = bounds ?? CreditsRegion.Bounds,
                WordsFile = wordFile ?? CreditsRegion.WordsFile,
                Words = words ?? CreditsRegion.Words
            };
            return this;
        }

        public AnalyseParameterBuilder WithShiptypeRegion(Rectangle? bounds = null, string wordFile = null, string[] words = null)
        {
            ShiptypeRegion = ShiptypeRegion with
            {
                Bounds = bounds ?? ShiptypeRegion.Bounds,
                WordsFile = wordFile ?? ShiptypeRegion.WordsFile,
                Words = words ?? ShiptypeRegion.Words
            };
            return this;
        }

        #endregion
    }

    #endregion

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
