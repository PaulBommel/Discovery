using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Discovery.Delivery
{
    public sealed class AnalyseParameterBuilder
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
            WithCargoRegion(new Rectangle(575, 315, 325, 405));
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
        }

        #endregion

        #region With
        
        [SupportedOSPlatform("windows")]
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
}
