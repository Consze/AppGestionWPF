using System.Windows;

namespace WPFApp1
{
    public partial class EntradaUsuario : Window
    {

        public string NumeroElegido = "";
        public EntradaUsuario()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            NumeroElegido = campoEntrada.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
