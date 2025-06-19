using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using WorldSimulator.Config;

namespace WorldSimulator.Converters
{
    public class CellToCanvasYConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int y)
            {
                return (double)(y * SimulationConfig.CellSize);
            }

            return 0.0;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
