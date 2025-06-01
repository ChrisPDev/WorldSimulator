using System.ComponentModel;

namespace WorldSim.Core.Models
{
    public class CellData : INotifyPropertyChanged
    {
        private int _globalX;
        public int GlobalX
        {
            get => _globalX;
            set
            {
                if (_globalX != value)
                {
                    _globalX = value;
                    OnPropertyChanged(nameof(GlobalX));
                }
            }
        }

        private int _globalY;
        public int GlobalY
        {
            get => _globalY;
            set
            {
                if (_globalY != value)
                {
                    _globalY = value;
                    OnPropertyChanged(nameof(GlobalY));
                }
            }
        }

        private TerrainData _terrain = new TerrainData();
        public TerrainData Terrain
        {
            get => _terrain;
            set
            {
                _terrain = value;
                OnPropertyChanged(nameof(Terrain));
            }
        }

        private VegetationData _vegetation = new VegetationData();
        public VegetationData Vegetation
        {
            get => _vegetation;
            set
            {
                _vegetation = value;
                OnPropertyChanged(nameof(Vegetation));
            }
        }

        private MineralData _mineral = new MineralData();
        public MineralData Mineral
        {
            get => _mineral;
            set
            {
                _mineral = value;
                OnPropertyChanged(nameof(Mineral));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString() => $"({GlobalX}, {GlobalY}) - {Terrain.Category}, {Terrain.Type}, {Vegetation.Type}, {Mineral.Type}";
    }
}
