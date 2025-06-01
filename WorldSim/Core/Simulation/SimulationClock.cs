using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSim.Core.Simulation
{
    public class SimulationClock
    {
        public int CurrentYear { get; private set; } = 0;

        public event Action<int> OnTick;

        public void Tick()
        {
            CurrentYear++;
            OnTick?.Invoke(CurrentYear);
        }

        public void Reset()
        {
            CurrentYear = 0;
            OnTick?.Invoke(CurrentYear);
        }
    }
}
