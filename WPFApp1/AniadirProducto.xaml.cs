using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para AniadirProducto.xaml
    /// </summary>
    public partial class AniadirProducto : Window
    {
        private AniadirProductoViewModel _viewModel;
        public static int Instancias { get; set; }
        public AniadirProducto(AniadirProductoViewModel viewModel)
        {
            Instancias++;
            DataContext = viewModel;
            _viewModel = viewModel;
            _viewModel.CierreSolicitado += OnCierreSolicitado;
            InitializeComponent();
        }

        private void OnCierreSolicitado(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instancias--;
        }
    }
}
