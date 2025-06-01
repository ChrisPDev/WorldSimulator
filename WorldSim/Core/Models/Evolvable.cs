namespace WorldSim.Core.Models
{
    public abstract class Evolvable<T> where T : struct
    {
        public T Type { get; set; }
        public T? NextStage { get; set; } = default;
        public int TimeSinceChange { get; set; } = 0;
        public int LifeExpectancy { get; set; } = -1;
    }
}
