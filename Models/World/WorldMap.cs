using WorldSimulator.Config;

namespace WorldSimulator.Models.World
{
    public class WorldMap
    {
        private readonly int _chunkWidth;
        private readonly int _chunkHeight;
        public int MapWidth { get; }
        public int MapHeight { get; }
        public int ChunkWidth { get; }
        public int ChunkHeight { get; }
        public Chunk[,] Chunks { get; }
        public WorldMap(int mapWidth, int mapHeight)
        {
            MapWidth = mapWidth;
            MapHeight = mapHeight;

            _chunkWidth = SimulationConfig.ChunkWidth;
            _chunkHeight = SimulationConfig.ChunkHeight;

            ChunkWidth = _chunkWidth;
            ChunkHeight = _chunkHeight;

            int chunksX = mapWidth / _chunkWidth;
            int chunksY = mapHeight / _chunkHeight;

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
            int chunkX = worldX / _chunkWidth;
            int chunkY = worldY / _chunkHeight;

            Chunk chunk = GetChunk(chunkX, chunkY);

            if (chunk == null)
            {
                return null;
            }

            int localX = worldX % _chunkWidth;
            int localY = worldY % _chunkHeight;

            return chunk.GetCell(localX, localY);
        }

    }
}
