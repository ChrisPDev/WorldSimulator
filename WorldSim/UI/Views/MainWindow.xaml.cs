using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorldSim.UI.ViewModels;
using WorldSim.Core.Models;
using WorldSim.Config;

namespace WorldSim.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Handles UI events and delegates logic to the MainViewModel.
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

        /// <summary>
        /// Dependency property for the currently selected cell.
        /// </summary>
        public static readonly DependencyProperty SelectedCellProperty =
            DependencyProperty.Register(
                "SelectedCell",
                typeof(CellData),
                typeof(MainWindow),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the currently selected cell in the UI.
        /// </summary>
        public CellData SelectedCell
        {
            get => (CellData)GetValue(SelectedCellProperty);
            set => SetValue(SelectedCellProperty, value);
        }

        /// <summary>
        /// Handles keyboard navigation for moving between chunks.
        /// </summary>
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

        /// <summary>
        /// Displays cell coordinates and sets the selected cell when hovering over a cell.
        /// </summary>
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

        /// <summary>
        /// Toggles simulation play/pause state.
        /// </summary>
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            _viewModel?.ToggleSimulation();
        }

        /// <summary>
        /// Resets the simulation to its initial state.
        /// </summary>
        private void ResetSimulation_Click(object sender, RoutedEventArgs e)
        {
            _viewModel?.ResetSimulation();
        }

        /// <summary>
        /// Resets the view to follow the current simulation year.
        /// </summary>
        private void ResetToCurrentYear_Click(object sender, RoutedEventArgs e)
        {
            _viewModel?.ResetToCurrentYear();
        }

        /// <summary>
        /// Adjusts the selected year by a specified delta.
        /// </summary>
        private void AdjustYear(int delta)
        {
            _viewModel?.AdjustSelectedYear(delta);
        }

        private void Back1Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-1);
        private void Back5Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-5);
        private void Back10Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-10);
        private void Back25Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(-25);
        private void Forward1Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(1);
        private void Forward5Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(5);
        private void Forward10Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(10);
        private void Forward25Btn_Click(object sender, RoutedEventArgs e) => AdjustYear(25);

        /// <summary>
        /// Toggles between time control and world navigation panels.
        /// </summary>
        private void TogglePanelButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel?.TogglePanelButton();
        }

        /// <summary>
        /// Centers the view on a clicked landmass seed.
        /// </summary>
        private void LandmassSeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ValueTuple<int, int> seed)
            {
                _viewModel?.CenterViewOnCoordinates(seed.Item1, seed.Item2);
            }
        }
    }
}
