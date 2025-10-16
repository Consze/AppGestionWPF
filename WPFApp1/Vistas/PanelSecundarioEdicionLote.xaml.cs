using WPFApp1.ViewModels;

namespace WPFApp1.Vistas
{
    /// <summary>
    /// Lógica de interacción para PanelSecundarioEdicionLote.xaml
    /// </summary>
    public partial class PanelSecundarioEdicionLote : System.Windows.Controls.UserControl
    {
        private PanelSecundarioEdicionLoteViewModel _viewModel;
        public PanelSecundarioEdicionLote()
        {
            InitializeComponent();
        }
        public PanelSecundarioEdicionLote(PanelSecundarioEdicionLoteViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
            _viewModel = ViewModel;
        }
    }
}
