using System.Windows;
using NPOI.XSSF.Streaming.Values;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;
using WPFApp1.ViewModels;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para AniadirProducto.xaml
    /// </summary>
    public partial class AniadirProducto : Window
    {
        private AniadirProductoViewModel _viewModel;
        public static AniadirProducto VentanaAniadirProductoVigente { get; private set; }
        public static int Instancias { get; set; }
        public AniadirProducto(AniadirProductoViewModel viewModel)
        {
            Instancias++;
            DataContext = viewModel;
            _viewModel = viewModel;
            _viewModel.CierreSolicitado += OnCierreSolicitado;
            VentanaAniadirProductoVigente = this;
            InitializeComponent();
        }
        private void OnCierreSolicitado(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
            Instancias--;
        }
        private void DobleClickModificarStock(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                Messenger.Default.Publish(new VistaAniadirProductosCantidadModificada { });
            }
        }
    }
}
