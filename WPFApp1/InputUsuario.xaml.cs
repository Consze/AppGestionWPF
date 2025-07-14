using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para InputUsuario.xaml
    /// </summary>
    public partial class InputUsuario : Window
    {
        private InputUsuarioViewModel _viewModel;
        public InputUsuario(InputUsuarioViewModel ViewModel)
        {
            DataContext = ViewModel;
            _viewModel = ViewModel;
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
