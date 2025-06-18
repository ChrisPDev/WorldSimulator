using WorldSimulator.Config;

namespace WorldSimulator.Models.World
{
    public class Chunk
    {
        public int ChunkX { get; }
        public int ChunkY { get; }

        public Cell[,] Cells { get; }

        public Chunk(int chunkX, int chunkY)
        {
            ChunkX = chunkX;
            ChunkY = chunkY;
            int width = SimulationConfig.ChunkWidth;
            int height = SimulationConfig.ChunkHeight;

            Cells = new Cell[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int worldX = chunkX * width + x;
                    int worldY = chunkY * height + y;

                    Cells[x, y] = new Cell(worldX, worldY);
                }
            }
        }
    }
}
