using System.Windows;
namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ExportarProductos.xaml
    /// </summary>
    public partial class ExportarProductos : Window
    {
        private ExportarProductosViewModel _viewModel;
        public ExportarProductos(ExportarProductosViewModel ViewModel)
        {
            DataContext = ViewModel;
            _viewModel = ViewModel;
            InitializeComponent();
        }
    }
}
