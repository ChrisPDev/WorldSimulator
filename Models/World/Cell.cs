using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace WorldSimulator.Models.World
{
    public class Cell : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }

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
            get => _backgroundBrush;
            set
            {
                if (_backgroundBrush != value)
                {
                    _backgroundBrush = value;
                    OnPropertyChanged();
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
