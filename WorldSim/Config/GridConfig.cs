namespace WorldSim.Config
{
    /// <summary>
    /// Configuration values for the simulation grid and world dimensions.
    /// </summary>
    public static class GridConfig
    {
        /// <summary>
        /// The size of each chunk (width and height in cells).
        /// </summary>
        public const int ChunkSize = 21;

        /// <summary>
        /// Number of chunks horizontally across the world.
        /// </summary>
        public const int WorldChunkWidth = 21;

        /// <summary>
        /// Number of chunks vertically across the world.
        /// </summary>
        public const int WorldChunkHeight = 21;

        /// <summary>
        /// Total width of the world in cells.
        /// </summary>
        public static int WorldWidth = ChunkSize * WorldChunkWidth;

        /// <summary>
        /// Total height of the world in cells.
        /// </summary>
        public static int WorldHeight = ChunkSize * WorldChunkHeight;

        /// <summary>
        /// Minimum chunk X coordinate.
        /// </summary>
        public static int MinChunkX => 0;

        /// <summary>
        /// Minimum chunk Y coordinate.
        /// </summary>
        public static int MinChunkY => 0;

        /// <summary>
        /// Maximum chunk X coordinate.
        /// </summary>
        public static int MaxChunkX => WorldChunkWidth - 1;

        /// <summary>
        /// Maximum chunk Y coordinate.
        /// </summary>
        public static int MaxChunkY => WorldChunkHeight - 1;
    }
}
