using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPFUI.CustomConverters
{
    /// <summary>
    /// Converter class to convert a file path to a BitmapImage.
    /// </summary>
    public class FileToBitmapConverter : IValueConverter
    {
        // Dictionary to cache BitmapImages for file paths
        private static readonly Dictionary<string, BitmapImage> _locations = new Dictionary<string, BitmapImage>();

        /// <summary>
        /// Converts a file path to a BitmapImage.
        /// </summary>
        /// <param name="value">The file path.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>A BitmapImage if the file path is valid; otherwise, null.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string filename))
            {
                return null;
            }

            if (!_locations.ContainsKey(filename))
            {
                _locations.Add(filename, new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}{filename}", UriKind.Absolute)));
            }

            return _locations[filename];
        }

        /// <summary>
        /// ConvertBack is not implemented and always returns null.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The converter parameter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>Always returns null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
