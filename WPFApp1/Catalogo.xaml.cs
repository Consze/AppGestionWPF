namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para CatalogoU.xaml
    /// </summary>
    public partial class Catalogo : System.Windows.Controls.UserControl
    {
        private CatalogoViewModel _viewModel;
        public Catalogo(CatalogoViewModel viewModel)
        {
            DataContext = viewModel;
            _viewModel = viewModel;
            InitializeComponent();
        }
    }
}
