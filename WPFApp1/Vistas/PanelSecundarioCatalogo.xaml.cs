using WPFApp1.ViewModels;

namespace WPFApp1.Vistas
{
    /// <summary>
    /// Lógica de interacción para PanelSecundarioCatalogo.xaml
    /// </summary>
    public partial class PanelSecundarioCatalogo : System.Windows.Controls.UserControl
    {
        private PanelSecundarioCatalogoViewModel _viewModel;
        public PanelSecundarioCatalogo(PanelSecundarioCatalogoViewModel ViewModel)
        {
            DataContext = ViewModel;
            _viewModel = ViewModel;
            InitializeComponent();
        }
    }
}
