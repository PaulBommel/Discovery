using System.Drawing;
using Tesseract;

namespace Discovery.Delivery.DataProviders
{
    /// <summary>
    /// Interface to extract specific data from a <see cref="AnalyseRegion"/>
    /// </summary>
    /// <typeparam name="T">Type of data, this processor provides</typeparam>
    internal interface IRegionDataProvider<out T>
    {
        T GetData(TesseractEngine engine, Bitmap bitmap);
    }
}
