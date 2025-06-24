using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para AniadirProducto.xaml
    /// </summary>
    public partial class AniadirProducto : Window
    {
        private AniadirProductoViewModel _viewModel;
        public AniadirProducto(AniadirProductoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
            _viewModel.CierreSolicitado += OnCierreSolicitado;
        }

        private void OnCierreSolicitado(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
