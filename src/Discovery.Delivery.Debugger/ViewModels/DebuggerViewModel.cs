using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Discovery.Delivery.Debugger.ViewModels
{
    using Commands;
    using Darkstat;

    public class DebuggerViewModel : AbstractViewModel
    {
        #region PropertyChangedEvents

        public static readonly PropertyChangedEventArgs SelectedImageArgs = new(nameof(SelectedImage));
        public static readonly PropertyChangedEventArgs AnalyseResultArgs = new(nameof(AnalyseResult));

        #endregion

        private Bitmap _selectedImage;
        private DeliveryRecord _analyseResult;
        private readonly DeliveryProofEngine _engine = new();
        private readonly Lazy<DarkstatClient> _client = new(() => new DarkstatClient(new DarkstatHttpClientFactory()));
        private readonly Lazy<string[]> _cargoNames;
        private readonly Lazy<string[]> _shiptypes;

        public ICommand SelectImageCommand { get; }
        public ICommand AnalyseCommand { get; }

        public Bitmap SelectedImage
        {
            get => _selectedImage;
            set 
            { 
                if(_selectedImage != value)
                {
                    _selectedImage = value;
                    FirePropertyChanged(SelectedImageArgs);
                }
            }
        }

        public DeliveryRecord AnalyseResult
        {
            get => _analyseResult;
            set 
            {
                if(_analyseResult != value)
                {
                    _analyseResult = value;
                    FirePropertyChanged(AnalyseResultArgs);
                }
            }
        }

        public DebuggerViewModel()
        {
            SelectImageCommand = new RelayCommand(p => SelectImage());
            AnalyseCommand = new RelayCommand(p => Analyse(), p => p is not null);
            _cargoNames = new(InitCargoNames);
            _shiptypes = new(InitShipTypes);
        }

        private void SelectImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.jpg;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedImage = new Bitmap(dialog.FileName);
            }
        }

        private void Analyse()
        {
            if (SelectedImage == null) return;
            var parameterBuilder = new AnalyseParameterBuilder()
            {
                TesseractDataPath = "tessdata"
            }.WithBitmap(SelectedImage)
            .WithCargoRegion(words: _cargoNames.Value, wordFile: "commodity_phrases")
            .WithShiptypeRegion(words: _shiptypes.Value, wordFile: "shipInfos_phrases")
            .WithNavigationRegion( wordFile: "location_phrases");
            var result = _engine.Analyse(parameterBuilder.Build());
            AnalyseResult = result.Record;
            SelectedImage = CloneWithRegionOverlay(SelectedImage, result.Regions);

        }


        public static Bitmap CloneWithRegionOverlay(Bitmap source, Rectangle[] regions, Color? boxColor = null, int thickness = 2)
        {
            Bitmap clone = (Bitmap)source.Clone();

            using (Graphics g = Graphics.FromImage(clone))
            using (Pen pen = new Pen(boxColor ?? Color.Red, thickness))
            using (SolidBrush textBrush = new SolidBrush(boxColor ?? Color.Red))
            {
                for (int i = 0; i < regions.Length; ++i)
                {
                    var region = regions[i];
                    g.DrawRectangle(pen, region);
                }
                    
                
            }

            return clone;
        }

        private string[] InitCargoNames()
        {
            var commoditiesTask = _client.Value.GetCommoditiesAsync();
            commoditiesTask.Wait();
            return [.. commoditiesTask.Result
                                      .Select(c => c.Name)
                                      .Where(c => !string.IsNullOrWhiteSpace(c))
                                      .Distinct()];
        }

        private string[] InitShipTypes()
        {
            var shipTask = _client.Value.GetShipInfosAsync();
            shipTask.Wait();
            return [.. shipTask.Result.Select(c => c.Name)];
        }

    }
}
