using WorldSimulator.Config;

namespace WorldSimulator.Models.World
{
    public class WorldMap
    {
        public int MapWidth { get; }
        public int MapHeight { get; }
        public int ChunkWidth { get; }
        public int ChunkHeight { get; }

        public Chunk[,] Chunks { get; }

        public WorldMap(int mapWidth, int mapHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            int chunkWidth = SimulationConfig.ChunkWidth;
            int chunkHeight = SimulationConfig.ChunkHeight;

            int chunksX = mapWidth / chunkWidth;
            int chunksY = mapHeight / chunkHeight;

            Chunks = new Chunk[chunksX, chunksY];

            for (int x = 0; x < chunksX; x++)
            {
                for (int y = 0; y < chunksY; y++)
                {
                    Chunks[x, y] = new Chunk(x, y);
                }
            }
        }
        public Chunk GetChunk(int chunkX, int chunkY)
        {
            if (chunkX < 0 || chunkX >= Chunks.GetLength(0) || chunkY < 0 || chunkY >= Chunks.GetLength(1))
            {
                return null;
            }

            return Chunks[chunkX, chunkY];
        }
        public Cell GetCell(int worldX, int worldY)
        {
            int chunkX = worldX / ChunkWidth;
            int chunkY = worldY / ChunkHeight;

            Chunk chunk = GetChunk(chunkX, chunkY);

            if (chunk == null)
            {
                return null;
            }

            int localX = worldX % ChunkWidth;
            int localY = worldY % ChunkHeight;

            return chunk.Cells[localX, localY];
        }
    }
}
