namespace WorldSim.Core.Models
{
    /// <summary>
    /// Represents terrain characteristics of a cell.
    /// </summary>
    public class TerrainData : Evolvable<TerrainSubtype>
    {
        /// <summary>
        /// The general category of terrain (e.g., Land, Water).
        /// </summary>
        public TerrainCategory Category { get; set; } = TerrainCategory.Water;

        /// <summary>
        /// The elevation level of the terrain.
        /// </summary>
        public int Elevation { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance with default values (Saltwater, Water, Elevation 0).
        /// </summary>
        public TerrainData()
        {
            Type = TerrainSubtype.Saltwater;
        }

        /// <summary>
        /// Initializes a new instance with specified values.
        /// </summary>
        public TerrainData(TerrainSubtype type, TerrainCategory category = TerrainCategory.Water, int elevation = 0)
        {
            Type = type;
            Category = category;
            Elevation = elevation;
        }
    }
}
