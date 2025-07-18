using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFApp1.DTOS;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ListaPersonas.xaml
    /// </summary>
    public partial class ListaPersonas : Window
    {
        public ObservableCollection<Persona> Personas { get; set; } 
        public ListaPersonas()
        {
            Personas = new ObservableCollection<Persona>();
            InitializeComponent();
            CargarPersonas(); 
            DataContext = this;
        }

        private void CargarPersonas()
        {
            List<Persona> registros = personaRepository.LeerRegistros();
            foreach (var persona in registros)
            {
                Personas.Add(persona);
            }
        }
        private void ListaDeRegistros_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("Doble Click detectado", "Detección", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ListaDeRegistros_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                System.Windows.MessageBox.Show("Enter detectado", "Detección", MessageBoxButton.OK, MessageBoxImage.Information);
                Persona personaSeleccionada = (Persona)ListaDeRegistros.SelectedItem;
                EditarPersona _edicion = new EditarPersona(personaSeleccionada);
                _edicion.Show();
            }
        }
    }
}
