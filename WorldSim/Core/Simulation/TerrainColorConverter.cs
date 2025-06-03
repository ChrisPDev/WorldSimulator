using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WorldSim.Core.Models;
using WorldSim.Config;

namespace WorldSim.Core.Simulation
{
    /// <summary>
    /// Converts a TerrainSubtype value into a corresponding Brush for UI rendering.
    /// </summary>
    public class TerrainColorConverter : IValueConverter
    {
        /// <summary>
        /// Converts a TerrainSubtype to a Brush using the TerrainColorConfig.
        /// </summary>
        /// <param name="value">The TerrainSubtype value.</param>
        /// <param name="targetType">The target type (should be Brush).</param>
        /// <param name="parameter">Optional parameter (unused).</param>
        /// <param name="culture">Culture info (unused).</param>
        /// <returns>A Brush representing the terrain color, or a fallback if not found.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var subtype = value as TerrainSubtype?;

            if (subtype.HasValue && TerrainColorConfig.TerrainColors.TryGetValue(subtype.Value, out var brush))
            {
                return brush;
            }

            return TerrainColorConfig.FallbackBrush;
        }

        /// <summary>
        /// Not supported. Throws NotSupportedException if called.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack is not supported for TerrainColorConverter.");
        }
    }
}
