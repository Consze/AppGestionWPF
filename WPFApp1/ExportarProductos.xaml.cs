using System.ComponentModel;
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
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
     
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /**
            if (e.PropertyName == nameof(_viewModel.ExportacionEnProceso))
            {
                if (!_viewModel.ExportacionEnProceso)
                {
                }
                else
                {
                }
            }
            */
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Instancias--;
        }
    }
}
