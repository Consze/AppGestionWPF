using System.Windows;
using WPFApp1.Repositorios;

namespace WPFApp1
{

    public partial class EliminarPersona : Window
    {
        public EliminarPersona()
        {
            InitializeComponent();
        }

        private void BotonEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(campoID.Text, out int IDPersona))
            {
                System.Windows.MessageBox.Show("La altura y el peso deben ser números enteros válidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (personaRepository.EliminarPersona(IDPersona))
            {
                System.Windows.MessageBox.Show("La acción se realizó correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                campoID.Clear();
            }
            else
            {
                System.Windows.MessageBox.Show("No se elimino el registro de persona debido a un error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
