using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WPFApp1
{
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Persona> ColeccionPersonas { get; set; }

        public CatalogoViewModel()
        {
            ColeccionPersonas = new ObservableCollection<Persona>();
            CargarPersonas();
        }

        private void CargarPersonas()
        {
            List<Persona> registros = personaRepository.LeerRegistros();
            foreach (var persona in registros)
            {
                ColeccionPersonas.Add(persona);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
