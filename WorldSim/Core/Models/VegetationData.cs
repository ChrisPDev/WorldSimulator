namespace WorldSim.Core.Models
{
    /// <summary>
    /// Represents vegetation characteristics of a cell.
    /// </summary>
    public class VegetationData : Evolvable<VegetationType>
    {
        /// <summary>
        /// Initializes a new instance with default vegetation type (None).
        /// </summary>
        public VegetationData()
        {
            Type = VegetationType.None;
        }
    }
}
