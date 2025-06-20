using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WorldSimulator.Models.World
{
    public class Cell : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }
        private BaseTerrainType _terrainType = BaseTerrainType.Water;
        public BaseTerrainType TerrainType
        {
            get => _terrainType;
            set
            {
                _terrainType = value;
                OnPropertyChanged(nameof(TerrainType));
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }
        private SubTerrainType _subTerrainType = SubTerrainType.Saltwater;
        public SubTerrainType SubTerrainType
        {
            get => _subTerrainType;
            set
            {
                _subTerrainType = value;
                OnPropertyChanged(nameof(SubTerrainType));
                OnPropertyChanged(nameof(BackgroundBrush));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BorderBrush));
                }
            }
        }
        
        private Brush _backgroundBrush = Brushes.LightGreen;
        public Brush BackgroundBrush
        {
            get
            {
                switch (TerrainType)
                {
                    case BaseTerrainType.Water:
                        return Brushes.LightBlue;
                    case BaseTerrainType.Land:
                        switch (SubTerrainType)
                        {
                            case SubTerrainType.Soil:
                                return Brushes.SaddleBrown;
                            case SubTerrainType.Sand:
                                return Brushes.Khaki;
                            case SubTerrainType.Gravel:
                                return Brushes.Gray;
                            default:
                                return Brushes.Green;
                        }
                    default:
                        return Brushes.Transparent;
                }
            }
        }


        public Brush BorderBrush => IsSelected ? Brushes.Red : Brushes.DarkGray;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Cell ({X}, {Y})";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
