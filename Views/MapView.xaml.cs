using System.Windows.Controls;
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
            DataContextChanged += (s, e) =>
            {
                ViewModel = DataContext as MainViewModel;
            };
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Cell cell && ViewModel != null)
            {
                foreach (var c in ViewModel.AllCells)
                {
                    c.IsSelected = false;
                }

                cell.IsSelected = true;
                ViewModel.Selection.SelectedCell = cell;
            }
        }
    }
}
