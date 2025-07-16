using System.Windows.Controls;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : System.Windows.Controls.UserControl
    {
        private CatalogoViewModel _viewModel;
        public Catalogo(CatalogoViewModel viewModel)
        {
            DataContext = viewModel;
            _viewModel = viewModel;
            InitializeComponent();
        }
        private void Row_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.Item != null)
            {
                var producto = row.Item;
                if (DataContext is CatalogoViewModel viewModel)
                {
                    if (viewModel.ItemDoubleClickCommand != null && viewModel.ItemDoubleClickCommand.CanExecute(producto))
                    {
                        viewModel.ItemDoubleClickCommand.Execute(producto);
                    }
                }
            }
        }
    }
}
