using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ConfigurarSQLServer.xaml
    /// </summary>
    public partial class ConfigurarSQLServer : Window
    {
        private ConfigurarSQLServerViewModel _viewModel;
        public ConfigurarSQLServer(ConfigurarSQLServerViewModel ViewModel)
        {
            _viewModel = ViewModel;
            DataContext = ViewModel;
            _viewModel.DialogoCerrado += OnDialogoCerrado;
            InitializeComponent();
        }
        private void OnDialogoCerrado(object sender, bool resultado)
        {
            DialogResult = resultado;
            Close();
        }
    }
}
