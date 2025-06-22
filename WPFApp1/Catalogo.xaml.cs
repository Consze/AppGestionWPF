using System.Collections.ObjectModel;
using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : Window
    {
        public ObservableCollection<Persona> ColeccionPersonas { get; set; }
        public Catalogo()
        {
            ColeccionPersonas = new ObservableCollection<Persona>();
            InitializeComponent();
            CargarPersonas();
            DataContext = this;
        }

        private void CargarPersonas()
        {
            List<Persona> registros = personaRepository.LeerRegistros();
            foreach (var persona in registros)
            {
                ColeccionPersonas.Add(persona);
            }
        }
    }
}
