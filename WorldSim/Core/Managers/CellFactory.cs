﻿using WorldSim.Core.Models;

namespace WorldSim.Core.Managers
{
    /// <summary>
    /// Factory for creating new CellData instances with default values.
    /// </summary>
    public static class CellFactory
    {
        public static CellData Create(int globalX, int globalY, TerrainData terrain)
        {
            return new CellData
            {
                GlobalX = globalX,
                GlobalY = globalY,
                Terrain = terrain,
                Vegetation = new VegetationData(),
                Mineral = new MineralData()
            };
        }
    }
}
