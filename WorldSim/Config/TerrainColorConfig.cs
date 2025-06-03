using System.Collections.Generic;
using System.Windows.Media;
using WorldSim.Core.Models;

namespace WorldSim.Config
{
    /// <summary>
    /// Provides color mappings for different terrain subtypes.
    /// </summary>
    public static class TerrainColorConfig
    {
        /// <summary>
        /// Dictionary mapping TerrainSubtype values to their corresponding UI brushes.
        /// </summary>
        public static readonly Dictionary<TerrainSubtype, Brush> TerrainColors = new Dictionary<TerrainSubtype, Brush>
        {
            { TerrainSubtype.Soil, Brushes.SaddleBrown },
            { TerrainSubtype.Sand, Brushes.Khaki },
            { TerrainSubtype.Saltwater, Brushes.LightBlue },
            { TerrainSubtype.Freshwater, Brushes.CornflowerBlue }
        };

        /// <summary>
        /// Fallback brush used when no specific mapping is found.
        /// </summary>
        public static readonly Brush FallbackBrush = Brushes.LightGray;
    }
}
