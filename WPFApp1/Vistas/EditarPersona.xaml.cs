using System.Windows;
using WPFApp1.DTOS;

namespace WPFApp1
{
    public partial class EditarPersona : Window
    {
        public Persona DetallePersona { get; set; }
        public EditarPersona(Persona RegistroPersona)
        {
            InitializeComponent();
            DetallePersona = RegistroPersona;
            DataContext = this;
        }

        private void editarRegistro_Click(object sender, RoutedEventArgs e)
        {
            Persona PersonaEditar = new Persona(0, "", 0, 0);
            PersonaEditar.nombre = CampoNombre.Text;
            PersonaEditar.altura = Convert.ToInt32(CampoAltura.Text);
            PersonaEditar.peso = Convert.ToInt32(CampoPeso.Text);
            PersonaEditar.id = DetallePersona.id;
            bool resultado = personaRepository.EditarPersona(PersonaEditar);
            if (resultado)
            {
                // TODO: Actualizar registro en colección si se esta mostrando en alguna vista. - Implementar arquitectura Pub/Sub
                System.Windows.MessageBox.Show("La acción se realizó correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("No se modifico ningun dato.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            this.Close();
        }
    }
}
