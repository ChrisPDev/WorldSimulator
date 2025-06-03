using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorldSim.UI.Converters
{
    /// <summary>
    /// Converts an enum value to Visibility.
    /// Returns Collapsed if the value is null or "None".
    /// </summary>
    public class EnumToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts an enum value to Visibility.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            var stringValue = value.ToString();
            return stringValue.Equals("None", StringComparison.OrdinalIgnoreCase)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// ConvertBack is not supported.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported for EnumToVisibilityConverter.");
        }
    }
}
