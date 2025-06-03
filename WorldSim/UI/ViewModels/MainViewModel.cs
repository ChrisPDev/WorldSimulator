using System.ComponentModel;
using System.Collections.ObjectModel;
using WorldSim.Config;
using WorldSim.Core.Models;
using WorldSim.Core.Managers;
using WorldSim.Core.Simulation;
using System.Windows.Threading;
using System;
using System.Threading;
using System.Collections.Generic;

namespace WorldSim.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly SimulationClock _clock = new SimulationClock();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly SimulationHistory _history = new SimulationHistory();
        private readonly WorldGenerator _worldGenerator;

        private int _currentYear;
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                _currentYear = value;
                OnPropertyChanged(nameof(CurrentYear));

                if (_isFollowingCurrentYear)
                {
                    SelectedYear = _currentYear;
                }
            }
        }

        private bool _isFollowingCurrentYear = true;

        private bool _isRunning = true;
        public string PlayPauseLabel => _isRunning ? "Pause" : "Play";
        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear == value)
                {
                    return;
                }

                _selectedYear = value;
                _isFollowingCurrentYear = (_selectedYear == CurrentYear);
                OnPropertyChanged(nameof(SelectedYear));
                OnPropertyChanged(nameof(SimulatorTitle));

                if (_selectedYear != CurrentYear)
                {
                    PauseSimulation();

                    if (_history.HasSnapshot(_selectedYear))
                    {
                        var snapshot = _history.LoadSnapshot(_selectedYear);
                        _worldGenerator.LoadWorldState(snapshot);
                        LoadChunk(_currentChunkX, _currentChunkY);
                    }
                }
                else
                {
                    ResumeSimulation();
                }
            }
        }

        public string SimulatorTitle => $"World - Viewing Year: {SelectedYear}";

        private bool _isTimeControlVisible = true;
        public bool IsTimeControlVisible
        {
            get => _isTimeControlVisible;
            set
            {
                if (_isTimeControlVisible != value)
                {
                    _isTimeControlVisible = value;
                    OnPropertyChanged(nameof(IsTimeControlVisible));
                    OnPropertyChanged(nameof(IsWorldNavControlVisible));
                    OnPropertyChanged(nameof(CurrentPanelLabel));
                }
            }
        }

        public void TogglePanelButton()
        {
            IsTimeControlVisible = !IsTimeControlVisible;
        }

        public bool IsWorldNavControlVisible => !IsTimeControlVisible;

        public string CurrentPanelLabel => IsTimeControlVisible ? "Switch to World Navigation" : "Switch to Time Control";

        private readonly GridManager _gridManager;

        public IReadOnlyList<(int x, int y)> LandMassSeeds => _worldGenerator?.LandMassSeeds ?? new List<(int x, int y)>();

        public ObservableCollection<CellData> VisibleCells { get; } = new ObservableCollection<CellData>();

        private int _currentChunkX = 0;
        private int _currentChunkY = 0;

        public int ChunkSize => GridConfig.ChunkSize;
        
        public MainViewModel()
        {
            _clock.OnTick += OnSimulationTick;
            SelectedYear = 0;
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += (s, e) => _clock.Tick();
            _timer.Start();

            _worldGenerator = new WorldGenerator();
            var globalTerrainMap = _worldGenerator.GenerateWorld();
            OnPropertyChanged(nameof(LandMassSeeds));
            _gridManager = new GridManager(globalTerrainMap);

            LoadChunk(_currentChunkX, _currentChunkY);
        }

        public void CenterViewOnCoordinates(int x, int y)
        {
            int chunkX = x / GridConfig.ChunkSize;
            int chunkY = y / GridConfig.ChunkSize;

            _currentChunkX = chunkX;
            _currentChunkY = chunkY;

            LoadChunk(chunkX, chunkY);
        }

        private void OnSimulationTick(int year)
        {
            CurrentYear = year;

            var state = _worldGenerator.GetWorldState(year);
            _history.SaveSnapshot(year, state);
        }

        public void LoadChunk(int chunkX, int chunkY)
        {
            _currentChunkX = chunkX;
            _currentChunkY = chunkY;

            VisibleCells.Clear();

            var chunk = _gridManager.GetOrCreateChunk(chunkX, chunkY);

            for (int y  = 0; y < ChunkSize; y++)
            {
                for (int x = 0; x < ChunkSize; x++)
                {
                    VisibleCells.Add(chunk[x, y]);
                }
            }

            OnPropertyChanged(nameof(_currentChunkX));
            OnPropertyChanged(nameof(_currentChunkY));
        }

        public void MoveChunk(int dx, int dy)
        {
            int newX = _currentChunkX + dx;
            int newY = _currentChunkY + dy;

            if (!IsChunkInBounds(newX, newY))
            {
                return;
            }

            LoadChunk(newX, newY);
        }

        private bool IsChunkInBounds(int x, int y) =>
            x >= GridConfig.MinChunkX && x <= GridConfig.MaxChunkX &&
            y >= GridConfig.MinChunkY && y <= GridConfig.MaxChunkY;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ToggleSimulation()
        {
            _isRunning = !_isRunning;
            OnPropertyChanged(nameof(PlayPauseLabel));

            if (_isRunning)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void PauseSimulation()
        {
            _timer.Stop();
            _isRunning = false;
            OnPropertyChanged(nameof(PlayPauseLabel));
        }

        private void ResumeSimulation()
        {
            _timer.Start();
            _isRunning = true;
            OnPropertyChanged(nameof(PlayPauseLabel));
        }

        public void ResetSimulation()
        {
            _timer.Stop();
            _history.Clear();
            _clock.Reset();
            CurrentYear = 0;
            SelectedYear = 0;

            _isFollowingCurrentYear = true;
            SelectedYear = CurrentYear;

            _timer.Start();
        }

        public void ResetToCurrentYear()
        {
            _isFollowingCurrentYear = true;
            SelectedYear = CurrentYear;
        }

        public void AdjustSelectedYear(int delta)
        {
            int newYear = SelectedYear + delta;

            if (newYear < 0)
            {
                newYear = 0;
            }

            if (newYear > CurrentYear)
            {
                newYear = CurrentYear;
            }

            SelectedYear = newYear;
        }
    }
}
