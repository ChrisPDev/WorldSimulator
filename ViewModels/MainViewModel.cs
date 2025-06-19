using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WorldSimulator.Commands;
using WorldSimulator.Config;
using WorldSimulator.Models.NatureBase;
using WorldSimulator.Models.World;
using WorldSimulator.Utils;

namespace WorldSimulator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public MainViewModel()
        {
            WorldMap = new WorldMap(SimulationConfig.MapWidth, SimulationConfig.MapHeight);
            CreateTestDataCommand = new RelayCommand(_ => CreateTestData());

            _simulationManager = new SimulationManager(Elements, OnSimulationTick);
            _selectionManager = new SelectionManager(Elements, GrowthLogMessages);

            foreach (var chunk in WorldMap.Chunks)
            {
                foreach (var cell in chunk.Cells)
                {
                    AllCells.Add(cell);
                }
            }
        }

        public ObservableCollection<Nature> Elements { get; } = new();
        public ObservableCollection<string> GrowthLogMessages { get; } = new();
        public ObservableCollection<Cell> AllCells { get; } = new();
        public ICommand CreateTestDataCommand { get; }

        private readonly SimulationManager _simulationManager;
        private readonly SelectionManager _selectionManager;

        public SelectionManager Selection => _selectionManager;

        private WorldMap _worldMap;
        public WorldMap WorldMap
        {
            get => _worldMap;
            set
            {
                _worldMap = value;
                OnPropertyChanged(nameof(WorldMap));
            }
        }

        public int CurrentSimYear => _simulationManager?.CurrentSimYear ?? 0;

        public void CreateTestData()
        {
            Elements.Clear();

            foreach (var item in CreateNatureObservableCollection())
            {
                item.Logger = new NatureLogger(message =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (item == _selectionManager.SelectedNature)
                        {
                            GrowthLogMessages.Add(message);
                        }
                    });
                }, item.LogEntries);

                Elements.Add(item);
                item.Logger?.LogPlanted(item.Name, item.Age);
            }

            _selectionManager.SelectedIndex = 0;
        }

        private static ObservableCollection<Nature> CreateNatureObservableCollection() => new()
        {
            new Bush("Berry bush", ProduceType.Blossom, ProduceType.Fruit),
            new Bush("Flower bush", ProduceType.Blossom),
            new Cactus("Cactus", ProduceType.Blossom, ProduceType.Fruit),
            new Fern("Fern"),
            new Flower("Red flower"),
            new Flower("Yellow flower"),
            new Flower("Green flower"),
            new Grass("Short grass"),
            new Grass("Tall grass"),
            new Moss("Moss"),
            new Mycelium("Mycelium", ProduceType.Fungi),
            new Tree("Oak tree", ProduceType.None),
            new Tree("Apple tree", ProduceType.Fruit),
            new Tree("Hazel tree", ProduceType.Nut),
            new Tree("Hybrid tree", ProduceType.Fruit, ProduceType.Nut),
            new Vine("Vine")
        };

        private void OnSimulationTick()
        {
            var selected = _selectionManager.SelectedIndex;
            _selectionManager.SelectedIndex = (selected >= 0 && selected < Elements.Count) ? Elements.IndexOf(Elements[selected]) : -1;

            _selectionManager.NotifyNaturePropertiesChanged();

            OnPropertyChanged(nameof(CurrentSimYear));
        }
    }
}
