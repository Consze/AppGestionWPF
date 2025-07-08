using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private bool _mostrarVistaTabular;
        public bool MostrarVistaTabular 
        {
            get { return _mostrarVistaTabular; }
            set {
                if(_mostrarVistaTabular != value)
                {
                    _mostrarVistaTabular = value;
                    OnPropertyChanged(nameof(MostrarVistaTabular));
                }
            } 
        }
        private bool _mostrarVistaExpandida;
        public bool MostrarVistaExpandida
        {
            get { return _mostrarVistaExpandida; }
            set
            {
                if (_mostrarVistaExpandida != value)
                {
                    _mostrarVistaExpandida = value;
                    OnPropertyChanged(nameof(MostrarVistaExpandida));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ItemDoubleClickCommand { get; private set; }
        public ICommand AniadirProductoCommand { get; private set; }
        public ICommand AlternarFormatoVistaCommand { get; private set; }

        public CatalogoViewModel()
        {
            this._mostrarVistaTabular = false;
            this._mostrarVistaExpandida = true;
            ColeccionProductos = new ObservableCollection<Productos>();
            CargarProductos();
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
            AniadirProductoCommand = new RelayCommand<object>(MostrarAniadirProducto);
            AlternarFormatoVistaCommand = new RelayCommand<object>(AlternarFormatoVista);
            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
            Messenger.Default.Subscribir<ProductoModificadoMensaje>(OnProductoModificado);
        }

        public void AlternarFormatoVista(object parameter)
        {
            if (this.MostrarVistaExpandida)
            {
                this.MostrarVistaExpandida = false;
                this.MostrarVistaTabular = true;
            }
            else
            {
                this.MostrarVistaExpandida = true;
                this.MostrarVistaTabular = false;
            }
        }
        public void MostrarAniadirProducto(object parameter)
        {
            if (AniadirProducto.Instancias < 1)
            {
                this.MostrarVentanaAniadirProducto = true;
                AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
                AniadirProducto AniadirProductoInstanciado = new AniadirProducto(_viewModel);
                AniadirProductoInstanciado.Show();
            }
            else
            {
                AniadirProducto.VentanaAniadirProductoVigente.Activate();
            }
        }
        private void CargarProductos()
        {
            List<Productos> registros = ProductosRepository.LeerProductos();
            foreach (var producto in registros)
            {
                ColeccionProductos.Add(producto);
            }
        }
        /// <summary>
        /// Inicia la vista de edición de productos
        /// </summary>
        /// <param name="ProductoClickeado"></param>
        private void EjecutarDobleClickItem(object ProductoClickeado)
        {
            if(AniadirProducto.Instancias > 0)
            {
                AniadirProducto.VentanaAniadirProductoVigente.Close();
            }
            if (ProductoClickeado is Productos producto)
            {
                AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
                _viewModel.ConfigurarEdicionDeProducto(producto);
                AniadirProducto AniadirProductoInstanciado = new AniadirProducto(_viewModel);
                AniadirProductoInstanciado.Show();
            }
        }
        private void OnNuevoProductoAniadido(ProductoAniadidoMensaje Mensaje)
        {
            if (Mensaje?.NuevoProducto != null)
            {
                ColeccionProductos.Add(Mensaje.NuevoProducto);
            }
        }
        private void OnProductoModificado(ProductoModificadoMensaje Mensaje)
        {
            if(Mensaje?.ProductoModificado != null)
            {
                Productos ProductoModificado = Mensaje.ProductoModificado;
                Productos productoAEditar = ColeccionProductos.FirstOrDefault(p => p.ID == ProductoModificado.ID);
                productoAEditar.Nombre = ProductoModificado.Nombre;
                productoAEditar.Precio= ProductoModificado.Precio;
                productoAEditar.Categoria= ProductoModificado.Categoria;
                productoAEditar.RutaImagen= System.IO.Path.GetFullPath(ProductoModificado.RutaImagen);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
