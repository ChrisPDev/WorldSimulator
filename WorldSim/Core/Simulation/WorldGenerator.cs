using System;
using System.Collections.Generic;
using WorldSim.Core.Models;
using WorldSim.Config;
using WorldSim.Core.Managers;

namespace WorldSim.Core.Simulation
{
    /// <summary>
    /// Responsible for generating and managing the simulated world terrain.
    /// </summary>
    public class WorldGenerator
    {
        private readonly Random _random = new Random();
        private readonly List<(int x, int y)> _landmassSeeds = new List<(int x, int y)>();
        private GridManager _gridManager;

        /// <summary>
        /// Generates a new world terrain map with landmasses and water bodies.
        /// </summary>
        public TerrainData[,] GenerateWorld()
        {
            int width = GridConfig.ChunkSize * GridConfig.WorldChunkWidth;
            int height = GridConfig.ChunkSize * GridConfig.WorldChunkHeight;

            var map = new TerrainData[width, height];

            // Initialize all cells as saltwater
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[x, y] = new TerrainData
                    {
                        Type = TerrainSubtype.Saltwater,
                        Category = TerrainCategory.Water,
                        Elevation = 0
                    };
                }
            }

            int totalCells = width * height;
            int landmassCount = (GridConfig.WorldChunkWidth / 6) * (GridConfig.WorldChunkHeight / 6);
            int minSize = totalCells / 120;
            int maxSize = totalCells / 40;

            SeedContinents(map, landmassCount, minSize, maxSize);
            ClassifyInlandWater(map);
            ApplyCoastalSand(map);

            _gridManager = new GridManager(map);
            return map;
        }

        /// <summary>
        /// Returns a snapshot of the current world state at a given year.
        /// </summary>
        public WorldState GetWorldState(int year)
        {
            var terrain = _gridManager.GetGridSnapshot();
            var vegetation = new VegetationData[terrain.GetLength(0), terrain.GetLength(1)];
            var minerals = new MineralData[terrain.GetLength(0), terrain.GetLength(1)];

            return new WorldState(year, terrain, vegetation, minerals);
        }

        /// <summary>
        /// Loads a previously saved world state into the generator.
        /// </summary>
        public void LoadWorldState(WorldState state)
        {
            _gridManager = new GridManager(state.TerrainMap);
        }

        /// <summary>
        /// Converts isolated saltwater cells surrounded by land into freshwater.
        /// </summary>
        private void ClassifyInlandWater(TerrainData[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    var cell = map[x, y];

                    if (cell.Category == TerrainCategory.Water && cell.Type == TerrainSubtype.Saltwater)
                    {
                        bool surroundedByLand =
                            map[x - 1, y].Category == TerrainCategory.Land &&
                            map[x + 1, y].Category == TerrainCategory.Land &&
                            map[x, y - 1].Category == TerrainCategory.Land &&
                            map[x, y + 1].Category == TerrainCategory.Land;

                        if (surroundedByLand)
                        {
                            cell.Type = TerrainSubtype.Freshwater;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Seeds and grows landmasses across the map using random placement and expansion.
        /// </summary>
        private void SeedContinents(TerrainData[,] map, int landmassCount, int minSize, int maxSize)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int i = 0; i < landmassCount; i++)
            {
                int seedX, seedY;
                int attempts = 0;

                do
                {
                    seedX = _random.Next(width / 10, width - width / 10);
                    seedY = _random.Next(height / 10, height - height / 10);
                    attempts++;
                }
                while (IsTooCloseToLand(map, seedX, seedY, 5) && attempts < 100);

                if (attempts >= 100)
                    continue;

                int landmassSize = _random.Next(minSize, maxSize);
                _landmassSeeds.Add((seedX, seedY));

                GrowLandmass(map, seedX, seedY, landmassSize);
            }
        }
        /// <summary>
        /// Checks if a given coordinate is too close to existing land.
        /// </summary>
        private bool IsTooCloseToLand(TerrainData[,] map, int x, int y, int minDistance)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int dy = -minDistance; dy <= minDistance; dy++)
            {
                for (int dx = -minDistance; dx <= minDistance; dx++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                    {
                        if (map[nx, ny].Category == TerrainCategory.Land)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Expands a landmass from a seed point using a randomized flood-fill algorithm.
        /// </summary>
        private void GrowLandmass(TerrainData[,] map, int startX, int startY, int maxTiles)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            var queue = new Queue<(int x, int y)>();
            var visited = new HashSet<(int x, int y)>();

            queue.Enqueue((startX, startY));
            visited.Add((startX, startY));

            int tilesPlaced = 0;

            while (queue.Count > 0 && tilesPlaced < maxTiles)
            {
                var (x, y) = queue.Dequeue();

                if (map[x, y].Category == TerrainCategory.Water)
                {
                    map[x, y].Type = TerrainSubtype.Soil;
                    map[x, y].Category = TerrainCategory.Land;
                    map[x, y].Elevation = 0;
                    tilesPlaced++;
                }

                foreach (var (nx, ny) in GetNeighbors(x, y, width, height))
                {
                    if (!visited.Contains((nx, ny)) && _random.NextDouble() < 0.6)
                    {
                        queue.Enqueue((nx, ny));
                        visited.Add((nx, ny));
                    }
                }
            }
        }

        /// <summary>
        /// Returns the 4-directional neighbors of a cell within bounds.
        /// </summary>
        private IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int width, int height)
        {
            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    yield return (nx, ny);
                }
            }
        }

        /// <summary>
        /// Applies sand terrain to land cells adjacent to water, simulating coastlines.
        /// </summary>
        private void ApplyCoastalSand(TerrainData[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
                {
                    var cell = map[x, y];

                    if (cell.Category == TerrainCategory.Land && cell.Type != TerrainSubtype.Sand)
                    {
                        bool isCoastal =
                            map[x - 1, y].Category == TerrainCategory.Water ||
                            map[x + 1, y].Category == TerrainCategory.Water ||
                            map[x, y - 1].Category == TerrainCategory.Water ||
                            map[x, y + 1].Category == TerrainCategory.Water;

                        if (isCoastal)
                        {
                            double chance = 0.6 + _random.NextDouble() * 0.2;

                            if (_random.NextDouble() < chance)
                            {
                                cell.Type = TerrainSubtype.Sand;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a read-only list of all landmass seed coordinates used during generation.
        /// </summary>
        public IReadOnlyList<(int x, int y)> LandMassSeeds => _landmassSeeds.AsReadOnly();
    }
}
