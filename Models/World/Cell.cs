namespace WorldSimulator.Models.World
{
    public class Cell
    {
        public int X { get; }
        public int Y { get; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Cell ({X}, {Y})";
        }
    }
}
