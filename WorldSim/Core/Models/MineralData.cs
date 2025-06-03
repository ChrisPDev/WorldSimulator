namespace WorldSim.Core.Models
{
    /// <summary>
    /// Represents mineral characteristics of a cell.
    /// </summary>
    public class MineralData : Evolvable<MineralType>
    {
        /// <summary>
        /// Initializes a new instance with default mineral type (None).
        /// </summary>
        public MineralData()
        {
            Type = MineralType.None;
        }
    }
}
