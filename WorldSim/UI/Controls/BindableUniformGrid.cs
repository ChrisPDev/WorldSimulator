using System.Windows;
using System.Windows.Controls.Primitives;

namespace WorldSim.UI.Controls
{
    public class BindableUniformGrid : UniformGrid
    {
        public static readonly DependencyProperty BindableRowsProperty =
            DependencyProperty.Register(
                nameof(BindableRows),
                typeof(int),
                typeof(BindableUniformGrid),
                new PropertyMetadata(0, OnBindableRowsChanged));

        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.Register(
                nameof(BindableColumns),
                typeof(int),
                typeof(BindableUniformGrid),
                new PropertyMetadata(0, OnBindableColumnsChanged));

        public int BindableRows
        {
            get => (int)GetValue(BindableRowsProperty);
            set => SetValue(BindableRowsProperty, value);
        }

        public int BindableColumns
        {
            get => (int)GetValue(BindableColumnsProperty);
            set => SetValue(BindableColumnsProperty, value);
        }

        private static void OnBindableRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableUniformGrid grid)
            {
                grid.Rows = (int)e.NewValue;
            }
        }

        private static void OnBindableColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindableUniformGrid grid)
            {
                grid.Columns = (int)e.NewValue;
            }
        }
    }
}
