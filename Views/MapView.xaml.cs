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
        public MainViewModel ViewModel { get; set; }
        public MapView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Cell cell)
            {
                ViewModel?.GetType().GetProperty("SelectedCell")?.SetValue(ViewModel, cell);
            }
        }
    }
}
