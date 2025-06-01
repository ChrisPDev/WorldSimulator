using System.ComponentModel;
using System.Collections.ObjectModel;
using WorldSim.Config;
using WorldSim.Core.Models;
using WorldSim.Core.Managers;

namespace WorldSim.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GridManager _gridManager = new GridManager();
        public ObservableCollection<CellData> VisibleCells { get; } = new ObservableCollection<CellData>();

        private int _currentChunkX = 0;
        private int _currentChunkY = 0;

        public int ChunkSize => GridConfig.ChunkSize;
        
        public MainViewModel()
        {
            LoadChunk(_currentChunkX, _currentChunkY);
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
    }
}
