using System.Windows;
using System.Windows.Controls.Primitives;

namespace WorldSim.UI.Controls
{
    /// <summary>
    /// A UniformGrid that supports data binding for Rows and Columns.
    /// Useful for MVVM scenarios where layout dimensions are dynamic.
    /// </summary>
    public class BindableUniformGrid : UniformGrid
    {
        /// <summary>
        /// DependencyProperty for bindable Rows.
        /// </summary>
        public static readonly DependencyProperty BindableRowsProperty =
            DependencyProperty.Register(
                nameof(BindableRows),
                typeof(int),
                typeof(BindableUniformGrid),
                new PropertyMetadata(0, OnBindableRowsChanged));

        /// <summary>
        /// DependencyProperty for bindable Columns.
        /// </summary>
        public static readonly DependencyProperty BindableColumnsProperty =
            DependencyProperty.Register(
                nameof(BindableColumns),
                typeof(int),
                typeof(BindableUniformGrid),
                new PropertyMetadata(0, OnBindableColumnsChanged));

        /// <summary>
        /// Gets or sets the number of rows in the grid.
        /// </summary>
        public int BindableRows
        {
            get => (int)GetValue(BindableRowsProperty);
            set => SetValue(BindableRowsProperty, value);
        }

        /// <summary>
        /// Gets or sets the number of columns in the grid.
        /// </summary>
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
