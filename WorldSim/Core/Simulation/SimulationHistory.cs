using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSim.Core.Simulation
{
    public class SimulationHistory
    {
        private readonly Dictionary<int, WorldState> _history = new Dictionary<int, WorldState>();

        public void SaveSnapshot(int year, WorldState state)
        {
            _history[year] = state;
        }

        public WorldState LoadSnapshot(int year)
        {
            WorldState state;
            return _history.TryGetValue(year, out state) ? state : null;
        }

        public bool HasSnapshot(int year)
        {
            return _history.ContainsKey(year);
        }

        public void Clear()
        {
            _history.Clear();
        }
    }
}
