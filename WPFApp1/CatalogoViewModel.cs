using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Productos> ColeccionProductos { get; set; }
        public ICommand ItemDoubleClickCommand { get; private set; }

        public CatalogoViewModel()
        {
            ColeccionProductos = new ObservableCollection<Productos>();
            CargarProductos();
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
        }

        private void CargarProductos()
        {
            List<Productos> registros = ProductosRepository.LeerProductos();
            foreach (var producto in registros)
            {
                ColeccionProductos.Add(producto);
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
