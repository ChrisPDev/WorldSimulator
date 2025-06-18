using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Shapes;
using WorldSimulator.Config;
using WorldSimulator.Models.World;
using WorldSimulator.ViewModels;

namespace WorldSimulator.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private Rectangle _previousSelectedRectangle;
        public MainViewModel ViewModel { get; set; }
        public MapView()
        {
            InitializeComponent();

            int mapWidth = SimulationConfig.MapWidth;
            int mapHeight = SimulationConfig.MapHeight;
            int cellSize = SimulationConfig.CellSize;

            double canvasWidth = mapWidth * cellSize;
            double canvasHeight = mapHeight * cellSize;

            MapCanvas.Width = canvasWidth;
            MapCanvas.Height = canvasHeight;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    var cell = new Cell(x, y);

                    var square = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Fill = Brushes.LightGreen,
                        Stroke = Brushes.DarkGray,
                        StrokeThickness = 1,
                        SnapsToDevicePixels = true,
                        Tag = cell,
                    };

                    square.MouseLeftButtonDown += Square_MouseLeftButtonDown;

                    Canvas.SetLeft(square, x * cellSize);
                    Canvas.SetTop(square, y * cellSize);

                    MapCanvas.Children.Add(square);
                }
            }
        }

        private void Square_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect && rect.Tag is Cell cell)
            {
                if (_previousSelectedRectangle != null)
                {
                    _previousSelectedRectangle.Stroke = Brushes.DarkGray;
                }

                rect.Stroke = Brushes.Red;
                _previousSelectedRectangle = rect;

                if (ViewModel != null)
                {
                    ViewModel.SelectedCell = cell;
                }
            }
        }
    }
}
