using WPFApp1.ViewModels;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ExportarProductosU.xaml
    /// </summary>
    public partial class ExportarProductos : System.Windows.Controls.UserControl
    {
        private ExportarProductosViewModel _viewModel;
        public ExportarProductos(ExportarProductosViewModel viewModel)
        {
            DataContext = viewModel;
            this._viewModel = viewModel;
            InitializeComponent();
        }
    }
}
