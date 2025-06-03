using WorldSim.Core.Models;

namespace WorldSim.Core.Simulation
{
    /// <summary>
    /// Represents a snapshot of the world's state at a given simulation year.
    /// </summary>
    public class WorldState
    {
        /// <summary>
        /// The simulation year this state represents.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The terrain map at this point in time.
        /// </summary>
        public TerrainData[,] TerrainMap { get; set; }

        /// <summary>
        /// The vegetation map at this point in time.
        /// </summary>
        public VegetationData[,] VegetationMap { get; set; }

        /// <summary>
        /// The mineral map at this point in time.
        /// </summary>
        public MineralData[,] MineralMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldState"/> class.
        /// </summary>
        /// <param name="year">The simulation year.</param>
        /// <param name="terrain">The terrain map.</param>
        /// <param name="vegetation">The vegetation map.</param>
        /// <param name="minerals">The mineral map.</param>
        public WorldState(int year, TerrainData[,] terrain, VegetationData[,] vegetation, MineralData[,] minerals)
        {
            Year = year;
            TerrainMap = terrain;
            VegetationMap = vegetation;
            MineralMap = minerals;
        }
    }
}
