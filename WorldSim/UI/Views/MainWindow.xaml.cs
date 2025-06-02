using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorldSim.UI.ViewModels;
using WorldSim.Core.Models;
using WorldSim.Config;

namespace WorldSim.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new MainViewModel();
            DataContext = _viewModel;
        }

        public static readonly DependencyProperty SelectedCellProperty =
            DependencyProperty.Register(
                "SelectedCell",
                typeof(CellData),
                typeof(MainWindow),
                new PropertyMetadata(null));

        public CellData SelectedCell
        {
            get => (CellData)GetValue(SelectedCellProperty);
            set => SetValue(SelectedCellProperty, value);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    _viewModel.MoveChunk(1, 0);
                    break;
                case Key.Left:
                    _viewModel.MoveChunk(-1, 0);
                    break;
                case Key.Up:
                    _viewModel.MoveChunk(0, -1);
                    break;
                case Key.Down:
                    _viewModel.MoveChunk(0, 1);
                    break;
            }
        }

        private void Cell_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Label label && label.DataContext is CellData cell)
            {
                int chunkX = cell.GlobalX / GridConfig.ChunkSize;
                int chunkY = cell.GlobalY / GridConfig.ChunkSize;

                CoordinatesLbl.Content = $"Coordinates: Global [{cell.GlobalX}, {cell.GlobalY}] | Chunk: [{chunkX}, {chunkY}]";

                SelectedCell = cell;
            }
        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.ToggleSimulation();
            }
        }

        private void ResetSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.ResetSimulation();
            }
        }

        private void Back1Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-1);
        private void Back5Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-5);
        private void Back10Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-10);
        private void Back25Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-25);
        private void Forward1Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(1);
        private void Forward5Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(5);
        private void Forward10Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(10);
        private void Forward25Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(25);

        private void ResetToCurrentYear_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.ResetToCurrentYear();
            }
        }

        private void AdjustYear(int delta)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.AdjustSelectedYear(delta);
            }
        }
    }
}
