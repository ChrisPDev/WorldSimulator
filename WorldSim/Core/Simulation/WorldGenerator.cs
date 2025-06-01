using System;
using WorldSim.Core.Models;
using WorldSim.Config;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace WorldSim.Core.Simulation
{
    public class WorldGenerator
    {
        private readonly Random _random = new Random();

        public TerrainData[,] GenerateWorld()
        {
            int width = GridConfig.ChunkSize * GridConfig.WorldChunkWidth;
            int height = GridConfig.ChunkSize * GridConfig.WorldChunkHeight;

            var map = new TerrainData[width, height];

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

            return map;
        }

        private void ClassifyInlandWater(TerrainData[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int y = 1; y < height; y++)
            {
                for (int x = 1; x < width; x++)
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
                while ((IsTooCloseToLand(map, seedX, seedY, 5)) && attempts < 100);

                if (attempts >= 100)
                {
                    continue;
                }

                int landmassSize = _random.Next(minSize, maxSize);
                GrowLandmass(map, seedX, seedY, landmassSize);
            }
        }

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
    }
}
