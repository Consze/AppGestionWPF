using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using WPFApp1.Conmutadores;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Enums;
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
    public enum EleccionPanelSecundario
    {
        Venta,
        Edicion
    }
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoConmutador _productoServicio;
        private readonly OrquestadorProductos OrquestadorProductos;
        private readonly ServicioIndexacionProductos _servicioIndexacion;
        private readonly IConmutadorEntidadGenerica<Categorias> servicioCategorias;
        private readonly IConmutadorEntidadGenerica<Marcas> servicioMarcas;
        private readonly IConmutadorEntidadGenerica<Ubicaciones> servicioUbicaciones;
        public ObservableCollection<ProductoBase> ColeccionProductos { get; set; }
        public ObservableCollection<Categorias> ColeccionCategorias { get; set; }
        public ObservableCollection<Marcas> ColeccionMarcas { get; set; }
        public ObservableCollection<Ubicaciones> ColeccionUbicaciones { get; set; }
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
        private bool _BusquedaAvanzadaActiva;
        public bool BusquedaAvanzadaActiva
        {
            get { return _BusquedaAvanzadaActiva; }
            set
            {
                if (_BusquedaAvanzadaActiva != value)
                {
                    _BusquedaAvanzadaActiva = value;
                    OnPropertyChanged(nameof(BusquedaAvanzadaActiva));
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

        //Bindings de busqueda avanzada
        private Categorias _categoriaBuscada;
        public Categorias CategoriaBuscada
        {
            get { return _categoriaBuscada; }
            set
            {
                if (_categoriaBuscada != value)
                {
                    _categoriaBuscada = value;
                    OnPropertyChanged(nameof(CategoriaBuscada));
                }
            }
        }
        private Marcas _marcaBuscada;
        public Marcas MarcaBuscada
        {
            get { return _marcaBuscada; }
            set
            {
                if (_marcaBuscada != value)
                {
                    _marcaBuscada = value;
                    OnPropertyChanged(nameof(MarcaBuscada));
                }
            }
        }
        private Ubicaciones _ubicacionBuscada;
        public Ubicaciones UbicacionBuscada
        {
            get { return _ubicacionBuscada; }
            set
            {
                if (_ubicacionBuscada != value)
                {
                    _ubicacionBuscada = value;
                    OnPropertyChanged(nameof(UbicacionBuscada));
                }
            }
        }
        private bool? _toggleItemsExistentes;
        public bool? ToggleItemsExistentes
        {
            get { return _toggleItemsExistentes; }
            set
            {
                if (_toggleItemsExistentes != value)
                {
                    _toggleItemsExistentes = value;
                    OnPropertyChanged(nameof(ToggleItemsExistentes));
                }
            }
        }
        private bool? _toggleItemsPrecioPublico;
        public bool? ToggleItemsPrecioPublico
        {
            get { return _toggleItemsPrecioPublico; }
            set
            {
                if (_toggleItemsPrecioPublico != value)
                {
                    _toggleItemsPrecioPublico = value;
                    OnPropertyChanged(nameof(ToggleItemsPrecioPublico));
                }
            }
        }
        private bool? _toggleItemsVisibilidadWeb;
        public bool? ToggleItemsVisibilidadWeb
        {
            get { return _toggleItemsVisibilidadWeb; }
            set
            {
                if (_toggleItemsVisibilidadWeb != value)
                {
                    _toggleItemsVisibilidadWeb = value;
                    OnPropertyChanged(nameof(ToggleItemsVisibilidadWeb));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SeleccionarBusquedaPreviaCommand { get; }
        public ICommand ItemDoubleClickCommand { get; private set; }
        public ICommand AniadirProductoCommand { get; private set; }
        public ICommand AlternarFormatoVistaCommand { get; private set; }
        public ICommand EjecutarBusquedaCommand { get; private set; }
        public ICommand LimpiarBusquedaCommand { get; private set; }
        public ICommand EliminarItemCommand { get; private set; }
        public ICommand DuplicarItemCommand { get; }
        public ICommand EliminarTextoBusquedaCommand { get; }
        public ICommand BusquedaRecibeFocoCommand { get; }
        public ICommand BusquedaPierdeFocoCommand { get; }
        public ICommand AniadirItemCarritoCommand { get; }
        public ICommand AniadirItemLoteEdicionCommand { get; }
        public ICommand ElegirTodoLoteCommand { get; }
        public ICommand ToggleBusquedaAvanzadaCommand { get; }
        public ICommand ReiniciarParametrosBusquedaCommand { get; }
        private ServicioSFX _servicioSFX { get; set; }
        public CatalogoViewModel(ProductoConmutador productoServicio, ServicioIndexacionProductos ServicioIndexacion, OrquestadorProductos _orquestador,
            IConmutadorEntidadGenerica<Categorias> _servicioCategorias, IConmutadorEntidadGenerica<Marcas> _servicioMarcas, IConmutadorEntidadGenerica<Ubicaciones> _servicioUbicaciones)
        {
            Procesando = true;

            _servicioIndexacion = ServicioIndexacion;
            servicioCategorias = _servicioCategorias;
            servicioMarcas = _servicioMarcas;
            servicioUbicaciones = _servicioUbicaciones;
            _productoServicio = productoServicio;
            OrquestadorProductos = _orquestador;

            _tituloVista = "Catálogo";
            _mostrarBotonRegresar = false;
            _mostrarVistaTabular = false;
            _mostrarVistaGaleria = true;
            _botonCruzVisible = false;
            _historialVisible = false;

            //Busqueda Avanzada bindings
            BusquedaAvanzadaActiva = false;
            CategoriaBuscada = null;
            MarcaBuscada = null;
            UbicacionBuscada = null;
            ToggleItemsExistentes = true;
            ToggleItemsPrecioPublico = null;
            ToggleItemsVisibilidadWeb = null;

            BusquedasRecientes = new ObservableCollection<string>();
            ColeccionCategorias = new ObservableCollection<Categorias>();
            ColeccionProductos = new ObservableCollection<ProductoBase>();
            ColeccionMarcas = new ObservableCollection<Marcas>();
            ColeccionUbicaciones = new ObservableCollection<Ubicaciones>();

            EliminarItemCommand = new RelayCommand<ProductoBase>(EliminarItem);
            LimpiarBusquedaCommand = new RelayCommand<object>(async (param) => await LimpiarBusquedaAsync());
            DuplicarItemCommand = new RelayCommand<ProductoBase>(DuplicarItem);
            ItemDoubleClickCommand = new RelayCommand<object>(async (param) => await EjecutarDobleClickItem(param));
            AniadirProductoCommand = new RelayCommand<object>(async (param) => await MostrarAniadirProducto());
            AlternarFormatoVistaCommand = new RelayCommand<object>(async (param) => await AlternarFormatoVista());
            EjecutarBusquedaCommand = new RelayCommand<object>(async (param) => await EjecutarBusqueda());
            EliminarTextoBusquedaCommand = new RelayCommand<object>(EliminarTextoBusqueda);
            BusquedaRecibeFocoCommand = new RelayCommand<object>(BusquedaRecibeFoco);
            BusquedaPierdeFocoCommand = new RelayCommand<object>(BusquedaPierdeFoco);
            ToggleBusquedaAvanzadaCommand = new RelayCommand<object>(ToggleBusquedaAvanzada);
            AniadirItemLoteEdicionCommand = new RelayCommand<ProductoBase>(async (param) => await AniadirItemLoteEdicion(param));
            AniadirItemCarritoCommand = new RelayCommand<ProductoBase>(async (param) => await AniadirItemCarrito(param));
            ElegirTodoLoteCommand = new RelayCommand<ProductoBase>(async (param) => await ElegirTodoLote(param));
            SeleccionarBusquedaPreviaCommand = new RelayCommand<object>(async (param) => await SeleccionarBusquedaPrevia(param));
            ReiniciarParametrosBusquedaCommand = new RelayCommand<object>(ReiniciarParametrosBusqueda);

            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
            Messenger.Default.Subscribir<ProductoModificadoMensaje>(OnProductoModificado);
            Messenger.Default.Subscribir<ProductoEliminadoMessage>(OnProductoEliminado);

            Procesando = false;
            _servicioSFX = new ServicioSFX();
        }
        public void ReiniciarParametrosBusqueda(object parameter)
        {
            BusquedaAvanzadaActiva = false;
            CategoriaBuscada = null;
            MarcaBuscada = null;
            UbicacionBuscada = null;
            ToggleItemsExistentes = true;
            ToggleItemsPrecioPublico = null;
            ToggleItemsVisibilidadWeb = null;
        }
        public void ToggleBusquedaAvanzada(object parameter)
        {
            BusquedaAvanzadaActiva = !BusquedaAvanzadaActiva;
        }
        public async Task ElegirTodoLote(object parameter)
        {
            DialogResult eleccionUsuario = MessageBox.Show("¿Editar productos o vender?", "Eliminar Item", MessageBoxButtons.YesNo);
            if (eleccionUsuario == DialogResult.Yes)
            {
                await AniadirTodoLoteEdicion();
            }
            else
            {
                await AniadirTodoLoteVenta();
            }
        }
        public async Task AniadirItemLoteEdicion(ProductoBase ProductoElegido)
        {
            await IniciarPanelSecundario(EleccionPanelSecundario.Edicion);
            Messenger.Default.Publish(new NuevoProductoEdicion { ProductoAniadido = ProductoElegido });
        }
        public async Task AniadirTodoLoteEdicion()
        {
            await IniciarPanelSecundario(EleccionPanelSecundario.Edicion);

            foreach(ProductoBase item in ColeccionProductos)
            {
                Messenger.Default.Publish(new NuevoProductoEdicion { ProductoAniadido = item });
            }
        }
        public async Task AniadirTodoLoteVenta()
        {
            await IniciarPanelSecundario(EleccionPanelSecundario.Venta);

            foreach (ProductoBase item in ColeccionProductos)
            {
                ProductoCatalogo productoAniadir = new ProductoCatalogo
                {
                    ProductoSKU = item.ProductoSKU,
                    Nombre = item.Nombre,
                    RutaImagen = item.RutaImagen,
                    ID = item.ID,
                    EsEliminado = item.EsEliminado,
                    FechaCreacion = item.FechaCreacion,
                    FechaModificacion = item.FechaModificacion,
                    Categoria = item.Categoria,
                    Precio = item.Precio
                };
                Ventas ItemVender = new Ventas { ItemVendido = productoAniadir, Cantidad = 1 };
                Messenger.Default.Publish(new NuevoProductoCarritoMessage { VentaDTO = ItemVender });
            }
        }
        public async Task IniciarPanelSecundario(EleccionPanelSecundario Caso)
        {
            PanelSecundarioStatusRequest EstadoCarrito = new PanelSecundarioStatusRequest();
            Messenger.Default.Publish(EstadoCarrito);

            switch(Caso)
            {
                case EleccionPanelSecundario.Edicion:
                    if (!EstadoCarrito.PanelSecundarioExiste || !(EstadoCarrito.ViewModel is PanelSecundarioEdicionLoteViewModel))
                    {
                        PanelSecundarioEdicionLoteViewModel _viewModel = App.GetService<PanelSecundarioEdicionLoteViewModel>();
                        await _viewModel.InicializarVista();
                        Messenger.Default.Publish(new PanelSecundarioBoxing { ViewModelGenerico = _viewModel, TituloPanel = "Edición de Lote" });
                    }
                    break;

                case EleccionPanelSecundario.Venta:
                    if (!EstadoCarrito.PanelSecundarioExiste || !(EstadoCarrito.ViewModel is PanelSecundarioCatalogoViewModel))
                    {
                        PanelSecundarioCatalogoViewModel _viewModel = App.GetService<PanelSecundarioCatalogoViewModel>();
                        await _viewModel.InicializarVM();
                        Messenger.Default.Publish(new PanelSecundarioBoxing { ViewModelGenerico = _viewModel, TituloPanel = "Lista de Ventas" });
                    }
                    break;
            }
            
            Messenger.Default.Publish(new TogglePanelSecundarioMW { MostrarPanel = true });
        }
        public async Task AniadirItemCarrito(ProductoBase ProductoElegido)
        {
            await IniciarPanelSecundario(EleccionPanelSecundario.Venta);
            ProductoCatalogo productoAniadir = new ProductoCatalogo
            {
                ProductoSKU = ProductoElegido.ProductoSKU,
                Nombre = ProductoElegido.Nombre,
                RutaImagen = ProductoElegido.RutaImagen,
                ID = ProductoElegido.ID,
                EsEliminado = ProductoElegido.EsEliminado,
                FechaCreacion = ProductoElegido.FechaCreacion,
                FechaModificacion = ProductoElegido.FechaModificacion,
                Categoria = ProductoElegido.Categoria,
                Precio = ProductoElegido.Precio
            };
            Ventas ItemVender = new Ventas { ItemVendido = productoAniadir, Cantidad = 1 };
            Messenger.Default.Publish(new NuevoProductoCarritoMessage { VentaDTO = ItemVender });
        }
        public async Task SeleccionarBusquedaPrevia(object EntradaElegida)
        {
            if(EntradaElegida is string termino)
            {
                TextoBusqueda = termino;
                BusquedaAvanzadaActiva = false;
                await EjecutarBusqueda();
            }
        }
        public void BusquedaPierdeFoco(object parameter)
        {
            HistorialVisible = false;
        }
        public void BusquedaRecibeFoco(object parameter)
        {
            if(BusquedasRecientes.Count > 0 && !BusquedaAvanzadaActiva)
                HistorialVisible = true;
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
            await CargarCategoriasAsync();
            await CargarMarcasAsync();
            await CargarUbicacionesAsync();
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
                    Notificacion _notificacion = new Notificacion { Mensaje = "Item Duplicado!", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                }
            }
            Messenger.Default.Publish(new CerrarVistaAniadirProductoMensaje());
        }
        private async Task EjecutarBusqueda()
        {
            _servicioSFX.Paginacion();
            this.Procesando = true;
            if (BusquedaAvanzadaActiva)
            {
                await RealizarBusquedaAvanzada();
                BusquedaAvanzadaActiva = false;
            }
            else
            {
                if (TextoBusqueda != null)
                {
                    int lenCadena = TextoBusqueda.Length;
                    if (decimal.TryParse(TextoBusqueda.Substring(0, lenCadena - 1), out decimal eanBuscado) && (lenCadena == 10 || lenCadena == 13))
                    {
                        await BuscarEAN(TextoBusqueda);
                    }
                    else
                    {
                        await BuscarProductosTitulos(TextoBusqueda);
                    }

                    // Persistir busquedas recientes
                    if (!string.IsNullOrEmpty(TextoBusqueda) && !BusquedasRecientes.Contains(TextoBusqueda))
                    {
                        BusquedasRecientes.Insert(0, TextoBusqueda);
                        if (BusquedasRecientes.Count > 7)
                        {
                            BusquedasRecientes.RemoveAt(BusquedasRecientes.Count - 1);
                        }
                    }
                }
            }

            string cuerpoNotificacion = string.Empty;
            string IconoAUtilizar = string.Empty;
            if (ColeccionProductos.Count < 1)
            {
                cuerpoNotificacion = "No se hallaron resultados para la busqueda";
                IconoAUtilizar = Path.GetFullPath(IconoNotificacion.SUSPENSO1);
                TituloVista = "Sin coincidencias...";
            }
            else
            {   
                string verbo = ColeccionProductos.Count > 1 ? "hallaron" : "hallo";
                string palabra = ColeccionProductos.Count > 1 ? "coincidencias" : "coincidencia";
                cuerpoNotificacion = $"Se {verbo} {ColeccionProductos.Count} {palabra}!";
                IconoAUtilizar = Path.GetFullPath(IconoNotificacion.OK);
                TituloVista = "Coincidencias";
            }

            MostrarBotonRegresar = true;
            Notificacion _notificacion = new Notificacion { Mensaje = cuerpoNotificacion, Titulo = TituloVista, IconoRuta = IconoAUtilizar, Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            this.Procesando = false;
        }
        private async Task BuscarEAN(string EanBuscado)
        {
            ResultadosBusquedaEAN registros = await Task.Run(() => OrquestadorProductos.BuscarProductoEAN(EanBuscado));

            App.Current.Dispatcher.Invoke(() =>
            {
                ColeccionProductos.Clear();
                foreach (var producto in registros.Productos)
                {
                    ProductoBase item = new ProductoBase
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
                    ColeccionProductos.Add(item);
                }

                if(registros.HayVersionesObsoletas)
                {
                    Notificacion _notificacion = new Notificacion { Mensaje = "Algunos resultados incluyen codigos de barra desactualizados", Titulo = TituloVista, IconoRuta = IconoNotificacion.NOTIFICACION, Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                }
            });
        }
        private async Task RealizarBusquedaAvanzada()
        {
            List<Propiedad_Valor> parametrosBusqueda = new List<Propiedad_Valor>();

            if (CategoriaBuscada != null)
            {
                Propiedad_Valor categoriaBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "Categoria",
                    Valor = CategoriaBuscada.ID
                };
                parametrosBusqueda.Add(categoriaBuscar);
            }

            if (UbicacionBuscada != null)
            {
                Propiedad_Valor ubicacionBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "UbicacionID",
                    Valor = UbicacionBuscada.ID
                };
                parametrosBusqueda.Add(ubicacionBuscar);
            }

            if (MarcaBuscada != null)
            {
                Propiedad_Valor marcaBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "MarcaID",
                    Valor = MarcaBuscada.ID
                };
                parametrosBusqueda.Add(marcaBuscar);
            }

            if (ToggleItemsVisibilidadWeb != null)
            {
                Propiedad_Valor visWebBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "VisibilidadWeb",
                    Valor = ToggleItemsVisibilidadWeb == true ? "1" : "0"
                };
                parametrosBusqueda.Add(visWebBuscar);
            }

            if (ToggleItemsExistentes != null)
            {
                Propiedad_Valor itemsExistentesBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "Haber",
                    Valor = ToggleItemsExistentes == true ? "1" : "0"
                };
                parametrosBusqueda.Add(itemsExistentesBuscar);
            }

            if (ToggleItemsPrecioPublico != null)
            {
                Propiedad_Valor precioPublicoBuscar = new Propiedad_Valor
                {
                    PropiedadNombre = "PrecioPublico",
                    Valor = ToggleItemsPrecioPublico == true ? "1" : "0"
                };
                parametrosBusqueda.Add(precioPublicoBuscar);
            }

            List<ProductoCatalogo> coincidencias = _productoServicio.RecuperarLotePorPropiedades(parametrosBusqueda);

            ColeccionProductos.Clear();
            foreach (var producto in coincidencias)
            {
                ProductoBase item = new ProductoBase
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
                ColeccionProductos.Add(item);
            }
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
            ReiniciarParametrosBusqueda(null);
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
        private async Task CargarCategoriasAsync()
        {
            await foreach(var categoria in servicioCategorias.RecuperarStreamAsync())
            {
                Categorias registro = new Categorias
                {
                    Nombre = categoria.Nombre,
                    ID = categoria.ID,
                    FechaCreacion = categoria.FechaCreacion,
                    FechaModificacion = categoria.FechaModificacion,
                    EsEliminado = categoria.EsEliminado
                };
                ColeccionCategorias.Add(registro);
            }
        }
        private async Task CargarMarcasAsync()
        {
            await foreach (var marca in servicioMarcas.RecuperarStreamAsync())
            {
                Marcas registro = new Marcas
                {
                    Nombre = marca.Nombre,
                    ID = marca.ID,
                    FechaCreacion = marca.FechaCreacion,
                    FechaModificacion = marca.FechaModificacion,
                    EsEliminado = marca.EsEliminado
                };
                ColeccionMarcas.Add(registro);
            }
        }
        private async Task CargarUbicacionesAsync()
        {
            await foreach (var ubicacion in servicioUbicaciones.RecuperarStreamAsync())
            {
                Ubicaciones registro = new Ubicaciones
                {
                    Nombre = ubicacion.Nombre,
                    ID = ubicacion.ID,
                    FechaCreacion = ubicacion.FechaCreacion,
                    FechaModificacion = ubicacion.FechaModificacion,
                    EsEliminado = ubicacion.EsEliminado
                };
                ColeccionUbicaciones.Add(registro);
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
        private void OnProductoEliminado(ProductoEliminadoMessage Mensaje)
        {
            if(Mensaje.ProductoEliminado != null)
            {
                ProductoBase _registro = ColeccionProductos.FirstOrDefault(p => p.ProductoSKU == Mensaje.ProductoEliminado.ProductoSKU);
                ColeccionProductos.Remove(_registro);
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
                ProductoBase productoAEditar = ColeccionProductos.FirstOrDefault(p => p.ProductoSKU == Mensaje.ProductoModificado.ProductoSKU);
                if (productoAEditar != null)
                {
                    productoAEditar.Nombre = Mensaje.ProductoModificado.Nombre;
                    productoAEditar.Precio= Mensaje.ProductoModificado.Precio;
                    productoAEditar.Categoria= Mensaje.ProductoModificado.CategoriaNombre;
                    productoAEditar.RutaImagen= string.IsNullOrWhiteSpace(productoAEditar.RutaImagen) ? string.Empty : System.IO.Path.GetFullPath(Mensaje.ProductoModificado.RutaImagen);
                    productoAEditar.FechaModificacion = Mensaje.ProductoModificado.FechaModificacion;
                    productoAEditar.FechaCreacion = Mensaje.ProductoModificado.FechaCreacion;
                }  
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
