namespace Caduhd.UserInterface.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// An <see cref="IValueConverter"/> to convert the Wi-Fi signal-to-noise ratio to a image.
    /// </summary>
    public class WiFiSnrToWiFiSnrImageConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapSource> WiFiSnrImageCache;

        static WiFiSnrToWiFiSnrImageConverter()
        {
            WiFiSnrImageCache = new Dictionary<string, BitmapSource>();

            var assembly = Assembly.GetExecutingAssembly();
            var wiFiSnrImageNames = assembly.GetManifestResourceNames().Where(r => r.Contains("wifi_"));

            foreach (string imageName in wiFiSnrImageNames)
            {
                using (var stream = assembly.GetManifestResourceStream(imageName))
                {
                    string name = imageName.Substring(imageName.IndexOf("wifi_"));
                    BitmapSource source = BitmapToBitmapSource(new Bitmap(stream), PixelFormats.Bgr32);

                    WiFiSnrImageCache.Add(imageName.Substring(imageName.IndexOf("wifi_")).Split('.')[0], source);
                }
            }
        }

        /// <summary>
        /// Convert the Wi-Fi signal-to-noise ratio to its corresponding image.
        /// </summary>
        /// <param name="value">The snr value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The result of the conversion.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                BitmapSource source = WiFiSnrImageCache["wifi_none"];

                if (int.TryParse(value.ToString(), out int wiFiSnr))
                {
                    if (60 < wiFiSnr)
                    {
                        source = WiFiSnrImageCache["wifi_strong"];
                    }
                    else if (30 < wiFiSnr)
                    {
                        source = WiFiSnrImageCache["wifi_medium"];
                    }
                    else if (0 < wiFiSnr)
                    {
                        source = WiFiSnrImageCache["wifi_weak"];
                    }
                }

                return source;
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// Converts the images back to Wi-Fi signal-to-noise ratio.
        /// </summary>
        /// <param name="value">The snr value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>The result of the reverse conversion.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static BitmapSource BitmapToBitmapSource(Bitmap bitmap, System.Windows.Media.PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, 96, 96, pixelFormat, null, bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }
    }
}
