using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WorldSim.Core.Models;
using WorldSim.Config;

namespace WorldSim.Core.Simulation
{
    public class TerrainColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var subtype = value as TerrainSubtype?;

            if (subtype.HasValue && TerrainColorConfig.TerrainColors.TryGetValue(subtype.Value, out var brush)) {
                return brush;
            }

            return TerrainColorConfig.FallbackBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported for TerrainColorConverter.");
        }
    }
}
