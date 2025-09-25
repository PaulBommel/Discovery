using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Tesseract;

namespace Discovery.Delivery
{
    using DataProviders;
    public class DeliveryProofEngine
    {

        public AnalyseResult Analyse(in AnalyseParameter parameter)
        {
            using (var engine = new TesseractEngine(parameter.TesseractDataPath, parameter.Language, EngineMode.Default))
            {
                return new()
                {
                    Regions =
                        [
                            parameter.CargoRegion.Bounds,
                            parameter.NavigationRegion.Bounds,
                            parameter.ChatRegion.Bounds,
                            parameter.CreditsRegion.Bounds,
                            parameter.ShiptypeRegion.Bounds
                        ],
                    Record = new()
                    {
                        Destination = new DestinationDataProvider(parameter.NavigationRegion).GetData(engine, parameter.Bitmap),
                        Time = new TimeDataProvider(parameter.ChatRegion).GetData(engine, parameter.Bitmap),
                        Credits = new CreditsDataProvider(parameter.CreditsRegion).GetData(engine, parameter.Bitmap),
                        Cargo = [.. new CargoDataProvider(parameter.CargoRegion).GetData(engine, parameter.Bitmap)],
                        Source = parameter.Bitmap.ToBase64(),
                        SourceType = parameter.Bitmap.RawFormat.ToString(),
                        ShipType = new ShiptypeDataProvider(parameter.ShiptypeRegion).GetData(engine, parameter.Bitmap)
                    }
                };
            }
        }
    }
}
