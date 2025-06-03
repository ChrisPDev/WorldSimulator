using System;

namespace WorldSim.Core.Simulation
{
    /// <summary>
    /// Manages the simulation's progression in time, measured in years.
    /// </summary>
    public class SimulationClock
    {
        /// <summary>
        /// The current year of the simulation.
        /// </summary>
        public int CurrentYear { get; private set; } = 0;

        /// <summary>
        /// Event triggered on each tick (year advancement).
        /// </summary>
        public event Action<int> OnTick;

        /// <summary>
        /// Advances the simulation by one year and triggers the OnTick event.
        /// </summary>
        public void Tick()
        {
            CurrentYear++;
            OnTick?.Invoke(CurrentYear);
        }

        /// <summary>
        /// Resets the simulation clock to year 0 and triggers the OnTick event.
        /// </summary>
        public void Reset()
        {
            CurrentYear = 0;
            OnTick?.Invoke(CurrentYear);
        }
    }
}
