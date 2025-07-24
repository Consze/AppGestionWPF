using WPFApp1.ViewModels;

namespace WPFApp1.Vistas
{
    /// <summary>
    /// Lógica de interacción para UCNotificacion.xaml
    /// </summary>
    public partial class UCNotificacion : System.Windows.Controls.UserControl
    {
        private UCNotificacionViewModel _viewModel;
        public UCNotificacion() // Sobrecarga de constructor para XAML
        {
            InitializeComponent();
        }
        public UCNotificacion(UCNotificacionViewModel ViewModel)
        {
            this._viewModel = ViewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
