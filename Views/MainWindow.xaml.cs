using System.Windows;

namespace WorldSimulator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new WorldSimulator.ViewModels.MainViewModel();
            this.DataContext = viewModel;

            MapViewControl.ViewModel = viewModel;
        }
    }
}