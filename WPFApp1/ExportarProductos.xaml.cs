using System.Windows;
namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ExportarProductos.xaml
    /// </summary>
    public partial class ExportarProductos : Window
    {
        private ExportarProductosViewModel _viewModel;
        public static ExportarProductos VentanaExportarProductosVigente { get; private set; }
        public static int Instancias { get; set; }
        public ExportarProductos(ExportarProductosViewModel ViewModel)
        {
            Instancias++;
            DataContext = ViewModel;
            _viewModel = ViewModel;
            VentanaExportarProductosVigente = this;
            InitializeComponent();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Instancias--;
        }
    }
}
