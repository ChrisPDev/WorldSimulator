using System.Collections.Generic;
using System.Windows.Media;
using WorldSim.Core.Models;

namespace WorldSim.Config
{
    public class TerrainColorConfig
    {
        public static readonly Dictionary<TerrainSubtype, Brush> TerrainColors = new Dictionary<TerrainSubtype, Brush>
        {
            { TerrainSubtype.Soil, Brushes.SaddleBrown },
            { TerrainSubtype.Sand, Brushes.Khaki },
            { TerrainSubtype.Saltwater, Brushes.LightBlue },
            { TerrainSubtype.Freshwater, Brushes.CornflowerBlue }
        };

        public static readonly Brush FallbackBrush = Brushes.LightGray;
    }
}
