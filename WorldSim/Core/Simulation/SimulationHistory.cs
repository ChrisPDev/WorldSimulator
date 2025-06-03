using System.Collections.Generic;

namespace WorldSim.Core.Simulation
{
    /// <summary>
    /// Manages historical snapshots of the simulation world state.
    /// </summary>
    public class SimulationHistory
    {
        private readonly Dictionary<int, WorldState> _history = new Dictionary<int, WorldState>();

        /// <summary>
        /// Saves a snapshot of the world state for a specific year.
        /// If a snapshot already exists for that year, it will be overwritten.
        /// </summary>
        /// <param name="year">The simulation year.</param>
        /// <param name="state">The world state to save.</param>
        public void SaveSnapshot(int year, WorldState state)
        {
            _history[year] = state;
        }

        /// <summary>
        /// Loads a snapshot of the world state for a specific year.
        /// </summary>
        /// <param name="year">The simulation year.</param>
        /// <returns>The saved world state, or null if not found.</returns>
        public WorldState LoadSnapshot(int year)
        {
            WorldState state;
            return _history.TryGetValue(year, out state) ? state : null;
        }

        /// <summary>
        /// Checks if a snapshot exists for the specified year.
        /// </summary>
        /// <param name="year">The simulation year.</param>
        /// <returns>True if a snapshot exists; otherwise, false.</returns>
        public bool HasSnapshot(int year)
        {
            return _history.ContainsKey(year);
        }

        /// <summary>
        /// Clears all saved snapshots from history.
        /// </summary>
        public void Clear()
        {
            _history.Clear();
        }
    }
}
