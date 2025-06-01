namespace WorldSim.Core.Models
{
    public class TerrainData : Evolvable<TerrainSubtype>
    {
        public TerrainCategory Category { get; set; } = TerrainCategory.Water;
        public int Elevation { get; set; } = 0;
        public TerrainData()
        {
            Type = TerrainSubtype.Saltwater;
        }

        public TerrainData(TerrainSubtype type, TerrainCategory category = TerrainCategory.Water, int elevation = 0)
        {
            Type = type;
            Category = category;
            Elevation = elevation;
        }
    }
}
