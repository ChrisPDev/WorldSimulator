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
            this.DataContext = new WorldSimulator.ViewModels.MainViewModel();
        }
    }
}