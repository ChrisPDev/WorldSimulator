﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using WorldSim.Config;
using WorldSim.Core.Models;
using WorldSim.Core.Managers;
using WorldSim.Core.Simulation;

namespace WorldSim.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the main simulation interface. Handles simulation control, chunk navigation, and UI state.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly SimulationClock _clock = new SimulationClock();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly SimulationHistory _history = new SimulationHistory();
        private readonly WorldGenerator _worldGenerator;
        private readonly GridManager _gridManager;

        private int _currentYear;
        private int _selectedYear;
        private bool _isFollowingCurrentYear = true;
        private bool _isRunning = true;
        private bool _isTimeControlVisible = true;

        private int _currentChunkX = 0;
        private int _currentChunkY = 0;

        private int _zoomLevel = 1;
        private int _zoomCenterX;
        private int _zoomCenterY;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The current year in the simulation.
        /// </summary>
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

        /// <summary>
        /// The year currently selected for viewing.
        /// </summary>
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear == value) return;

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

        /// <summary>
        /// The title displayed in the UI.
        /// </summary>
        public string SimulatorTitle => $"World - Viewing Year: {SelectedYear}";

        /// <summary>
        /// Indicates whether the time control panel is visible.
        /// </summary>
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

        /// <summary>
        /// Indicates whether the world navigation panel is visible.
        /// </summary>
        public bool IsWorldNavControlVisible => !IsTimeControlVisible;

        /// <summary>
        /// Label for the panel toggle button.
        /// </summary>
        public string CurrentPanelLabel => IsTimeControlVisible ? "Switch to World Navigation" : "Switch to Time Control";

        /// <summary>
        /// Label for the play/pause button.
        /// </summary>
        public string PlayPauseLabel => _isRunning ? "Pause" : "Play";

        /// <summary>
        /// The size of a single chunk.
        /// </summary>
        public int ChunkSize => GridConfig.ChunkSize;

        /// <summary>
        /// The currently visible cells in the UI.
        /// </summary>
        public ObservableCollection<CellData> VisibleCells { get; } = new ObservableCollection<CellData>();

        /// <summary>
        /// The coordinates of the landmass seeds used during generation.
        /// </summary>
        public IReadOnlyList<(int x, int y)> LandMassSeeds => _worldGenerator?.LandMassSeeds ?? new List<(int, int)>();

        public int ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                if (_zoomLevel != value && value % 2 == 1)
                {
                    _zoomLevel = value;
                    OnPropertyChanged(nameof(ZoomLevel));
                    UpdateVisibleCells();
                }
            }
        }

        /// <summary>
        /// Initializes the simulation and starts the timer.
        /// </summary>
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

            _zoomCenterX = (GridConfig.WorldChunkWidth * GridConfig.ChunkSize) / 2;
            _zoomCenterY = (GridConfig.WorldChunkHeight * GridConfig.ChunkSize) / 2;

            UpdateVisibleCells();
        }

        /// <summary>
        /// Toggles between time control and world navigation panels.
        /// </summary>
        public void TogglePanelButton()
        {
            IsTimeControlVisible = !IsTimeControlVisible;
        }

        /// <summary>
        /// Moves the view to center on a specific global coordinate.
        /// </summary>
        public void CenterViewOnCoordinates(int x, int y)
        {
            int chunkX = x / GridConfig.ChunkSize;
            int chunkY = y / GridConfig.ChunkSize;

            _currentChunkX = chunkX;
            _currentChunkY = chunkY;

            LoadChunk(chunkX, chunkY);
        }

        /// <summary>
        /// Loads a specific chunk and updates the visible cells.
        /// </summary>
        public void LoadChunk(int chunkX, int chunkY)
        {
            _currentChunkX = chunkX;
            _currentChunkY = chunkY;

            UpdateVisibleCells();

            OnPropertyChanged(nameof(_currentChunkX));
            OnPropertyChanged(nameof(_currentChunkY));
        }


        /// <summary>
        /// Moves the view to a neighboring chunk.
        /// </summary>
        public void MoveChunk(int dx, int dy)
        {
            int newX = _currentChunkX + dx;
            int newY = _currentChunkY + dy;

            if (!IsChunkInBounds(newX, newY)) return;

            LoadChunk(newX, newY);

            _zoomCenterX = newX * GridConfig.ChunkSize + GridConfig.ChunkSize / 2;
            _zoomCenterY = newY * GridConfig.ChunkSize + GridConfig.ChunkSize / 2;

            UpdateVisibleCells();

        }

        /// <summary>
        /// Starts or pauses the simulation.
        /// </summary>
        public void ToggleSimulation()
        {
            _isRunning = !_isRunning;
            OnPropertyChanged(nameof(PlayPauseLabel));

            if (_isRunning)
                _timer.Start();
            else
                _timer.Stop();
        }

        /// <summary>
        /// Resets the simulation to year 0 and clears history.
        /// </summary>
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

        /// <summary>
        /// Resets the view to follow the current simulation year.
        /// </summary>
        public void ResetToCurrentYear()
        {
            _isFollowingCurrentYear = true;
            SelectedYear = CurrentYear;
        }

        /// <summary>
        /// Adjusts the selected year by a delta value.
        /// </summary>
        public void AdjustSelectedYear(int delta)
        {
            int newYear = SelectedYear + delta;

            if (newYear < 0) newYear = 0;
            if (newYear > CurrentYear) newYear = CurrentYear;

            SelectedYear = newYear;
        }

        public void UpdateVisibleCells()
        {
            VisibleCells.Clear();

            int centerChunkX = _zoomCenterX / GridConfig.ChunkSize;
            int centerChunkY = _zoomCenterY / GridConfig.ChunkSize;
            int half = ZoomLevel / 2;

            for (int offsetY = -half; offsetY <= half; offsetY++)
            {
                for (int offsetX = -half; offsetX <= half; offsetX++)
                {
                    int chunkX = centerChunkX + offsetX;
                    int chunkY = centerChunkY + offsetY;

                    if (!IsChunkInBounds(chunkX, chunkY))
                        continue;

                    var chunk = _gridManager.GetOrCreateChunk(chunkX, chunkY);
                    if (chunk == null)
                        continue;

                    for (int y = 0; y < GridConfig.ChunkSize; y++)
                    {
                        for (int x = 0; x < GridConfig.ChunkSize; x++)
                        {
                            VisibleCells.Add(chunk[x, y]);
                        }
                    }
                }
            }
        }

        private void OnSimulationTick(int year)
        {
            CurrentYear = year;
            var state = _worldGenerator.GetWorldState(year);
            _history.SaveSnapshot(year, state);
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

        private bool IsChunkInBounds(int x, int y)
        {
            return x >= GridConfig.MinChunkX && x <= GridConfig.MaxChunkX &&
                   y >= GridConfig.MinChunkY && y <= GridConfig.MaxChunkY;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
