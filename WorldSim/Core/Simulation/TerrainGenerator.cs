using System;
using System.Collections.Generic;
using WorldSim.Core.Models;

namespace WorldSim.Core.Simulation
{
    public class TerrainGenerator
    {
        private readonly Random _random = new Random();

        public TerrainData[,] GenerateTerrainMap(int width, int height, double landRatio = 0.3)
        {
            var map = InitializeMap(width, height, TerrainCategory.Water, TerrainSubtype.Saltwater);

            int totalCells = width * height;
            int landCellsTarget = (int)(totalCells * landRatio);

            GenerateClusteredLand(map, width, height, landCellsTarget);

            return map;
        }

        private void GenerateClusteredLand(TerrainData[,] map, int width, int height, int landCellsTarget)
        {
            int landCells = 0;

            int startX = _random.Next(width);
            int startY = _random.Next(height);

            var stack = new Stack<(int x, int y)>();
            stack.Push((startX, startY));

            while (stack.Count > 0 && landCells < landCellsTarget)
            {
                var (x, y) = stack.Pop();
                
                if (map[x, y].Category == TerrainCategory.Water)
                {
                    map[x, y] = new TerrainData
                    {
                        Category = TerrainCategory.Land,
                        Type = TerrainSubtype.Soil
                    };

                    landCells++;

                    foreach (var (nx, ny) in GetNeighbors(x, y, width, height))
                    {
                        stack.Push((nx, ny));
                    }
                }
            }
        }

        private IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int width, int height)
        {
            var directions = new (int dx, int dy)[]
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            foreach (var (dx, dy) in directions) {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    yield return (nx, ny);
                }
            }
        }

        private TerrainData[,] InitializeMap(int width, int height, TerrainCategory category, TerrainSubtype subtype)
        {
            var map = new TerrainData[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x= 0;  x < width; x++)
                {
                    map[x, y] = new TerrainData
                    {
                        Category = category,
                        Type = subtype
                    };
                }
            }

            return map;
        }
    }
}
