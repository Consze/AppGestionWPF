namespace WPFApp1.ViewModels
{
    /// <summary>
    /// Lógica de interacción para Configuraciones.xaml
    /// </summary>
    public partial class Configuraciones : System.Windows.Controls.UserControl
    {
        private ConfiguracionesViewModel _viewModel;
        public Configuraciones(ConfiguracionesViewModel ViewModel)
        {
            _viewModel = ViewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }
    }
}
