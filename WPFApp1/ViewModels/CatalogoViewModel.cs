using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Enums;
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
        private readonly IProductosServicio _productoServicio;
        private readonly OrquestadorProductos OrquestadorProductos;
        private readonly ServicioIndexacionProductos _servicioIndexacion;
        public ObservableCollection<ProductoBase> ColeccionProductos { get; set; }
        public ObservableCollection<string> BusquedasRecientes{ get; set; }
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
        private string _textoBusqueda;
        public string TextoBusqueda
        {
            get { return _textoBusqueda; }
            set
            {
                if (_textoBusqueda != value)
                {
                    _textoBusqueda = value;
                    OnPropertyChanged(nameof(TextoBusqueda));
                    if(string.IsNullOrEmpty(value))
                    {
                        BotonCruzVisible = false;
                    }
                    else
                    {
                        BotonCruzVisible = true;
                    }
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
        private bool _botonCruzVisible;
        public bool BotonCruzVisible
        {
            get { return _botonCruzVisible; }
            set
            {
                if (_botonCruzVisible != value)
                {
                    _botonCruzVisible = value;
                    OnPropertyChanged(nameof(BotonCruzVisible));
                }
            }
        }
        private bool _historialVisible;
        public bool HistorialVisible
        {
            get { return _historialVisible; }
            set
            {
                if (_historialVisible != value)
                {
                    _historialVisible = value;
                    OnPropertyChanged(nameof(HistorialVisible));
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
        public ICommand DuplicarItemCommand { get; }
        public ICommand EliminarTextoBusquedaCommand { get; }
        public ICommand BusquedaRecibeFocoCommand { get; }
        public ICommand BusquedaPierdeFocoCommand { get; }
        private ServicioSFX _servicioSFX { get; set; }
        public CatalogoViewModel(IProductosServicio productoServicio, ServicioIndexacionProductos ServicioIndexacion, OrquestadorProductos _orquestador)
        {
            _servicioIndexacion = ServicioIndexacion;
            Procesando = true;
            _productoServicio = productoServicio;
            OrquestadorProductos = _orquestador;
            _tituloVista = "Catálogo";
            _mostrarBotonRegresar = false;
            _mostrarVistaTabular = false;
            _mostrarVistaGaleria = true;
            _botonCruzVisible = false;
            _historialVisible = false;

            BusquedasRecientes = new ObservableCollection<string>();
            ColeccionProductos = new ObservableCollection<ProductoBase>();
            EliminarItemCommand = new RelayCommand<ProductoBase>(EliminarItem);
            LimpiarBusquedaCommand = new RelayCommand<object>(async (param) => await LimpiarBusquedaAsync());
            DuplicarItemCommand = new RelayCommand<ProductoBase>(DuplicarItem);
            ItemDoubleClickCommand = new RelayCommand<object>(async (param) => await EjecutarDobleClickItem(param));
            AniadirProductoCommand = new RelayCommand<object>(async (param) => await MostrarAniadirProducto());
            AlternarFormatoVistaCommand = new RelayCommand<object>(async (param) => await AlternarFormatoVista());
            BuscarTituloCommand = new RelayCommand<object>(async (param) => await BuscarTitulo());
            EliminarTextoBusquedaCommand = new RelayCommand<object>(EliminarTextoBusqueda);
            BusquedaRecibeFocoCommand = new RelayCommand<object>(BusquedaRecibeFoco);
            BusquedaPierdeFocoCommand = new RelayCommand<object>(BusquedaPierdeFoco);

            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
            Messenger.Default.Subscribir<ProductoModificadoMensaje>(OnProductoModificado);
            Procesando = false;
            _servicioSFX = new ServicioSFX();
            BusquedasRecientes.Add("item 1");
        }
        public void BusquedaPierdeFoco(object parameter)
        {
            HistorialVisible = false;
            //Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
        }
        public void BusquedaRecibeFoco(object parameter)
        {
            HistorialVisible = true;
            //Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
        }
        public void EliminarTextoBusqueda(object parameter)
        {
            TextoBusqueda = string.Empty;
        }
        public async Task InicializarAsync()
        {
            Procesando = true;
            await CargarEstadoInicialAsync();
            await CargarProductosAsync();
            Procesando = false;
        }
        public async Task CargarEstadoInicialAsync()
        {
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
        }
        public async Task AlternarFormatoVista()
        {
            Procesando = true;
            await AlternarFormatoVistaAsync().ConfigureAwait(false);
            Procesando = false;
        }
        public void EliminarItem(ProductoBase ProductoEliminar)
        {
            Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
            if (ProductoEliminar != null)
            {
                DialogResult eleccionUsuario = MessageBox.Show("¿Eliminar Producto?", "Eliminar Item", MessageBoxButtons.YesNo);
                if (eleccionUsuario == DialogResult.Yes)
                {
                    ColeccionProductos.Remove(ProductoEliminar);
                    _productoServicio.EliminarProducto(ProductoEliminar.ProductoSKU, TipoEliminacion.Logica);
                    _servicioSFX.Confirmar();
                    Notificacion _notificacion = new Notificacion { Mensaje = "Item Eliminado", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                } 
            }
            Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
        }
        public void DuplicarItem(ProductoBase ProductoDuplicar)
        {
            Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
            if (ProductoDuplicar != null)
            {
                DialogResult eleccionUsuario = MessageBox.Show("¿Duplicar Item?", "Confirmar Operación", MessageBoxButtons.YesNo);
                if (eleccionUsuario == DialogResult.Yes)
                {
                    ProductoCatalogo productoCompleto = _productoServicio.RecuperarProductoPorID(ProductoDuplicar.ProductoSKU);
                    ProductoBase ProductoMensaje = productoCompleto;
                    ProductoMensaje.ProductoSKU = OrquestadorProductos.CrearProducto(productoCompleto);
                    ProductoMensaje.Categoria = productoCompleto.CategoriaNombre;
                    Messenger.Default.Publish(new ProductoAniadidoMensaje { NuevoProducto = ProductoMensaje });
                    _servicioSFX.Confirmar();
                    Notificacion _notificacion = new Notificacion { Mensaje = "Item Duplicado!", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                }
            }
            Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
        }
        private async Task BuscarTitulo()
        {
            _servicioSFX.Paginacion();
            if(TextoBusqueda != null)
            {
                this.Procesando = true;
                await Task.Run(() => BuscarProductosTitulos(TextoBusqueda));
                this.Procesando = false;
                string cuerpoNotificacion = string.Empty;
                string IconoAUtilizar = string.Empty;

                if (ColeccionProductos.Count < 1)
                {
                    _servicioSFX.Suspenso();
                    cuerpoNotificacion = "No se hallaron resultados para la busqueda";
                    IconoAUtilizar = Path.GetFullPath(IconoNotificacion.SUSPENSO1);
                    TituloVista = "Sin coincidencias...";
                }
                else
                {
                    _servicioSFX.Confirmar();
                    string verbo = ColeccionProductos.Count > 1 ? "hallaron" : "hallo";
                    string palabra = ColeccionProductos.Count > 1 ? "coincidencias" : "coincidencia";
                    cuerpoNotificacion = $"Se {verbo} {ColeccionProductos.Count} {palabra}!";
                    IconoAUtilizar = Path.GetFullPath(IconoNotificacion.OK);
                    TituloVista = "Coincidencias";
                }
                
                MostrarBotonRegresar = true;
                Notificacion _notificacion = new Notificacion { Mensaje = cuerpoNotificacion, Titulo = TituloVista, IconoRuta = IconoAUtilizar, Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            TextoBusqueda = string.Empty;
        }
        private async Task BuscarProductosTitulos(string Titulo)
        {
            List<ProductoBase> registros = await Task.Run(() => _servicioIndexacion.BuscarTituloProductos(Titulo));

            App.Current.Dispatcher.Invoke(() =>
            {
                ColeccionProductos.Clear();
                foreach (var producto in registros)
                {
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
            VistaElegida vista = new VistaElegida();
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
            _servicioSFX.Shutter();
            PersistenciaConfiguracion.GuardarUltimaVista(vista);
        }
        public async Task MostrarAniadirProducto()
        {
            if (AniadirProducto.Instancias < 1)
            {
                _servicioSFX.Paginacion();
                Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
                MostrarVentanaAniadirProducto = true;
                var _viewModel = App.GetService<AniadirProductoViewModel>();
                await _viewModel.InicializarFormulario();
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
            await foreach (var producto in _productoServicio.LeerProductosAsync())
            {
                ProductoBase _registro = new ProductoBase
                {
                    Nombre = producto.Nombre,
                    ID = producto.ID,
                    Precio = producto.Precio,
                    Categoria = producto.CategoriaNombre,
                    RutaImagen = producto.RutaImagen,
                    ProductoSKU = producto.ProductoSKU,
                    FechaCreacion = producto.FechaCreacion,
                    FechaModificacion = producto.FechaModificacion
                };
                ColeccionProductos.Add(_registro);
            }
        }
        /// <summary>
        /// Inicia la vista de edición de productos
        /// </summary>
        /// <param name="ProductoClickeado"></param>
        private async Task EjecutarDobleClickItem(object ProductoClickeado)
        {
            if(AniadirProducto.Instancias > 0)
            {
                AniadirProducto.VentanaAniadirProductoVigente.Close();
            }
            if (ProductoClickeado is ProductoBase _producto)
            {
                ProductoCatalogo producto = _productoServicio.RecuperarProductoPorID(_producto.ProductoSKU);
                _servicioSFX.Paginacion();
                Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
                var _viewModel = App.GetService<AniadirProductoViewModel>();
                AniadirProducto AniadirProductoInstanciado = new AniadirProducto(_viewModel);
                await _viewModel.InicializarFormulario();
                _viewModel.ConfigurarEdicionDeProducto(producto);
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
                ProductoBase ProductoModificado = Mensaje.ProductoModificado;
                ProductoBase productoAEditar = ColeccionProductos.FirstOrDefault(p => p.ID == ProductoModificado.ID);
                if (productoAEditar != null)
                {
                    productoAEditar.Nombre = ProductoModificado.Nombre;
                    productoAEditar.Precio= ProductoModificado.Precio;
                    productoAEditar.Categoria= ProductoModificado.Categoria;
                    productoAEditar.RutaImagen= string.IsNullOrWhiteSpace(productoAEditar.RutaImagen) ? string.Empty : System.IO.Path.GetFullPath(ProductoModificado.RutaImagen);
                    productoAEditar.FechaModificacion = ProductoModificado.FechaModificacion;
                    productoAEditar.FechaCreacion = ProductoModificado.FechaCreacion;
                }  
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
