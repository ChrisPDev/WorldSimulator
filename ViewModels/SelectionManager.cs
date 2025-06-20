using System.Collections.ObjectModel;
using System.ComponentModel;
using WorldSimulator.Config;
using WorldSimulator.Models.NatureBase;
using WorldSimulator.Models.World;

namespace WorldSimulator.ViewModels
{
    public class SelectionManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly ObservableCollection<Nature> _elements;
        private readonly ObservableCollection<string> _growthLogMessages;

        private int _selectedIndex = -1;
        private Nature _selectedNature;
        private Cell _selectedCell;

        public SelectionManager(ObservableCollection<Nature> elements, ObservableCollection<string> growthLogMessages)
        {
            _elements = elements;
            _growthLogMessages = growthLogMessages;
        }

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

        public Nature SelectedNature
        {
            get => _selectedNature;
            private set
            {
                if (_selectedNature != value)
                {
                    _selectedNature = value;
                    NotifyNaturePropertiesChanged();
                }
            }
        }

        public Cell SelectedCell
        {
            get => _selectedCell;
            set
            {
                if (_selectedCell != value)
                {
                    _selectedCell = value;
                    OnPropertyChanged(nameof(SelectedCell));
                    OnPropertyChanged(nameof(SelectedChunkX));
                    OnPropertyChanged(nameof(SelectedChunkY));
                    OnPropertyChanged(nameof(SelectedCellDisplay));
                    OnPropertyChanged(nameof(SelectedChunkDisplay));
                }
            }
        }

        public string SelectedPlantName => SelectedNature?.Name ?? string.Empty;
        public string SelectedPlantType => SelectedNature?.Type ?? string.Empty;
        public string SelectedPlantProduce => SelectedNature?.GetProduceSummary() ?? string.Empty;
        public string SelectedPlantAge => SelectedNature?.Age.ToString() ?? string.Empty;
        public string SelectedPlantLifespan => SelectedNature?.Lifespan.ToString() ?? string.Empty;
        public string SelectedPlantStage => SelectedNature?.Stage.ToString() ?? string.Empty;

        public int SelectedChunkX => SelectedCell?.X / SimulationConfig.ChunkWidth ?? -1;
        public int SelectedChunkY => SelectedCell?.Y / SimulationConfig.ChunkHeight ?? -1;
        public string SelectedCellDisplay => SelectedCell != null ? $"({SelectedCell.X}, {SelectedCell.Y})" : string.Empty;
        public string SelectedChunkDisplay => SelectedCell != null ? $"({SelectedChunkX}, {SelectedChunkY})" : string.Empty;
        public string SelectedCellBaseTerrain => SelectedCell != null ? SelectedCell.TerrainType.ToString() : "None";
        public string SelectedCellSubTerrain => SelectedCell != null ? SelectedCell.SubTerrainType.ToString() : "None";

        public void UpdateSelectedNature()
        {
            if (SelectedIndex >= 0 && SelectedIndex < _elements.Count)
            {
                SelectedNature = _elements[SelectedIndex];
                UpdateGrowthLogMessages(SelectedNature);
            }
            else
            {
                SelectedNature = null;
                _growthLogMessages.Clear();
            }
        }

        private void UpdateGrowthLogMessages(Nature nature)
        {
            _growthLogMessages.Clear();

            if (nature?.LogEntries == null || nature.LogEntries.Count == 0)
                return;

            foreach (var message in nature.LogEntries)
            {
                _growthLogMessages.Add(message);
            }
        }

        public void NotifyNaturePropertiesChanged()
        {
            OnPropertyChanged(nameof(SelectedNature));
            OnPropertyChanged(nameof(SelectedPlantName));
            OnPropertyChanged(nameof(SelectedPlantType));
            OnPropertyChanged(nameof(SelectedPlantProduce));
            OnPropertyChanged(nameof(SelectedPlantAge));
            OnPropertyChanged(nameof(SelectedPlantLifespan));
            OnPropertyChanged(nameof(SelectedPlantStage));
            OnPropertyChanged(nameof(SelectedCellBaseTerrain));
            OnPropertyChanged(nameof(SelectedCellSubTerrain));
        }
    }
}
