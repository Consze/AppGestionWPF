using System.Windows;
namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ExportarProductos.xaml
    /// </summary>
    public partial class ExportarProductos : Window
    {
        private ExportarProductosViewModel _viewModel;
        public static int InstanciaVistas { get; set; }
        public ExportarProductos(ExportarProductosViewModel ViewModel)
        {
            InstanciaVistas++;
            DataContext = ViewModel;
            _viewModel = ViewModel;
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            InstanciaVistas--;
        }
    }
}
