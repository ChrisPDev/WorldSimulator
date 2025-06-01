using System.Collections.Generic;
using WorldSim.Core.Models;
using WorldSim.Config;
using WorldSim.Core.Simulation;

namespace WorldSim.Core.Managers
{
    public class GridManager
    {
        private readonly TerrainGenerator _terrainGenerator = new TerrainGenerator();
        public Dictionary<(int, int), CellData[,]> ChunkCache { get; } = new Dictionary<(int, int), CellData[,]>();

        public CellData[,] GetOrCreateChunk(int chunkX, int chunkY)
        {
            if (!IsChunkInBounds(chunkX, chunkY)) return null;

            if (!ChunkCache.TryGetValue((chunkX, chunkY), out var chunk))
            {
                chunk = new CellData[GridConfig.ChunkSize, GridConfig.ChunkSize];

                var terrainMap = _terrainGenerator.GenerateTerrainMap(GridConfig.ChunkSize, GridConfig.ChunkSize); // Replace later

                for (int y = 0; y < GridConfig.ChunkSize; y++)
                {
                    for (int x = 0; x < GridConfig.ChunkSize; x++)
                    {
                        chunk[x, y] = CellFactory.Create(
                            globalX: chunkX * GridConfig.ChunkSize + x,
                            globalY: chunkY * GridConfig.ChunkSize + y,
                            terrain: terrainMap[x, y]
                        );
                    }
                }

                ChunkCache[(chunkX, chunkY)] = chunk;
            }
            return chunk;
        }

        private bool IsChunkInBounds(int x, int y) =>
            x >= GridConfig.MinChunkX && x <= GridConfig.MaxChunkX &&
            y >= GridConfig.MinChunkY && y <= GridConfig.MaxChunkY;

        public CellData GetCell (int globalX, int globalY)
        {
            int chunkX = globalX / GridConfig.ChunkSize;
            int chunkY = globalY / GridConfig.ChunkSize;
            int localX = globalX % GridConfig.ChunkSize;
            int localY = globalY % GridConfig.ChunkSize;

            var chunk = GetOrCreateChunk(chunkX, chunkY);
            return chunk[localX, localY];
        }
    }
}
