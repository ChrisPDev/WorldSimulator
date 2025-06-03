using System.Collections.Generic;
using WorldSim.Core.Models;
using WorldSim.Config;

namespace WorldSim.Core.Managers
{
    /// <summary>
    /// Manages the simulation grid and provides access to terrain-based cell chunks.
    /// </summary>
    public class GridManager
    {
        private readonly TerrainData[,] _globalTerrainMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridManager"/> class.
        /// </summary>
        /// <param name="globalTerrainMap">The full terrain map of the world.</param>
        public GridManager(TerrainData[,] globalTerrainMap)
        {
            _globalTerrainMap = globalTerrainMap;
        }

        /// <summary>
        /// Caches generated chunks to avoid redundant creation.
        /// </summary>
        public Dictionary<(int, int), CellData[,]> ChunkCache { get; } = new Dictionary<(int, int), CellData[,]>();

        /// <summary>
        /// Retrieves a chunk of cells at the specified chunk coordinates, creating it if necessary.
        /// </summary>
        public CellData[,] GetOrCreateChunk(int chunkX, int chunkY)
        {
            if (!IsChunkInBounds(chunkX, chunkY)) return null;

            if (!ChunkCache.TryGetValue((chunkX, chunkY), out var cellChunk))
            {
                cellChunk = new CellData[GridConfig.ChunkSize, GridConfig.ChunkSize];

                for (int y = 0; y < GridConfig.ChunkSize; y++)
                {
                    for (int x = 0; x < GridConfig.ChunkSize; x++)
                    {
                        int globalX = chunkX * GridConfig.ChunkSize + x;
                        int globalY = chunkY * GridConfig.ChunkSize + y;

                        var terrain = _globalTerrainMap[globalX, globalY];
                        cellChunk[x, y] = CellFactory.Create(globalX, globalY, terrain);
                    }
                }

                ChunkCache[(chunkX, chunkY)] = cellChunk;
            }

            return cellChunk;
        }

        /// <summary>
        /// Returns a deep copy of the current terrain grid.
        /// </summary>
        public TerrainData[,] GetGridSnapshot()
        {
            int width = _globalTerrainMap.GetLength(0);
            int height = _globalTerrainMap.GetLength(1);
            var copy = new TerrainData[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var original = _globalTerrainMap[x, y];
                    copy[x, y] = new TerrainData
                    {
                        Type = original.Type,
                        Category = original.Category,
                        Elevation = original.Elevation
                    };
                }
            }

            return copy;
        }

        /// <summary>
        /// Checks if the given chunk coordinates are within the configured bounds.
        /// </summary>
        private bool IsChunkInBounds(int x, int y) =>
            x >= GridConfig.MinChunkX && x <= GridConfig.MaxChunkX &&
            y >= GridConfig.MinChunkY && y <= GridConfig.MaxChunkY;

        /// <summary>
        /// Retrieves a single cell at global coordinates.
        /// </summary>
        public CellData GetCell(int globalX, int globalY)
        {
            int chunkX = globalX / GridConfig.ChunkSize;
            int chunkY = globalY / GridConfig.ChunkSize;
            int localX = globalX % GridConfig.ChunkSize;
            int localY = globalY % GridConfig.ChunkSize;

            var chunk = GetOrCreateChunk(chunkX, chunkY);
            return chunk?[localX, localY]; // Safe access
        }
    }
}
