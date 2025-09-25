using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Tesseract;

namespace Discovery.Delivery
{
    using ImageFormat = System.Drawing.Imaging.ImageFormat;
    internal static class OcrExtensions
    {
        public static Pix ToPix(this Bitmap bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return Pix.LoadFromMemory(stream.ToArray());
            }
        }
        public static string ToBase64(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);

                byte[] imageBytes = ms.ToArray();

                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Bitmap ScaleImage(this Bitmap source, float scale)
        {
            int width = (int)(source.Width * scale);
            int height = (int)(source.Height * scale);
            var scaled = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(scaled))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(source, 0, 0, width, height);
            }

            return scaled;
        }

        internal static bool IsSelectionPixel(this Color pixel)
        {
            double hue = pixel.GetHue();        // 0-360
            double sat = pixel.GetSaturation(); // 0-1
            double bri = pixel.GetBrightness(); // 0-1

            return (hue >= 40 && hue <= 65) && sat > 0.5 && bri > 0.5;
        }
    }
}
