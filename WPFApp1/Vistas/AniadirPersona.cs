using System.Windows;
using WPFApp1.DTOS;
using WPFApp1.Repositorios;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para AniadirPersonaxaml.xaml
    /// </summary>
    public partial class AniadirPersona : Window
    {
        public AniadirPersona()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CampoNombre.Text) ||
               string.IsNullOrWhiteSpace(CampoAltura.Text) ||
               string.IsNullOrWhiteSpace(CampoPeso.Text))
            {
                System.Windows.MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(CampoAltura.Text, out int altura) || !int.TryParse(CampoPeso.Text, out int peso))
            {
                System.Windows.MessageBox.Show("La altura y el peso deben ser números enteros válidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Persona nuevaPersona = new Persona(0, "", 0, 0);
            nuevaPersona.nombre = CampoNombre.Text;
            nuevaPersona.altura = altura;
            nuevaPersona.peso = peso;
            if (personaRepository.AniadirPersona(nuevaPersona))
            {
                CampoNombre.Clear();
                CampoAltura.Clear();
                CampoPeso.Clear();
                System.Windows.MessageBox.Show("La acción se realizó correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Hubo un error al intentar añadir la persona.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
