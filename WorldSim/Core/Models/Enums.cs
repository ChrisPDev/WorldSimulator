namespace WorldSim.Core.Models
{
    /// <summary>
    /// High-level terrain classification.
    /// </summary>
    public enum TerrainCategory
    {
        Land,
        Water
    }

    /// <summary>
    /// Specific terrain types.
    /// </summary>
    public enum TerrainSubtype
    {
        Soil,
        Sand,
        Saltwater,
        Freshwater
    }

    /// <summary>
    /// Types of vegetation that can exist in a cell.
    /// </summary>
    public enum VegetationType
    {
        None,
        Grass,
        Flower,
        Bush,
        Tree
    }

    /// <summary>
    /// Types of minerals that can exist in a cell.
    /// </summary>
    public enum MineralType
    {
        None,
        Rock,
        Ore,
        Gemstone,
        Stone,
        Gravel
    }
}
