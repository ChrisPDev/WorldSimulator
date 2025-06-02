using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldSim.Core.Models;

namespace WorldSim.Core.Simulation
{
    public class WorldState
    {
        public int Year { get; set; }
        public TerrainData[,] TerrainMap { get; set; }
        public VegetationData[,] VegetationMap { get; set; }
        public MineralData[,] MineralMap { get; set; }

        public WorldState(int year, TerrainData[,] terrain, VegetationData[,] vegetation, MineralData[,] minerals)
        {
            Year = year;
            TerrainMap = terrain;
            VegetationMap = vegetation;
            MineralMap = minerals;
        }
    }
}
