using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WorldSim.UI.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            var stringValue = value.ToString();

            if (stringValue.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo Culture)
        {
            throw new NotSupportedException("ConvertBack is not supported for TerrainColorConverter.");
        }
    }
}
