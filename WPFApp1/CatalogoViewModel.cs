using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Productos> ColeccionProductos { get; set; }

        private bool _mostrarVentanaAniadirProducto;
        public bool MostrarVentanaAniadirProducto
        {
            get { return _mostrarVentanaAniadirProducto; }
            set
            {
                if (_mostrarVentanaAniadirProducto != value)
                {
                    _mostrarVentanaAniadirProducto = value;
                    OnPropertyChanged(nameof(MostrarVentanaAniadirProducto));
                }
            }
        }  
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ItemDoubleClickCommand { get; private set; }
        public ICommand AniadirProductoCommand { get; private set; }

        public CatalogoViewModel()
        {
            ColeccionProductos = new ObservableCollection<Productos>();
            CargarProductos();
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
        }

        public void AniadirProducto(object parameter)
        {
            this.MostrarVentanaAniadirProducto = true;
            AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
            AniadirProducto AniadirProductoInstanciado = new AniadirProducto(_viewModel);
            AniadirProductoInstanciado.Show();
        }
        private void CargarProductos()
        {
            List<Productos> registros = ProductosRepository.LeerProductos();
            foreach (var producto in registros)
            {
                ColeccionProductos.Add(producto);
            }
        }
        private void EjecutarDobleClickItem(object ProductoClickeado)
        {
            if (ProductoClickeado is Productos producto)
            {
                System.Windows.MessageBox.Show($"Doble Click sobre item. {producto.Nombre}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void OnNuevoProductoAniadido(ProductoAniadidoMensaje Mensaje)
        {
            if (Mensaje?.NuevoProducto != null)
            {
                ColeccionProductos.Add(Mensaje.NuevoProducto);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
