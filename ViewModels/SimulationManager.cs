using System.Collections.ObjectModel;
using System.Windows.Threading;
using WorldSimulator.Models.NatureBase;

namespace WorldSimulator.ViewModels
{
    public class SimulationManager
    {
        private readonly DispatcherTimer _simTimer;
        private readonly ObservableCollection<Nature> _elements;
        private readonly Action _onSimulationTick;

        public int CurrentSimYear { get; private set; }

        public SimulationManager(ObservableCollection<Nature> elements, Action onSimulationTick)
        {
            _elements = elements;
            _onSimulationTick = onSimulationTick;

            _simTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _simTimer.Tick += (_, _) => Tick();
            _simTimer.Start();
        }

        public void Tick()
        {
            for (int i = _elements.Count - 1; i >= 0; i--)
            {
                var element = _elements[i];

                element.IncrementAge();
                element.AttemptGrowth();

                if (element is ProduceCapableNature producer)
                {
                    producer.AttemptProduceGrowth();
                    producer.AgeAndDecayProduce();
                }

                if (element.Age >= element.Lifespan || element.Stage == GrowthStage.None)
                {
                    _elements.RemoveAt(i);
                }
            }

            CurrentSimYear++;
            _onSimulationTick?.Invoke();
        }
    }
}
