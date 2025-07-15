using System.Windows.Controls;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ConfigurarSQLServer.xaml
    /// </summary>
    public partial class ConfigurarSQLServer : System.Windows.Controls.UserControl
    {
        private ConfigurarSQLServerViewModel _viewModel;
        public ConfigurarSQLServer(ConfigurarSQLServerViewModel ViewModel)
        {
            this._viewModel = ViewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
