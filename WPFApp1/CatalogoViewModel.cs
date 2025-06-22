using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Persona> ColeccionPersonas { get; set; }
        public ICommand ItemDoubleClickCommand { get; private set; }

        public CatalogoViewModel()
        {
            ColeccionPersonas = new ObservableCollection<Persona>();
            CargarPersonas();
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
        }

        private void CargarPersonas()
        {
            List<Persona> registros = personaRepository.LeerRegistros();
            foreach (var persona in registros)
            {
                ColeccionPersonas.Add(persona);
            }
        }
        private void EjecutarDobleClickItem(object personaClickeada)
        {
            if (personaClickeada is Persona persona)
            {
                System.Windows.MessageBox.Show($"Doble Click sobre item. {persona.nombre}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
