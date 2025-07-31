using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Repositorios;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public enum VistaElegida
    {
        Tabla,
        Galeria,
        Ninguna
    }
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        private readonly IProductoServicio _productoServicio;
        public ObservableCollection<Productos> ColeccionProductos { get; set; }
        public bool _mostrarBotonRegresar;
        public bool MostrarBotonRegresar
        {
            get { return _mostrarBotonRegresar; }
            set
            {
                if (_mostrarBotonRegresar != value)
                {
                    _mostrarBotonRegresar = value;
                    OnPropertyChanged(nameof(MostrarBotonRegresar));
                }
            }
        }
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
        private string _tituloVista;
        public string TituloVista
        {
            get { return _tituloVista; }
            set
            {
                if (_tituloVista != value)
                {
                    _tituloVista = value;
                    OnPropertyChanged(nameof(TituloVista));
                }
            }
        }
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
        private bool _mostrarVistaGaleria;
        public bool MostrarVistaGaleria
        {
            get { return _mostrarVistaGaleria; }
            set
            {
                if (_mostrarVistaGaleria != value)
                {
                    _mostrarVistaGaleria = value;
                    OnPropertyChanged(nameof(MostrarVistaGaleria));
                }
            }
        }
        private bool _procesando = true;
        public bool Procesando {
            get { return _procesando; }
            set
            {
                if (_procesando != value)
                {
                    _procesando = value;
                    OnPropertyChanged(nameof(Procesando));
                }
            }        
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ItemDoubleClickCommand { get; private set; }
        public ICommand AniadirProductoCommand { get; private set; }
        public ICommand AlternarFormatoVistaCommand { get; private set; }
        public ICommand BuscarTituloCommand { get; private set; }
        public ICommand LimpiarBusquedaCommand { get; private set; }
        public ICommand EliminarItemCommand { get; private set; }
        private ServicioSFX _servicioSFX { get; set; }
        public CatalogoViewModel(IProductoServicio productoServicio)
        {
            Procesando = true;
            _productoServicio = productoServicio;
            Task.Run(async () => await CargarEstadoInicialAsync());
            Task.Run(async () => await CargarProductosAsync());
            _tituloVista = "Catálogo";
            _mostrarBotonRegresar = false;
            _mostrarVistaTabular = false;
            _mostrarVistaGaleria = true;
            ColeccionProductos = new ObservableCollection<Productos>();
            EliminarItemCommand = new RelayCommand<Productos>(EliminarItem);
            LimpiarBusquedaCommand = new RelayCommand<object>(async (param) => await LimpiarBusquedaAsync());
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
            AniadirProductoCommand = new RelayCommand<object>(MostrarAniadirProducto);
            AlternarFormatoVistaCommand = new RelayCommand<object>(async (param) => await AlternarFormatoVista());
            BuscarTituloCommand = new RelayCommand<object>(async (param) => await BuscarTitulo());
            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
            Messenger.Default.Subscribir<ProductoModificadoMensaje>(OnProductoModificado);
            Procesando = false;
            _servicioSFX = new ServicioSFX();
        }

        public async Task CargarEstadoInicialAsync()
        {
            Procesando = true;
            VistaElegida vista = PersistenciaConfiguracion.LeerUltimaVista();
            switch(vista)
            {
                case VistaElegida.Ninguna:
                case VistaElegida.Galeria:
                    MostrarVistaGaleria = true;
                    MostrarVistaTabular = false;
                    break;
                case VistaElegida.Tabla:
                    MostrarVistaGaleria = false;
                    MostrarVistaTabular = true;
                    break;
            }
            Procesando = false;
        }
        public async Task AlternarFormatoVista()
        {
            Procesando = true;
            await AlternarFormatoVistaAsync().ConfigureAwait(false);
        }
        public void EliminarItem(Productos ProductoEliminar)
        {
            if(ProductoEliminar != null)
            {
                ColeccionProductos.Remove(ProductoEliminar);
                _productoServicio.EliminarProducto(ProductoEliminar.ID);
                _servicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Item Eliminado", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        private async Task BuscarTitulo()
        {
            _servicioSFX.Paginacion();
            Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
            InputUsuarioViewModel viewModel = new InputUsuarioViewModel("Ingrese el titulo a buscar");
            InputUsuario dialogo = new InputUsuario(viewModel);
            bool? resultado = dialogo.ShowDialog();
            Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
            if (resultado == true && !(string.IsNullOrWhiteSpace(viewModel.Entrada)))
            {
                this.Procesando = true;
                string titulo = viewModel.Entrada;
                await Task.Run(() => BuscarProductosTitulos(titulo));
                this.Procesando = false;
                string cuerpoNotificacion = string.Empty;
                string IconoAUtilizar = string.Empty;

                if (ColeccionProductos.Count < 1)
                {
                    _servicioSFX.Suspenso();
                    cuerpoNotificacion = "No se hallaron resultados para la busqueda...";
                    IconoAUtilizar = Path.GetFullPath(IconoNotificacion.SUSPENSO1);
                    TituloVista = "Sin coincidencias...";
                }
                else
                {
                    _servicioSFX.Confirmar();
                    cuerpoNotificacion = $"Se hallaron {ColeccionProductos.Count} coincidencias!";
                    IconoAUtilizar = Path.GetFullPath(IconoNotificacion.OK);
                    TituloVista = "Resultados de Busqueda";
                }
                
                MostrarBotonRegresar = true;
                Notificacion _notificacion = new Notificacion { Mensaje = cuerpoNotificacion, Titulo = "Operación Completada", IconoRuta = IconoAUtilizar, Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        private async Task BuscarProductosTitulos(string Titulo)
        {
            var indexadorService = App.GetService<IndexadorProductoService>();
            List<CoincidenciasBusqueda> coincidencias = await Task.Run(() => indexadorService.RecuperarRegistros(Titulo));
            List<Productos> registros = await Task.Run(() => indexadorService.RecuperarProductos(coincidencias));

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ColeccionProductos.Clear();
                foreach (var producto in registros)
                {
                    producto.RutaImagen = Path.GetFullPath(producto.RutaImagen);
                    ColeccionProductos.Add(producto);
                }
            });
        }
        private async Task LimpiarBusquedaAsync()
        {
            this.Procesando = true;
            ColeccionProductos.Clear();
            await CargarProductosAsync();
            this.Procesando = false;
            _servicioSFX.Shuffle();
            this.MostrarBotonRegresar = false;
            this.TituloVista = "Catálogo";
        }
        public async Task AlternarFormatoVistaAsync()
        {
            _servicioSFX.Shutter();
            VistaElegida vista = new VistaElegida();
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (MostrarVistaGaleria)
                {
                    vista = VistaElegida.Tabla;
                    MostrarVistaGaleria = false;
                    MostrarVistaTabular = true;
                }
                else
                {
                    vista = VistaElegida.Galeria;
                    MostrarVistaGaleria = true;
                    MostrarVistaTabular = false;
                }
                PersistenciaConfiguracion.GuardarUltimaVista(vista);
            });
            Procesando = false;
        }
        public void MostrarAniadirProducto(object parameter)
        {
            if (AniadirProducto.Instancias < 1)
            {
                _servicioSFX.Paginacion();
                Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
                MostrarVentanaAniadirProducto = true;
                var _viewModel = App.GetService<AniadirProductoViewModel>();
                AniadirProducto AniadirProductoInstanciado = new AniadirProducto(_viewModel);
                AniadirProductoInstanciado.Show();
            }
            else
            {
                AniadirProducto.VentanaAniadirProductoVigente.Activate();
            }
        }
        private async Task CargarProductosAsync()
        {
            List<Productos> registros = await Task.Run(() => _productoServicio.LeerProductos());

            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
                foreach (var producto in registros)
                {
                    ColeccionProductos.Add(producto);
                }
                Procesando = false;
            });
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
                _servicioSFX.Paginacion();
                Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
                var _viewModel = App.GetService<AniadirProductoViewModel>();
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
                if (productoAEditar != null)
                {
                    productoAEditar.Nombre = ProductoModificado.Nombre;
                    productoAEditar.Precio= ProductoModificado.Precio;
                    productoAEditar.Categoria= ProductoModificado.Categoria;
                    productoAEditar.RutaImagen= string.IsNullOrWhiteSpace(productoAEditar.RutaImagen) ? string.Empty : System.IO.Path.GetFullPath(ProductoModificado.RutaImagen);
                }
                
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
