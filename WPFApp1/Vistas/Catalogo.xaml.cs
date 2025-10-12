using System.Windows.Controls;
using WPFApp1.ViewModels;

namespace WPFApp1.Vistas
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : System.Windows.Controls.UserControl
    {
        private CatalogoViewModel _viewModel;
        public Catalogo()
        {
            InitializeComponent();
        }
        public Catalogo(CatalogoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
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

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is CatalogoViewModel viewModel)
            {
                viewModel.InicializarAsync();
            }
        }
    }
}
