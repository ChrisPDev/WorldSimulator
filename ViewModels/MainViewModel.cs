using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using WorldSimulator.Commands;
using WorldSimulator.Models.NatureBase;


namespace WorldSimulator.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainViewModel()
        {
            CreateTestDataCommand = new RelayCommand(_ => CreateTestData());

            simTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            simTimer.Tick += SimTimer_Tick;
            simTimer.Start();

            CurrentSimYear = 0;
        }
        private void SimTimer_Tick(object? sender, EventArgs e)
        {
            if (Elements == null) return;

            Nature previouslySelected = (SelectedIndex >= 0 && SelectedIndex < Elements.Count) ? Elements[SelectedIndex] : null;

            for (int i = Elements.Count - 1; i >= 0; i--)
            {
                Nature element = Elements[i];

                element.IncrementAge();
                element.AttemptGrowth();

                if (element is ProduceCapableNature producer)
                {
                    producer.AttemptProduceGrowth();
                    producer.AgeAndDecayProduce();
                }

                if (element.Age >= element.Lifespan || element.Stage == GrowthStage.None)
                {
                    Elements.RemoveAt(i);
                }
            }

            if (previouslySelected != null)
            {
                int newIndex = Elements.IndexOf(previouslySelected);
                SelectedIndex = newIndex;
            }
            else
            {
                SelectedIndex = -1;
            }

            if (SelectedIndex != null)
            {
                OnPropertyChanged(nameof(SelectedPlantName));
                OnPropertyChanged(nameof(SelectedPlantType));
                OnPropertyChanged(nameof(SelectedPlantProduce));
                OnPropertyChanged(nameof(SelectedPlantAge));
                OnPropertyChanged(nameof(SelectedPlantLifespan));
                OnPropertyChanged(nameof(SelectedPlantStage));
            }

            CurrentSimYear++;
        }
        public ObservableCollection<Nature> Elements { get; set; } = new ObservableCollection<Nature>();
        public ObservableCollection<string> GrowthLogMessages { get; set; } = new ObservableCollection<string>();
        public ICommand CreateTestDataCommand { get; }
        private DispatcherTimer simTimer;
        private int _currentSimYear;
        public int CurrentSimYear
        {
            get => _currentSimYear;
            set
            {
                _currentSimYear = value;
                OnPropertyChanged(nameof(CurrentSimYear));
            }
        }
        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                    UpdateSelectedNature();
                }
            }
        }
        private Nature _selectedNature;
        public Nature SelectedNature
        {
            get => _selectedNature;
            set
            {
                if (SelectedNature != value)
                {
                    _selectedNature = value;
                    OnPropertyChanged(nameof(SelectedNature));
                    OnPropertyChanged(nameof(SelectedPlantName));
                    OnPropertyChanged(nameof(SelectedPlantType));
                    OnPropertyChanged(nameof(SelectedPlantProduce));
                    OnPropertyChanged(nameof(SelectedPlantAge));
                    OnPropertyChanged(nameof(SelectedPlantLifespan));
                    OnPropertyChanged(nameof(SelectedPlantStage));
                }
            }
        }
        public string SelectedPlantName => SelectedNature?.Name ?? string.Empty;
        public string SelectedPlantType => SelectedNature?.Type ?? string.Empty;
        public string SelectedPlantProduce => SelectedNature?.GetProduceSummary() ?? string.Empty;
        public string SelectedPlantAge => SelectedNature != null ? SelectedNature.Age.ToString() : string.Empty;
        public string SelectedPlantLifespan => SelectedNature != null ? SelectedNature.Lifespan.ToString() : string.Empty;
        public string SelectedPlantStage => SelectedNature?.Stage.ToString() ?? string.Empty;
        private void UpdateSelectedNature()
        {
            if (SelectedIndex >= 0 && SelectedIndex < Elements.Count)
            {
                SelectedNature = Elements[SelectedIndex];
                DisplayGrowthHistory(SelectedNature);
            }
            else
            {
                SelectedNature = null;
                GrowthLogMessages.Clear();
            }
        }
        private void DisplayGrowthHistory(Nature nature)
        {
            GrowthLogMessages.Clear();

            if (nature?.LogEntries == null || nature.LogEntries.Count == 0)
                return;

            foreach (var message in nature.LogEntries)
            {
                GrowthLogMessages.Add(message);
            }
        }
        public void CreateTestData()
        {
            Elements.Clear();

            foreach (var item in CreateNatureObservableCollection())
            {
                item.Logger = new Utils.NatureLogger(message =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (item == SelectedNature)
                        {
                            GrowthLogMessages.Add(message);
                        }
                    });
                }, item.LogEntries);

                Elements.Add(item);

                item.Logger?.LogPlanted(item.Name, item.Age);
            }

            SelectedIndex = 0;
        }

        private ObservableCollection<Nature> CreateNatureObservableCollection()
        {
            return new ObservableCollection<Nature>
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
                new Moss ("Moss"),
                new Mycelium("Mycelium", ProduceType.Fungi),
                new Tree("Oak tree", ProduceType.None),
                new Tree("Apple tree", ProduceType.Fruit),
                new Tree("Hazel tree", ProduceType.Nut),
                new Tree("Hybrid tree", ProduceType.Fruit, ProduceType.Nut),
                new Vine("Vine")
            };
        }
    }
}
