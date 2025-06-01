using WorldSim.Core.Models;
using System.Windows.Media;

namespace WorldSim.Config
{
    public static class GridConfig
    {
        public const int ChunkSize = 21;

        public const int WorldChunkWidth = 21;
        public const int WorldChunkHeight = 21;

        public static int WorldWidth = ChunkSize * WorldChunkWidth;
        public static int WorldHeight = ChunkSize * WorldChunkHeight;

        public static int MinChunkX => 0;
        public static int MinChunkY => 0;
        public static int MaxChunkX => WorldChunkWidth - 1;
        public static int MaxChunkY => WorldChunkHeight - 1;
    }
}
