namespace WorldSim.Core.Models
{
    /// <summary>
    /// Represents an entity that can evolve over time.
    /// </summary>
    /// <typeparam name="T">The type of the evolutionary state (e.g., VegetationType, MineralType).</typeparam>
    public abstract class Evolvable<T> where T : struct
    {
        /// <summary>
        /// The current type or state of the entity.
        /// </summary>
        public T Type { get; set; }

        /// <summary>
        /// The next stage in the evolution, if any.
        /// </summary>
        public T? NextStage { get; set; } = default(T?);

        /// <summary>
        /// Time (in ticks or steps) since the last change.
        /// </summary>
        public int TimeSinceChange { get; set; } = 0;

        /// <summary>
        /// Expected lifespan or duration before evolution or decay.
        /// </summary>
        public int LifeExpectancy { get; set; } = -1;
    }
}
