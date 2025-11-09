using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public enum LadoMasLargo
    {
        Ancho,
        Alto
    }
    public class AbrirVistaAniadirProductoMensaje { }
    public class CerrarVistaAniadirProductoMensaje { }
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        private readonly IConmutadorEntidadGenerica<Formatos> servicioFormatos;
        private readonly IConmutadorEntidadGenerica<Condiciones> servicioCondiciones;
        private readonly IConmutadorEntidadGenerica<Marcas> servicioMarcas;
        private readonly IConmutadorEntidadGenerica<Categorias> servicioCategorias;
        private readonly IConmutadorEntidadGenerica<Ubicaciones> servicioUbicaciones;
        private readonly OrquestadorProductos OrquestadorProductos;
        public ObservableCollection<Formatos> Formatos { get; } = new();
        public ObservableCollection<Marcas> Marcas { get; } = new();
        public ObservableCollection<Condiciones> Condiciones { get; } = new();
        public ObservableCollection<Categorias> Categorias { get; } = new();
        public ObservableCollection<Ubicaciones> Ubicaciones { get; } = new();
        public bool EsModoEdicion { get; set; }
        public string NombreDeVentana { get; set; }
        private int CalculoAlturaMarco;
        private int CalculoAnchoMarco;
        private bool _Conflicto;
        public bool Conflicto
        {
            get { return _Conflicto; }
            set
            {
                if (_Conflicto != value)
                {
                    _Conflicto = value;
                    OnPropertyChanged(nameof(Conflicto));
                }
            }
        }
        private bool _BotonHabilitado;
        public bool BotonHabilitado
        {
            get { return _BotonHabilitado; }
            set
            {
                if (_BotonHabilitado != value)
                {
                    _BotonHabilitado = value;
                    OnPropertyChanged(nameof(BotonHabilitado));
                }
            }
        }
        private bool _ToggleEdicionMarca;
        public bool ToggleEdicionMarca
        {
            get { return _ToggleEdicionMarca; }
            set
            {
                if (_ToggleEdicionMarca != value)
                {
                    _ToggleEdicionMarca = value;
                    OnPropertyChanged(nameof(ToggleEdicionMarca));
                }
            }
        }
        private bool _ToggleSeleccionMarca;
        public bool ToggleSeleccionMarca
        {
            get { return _ToggleSeleccionMarca; }
            set
            {
                if (_ToggleSeleccionMarca != value)
                {
                    _ToggleSeleccionMarca = value;
                    OnPropertyChanged(nameof(ToggleSeleccionMarca));
                }
            }
        }
        private bool _ToggleEdicionFormato;
        public bool ToggleEdicionFormato
        {
            get { return _ToggleEdicionFormato; }
            set
            {
                if (_ToggleEdicionFormato != value)
                {
                    _ToggleEdicionFormato = value;
                    OnPropertyChanged(nameof(ToggleEdicionFormato));
                }
            }
        }
        private bool _ToggleSeleccionFormato;
        public bool ToggleSeleccionFormato
        {
            get { return _ToggleSeleccionFormato; }
            set
            {
                if (_ToggleSeleccionFormato != value)
                {
                    _ToggleSeleccionFormato = value;
                    OnPropertyChanged(nameof(ToggleSeleccionFormato));
                }
            }
        }
        private bool _ToggleEdicionCategoria;
        public bool ToggleEdicionCategoria
        {
            get { return _ToggleEdicionCategoria; }
            set
            {
                if (_ToggleEdicionCategoria != value)
                {
                    _ToggleEdicionCategoria = value;
                    OnPropertyChanged(nameof(ToggleEdicionCategoria));
                }
            }
        }
        private bool _ToggleSeleccionCategoria;
        public bool ToggleSeleccionCategoria
        {
            get { return _ToggleSeleccionCategoria; }
            set
            {
                if (_ToggleSeleccionCategoria != value)
                {
                    _ToggleSeleccionCategoria = value;
                    OnPropertyChanged(nameof(ToggleSeleccionCategoria));
                }
            }
        }
        private bool _ToggleEdicionUbicacion;
        public bool ToggleEdicionUbicacion
        {
            get { return _ToggleEdicionUbicacion; }
            set
            {
                if (_ToggleEdicionUbicacion != value)
                {
                    _ToggleEdicionUbicacion = value;
                    OnPropertyChanged(nameof(ToggleEdicionUbicacion));
                }
            }
        }
        private bool _ToggleSeleccionUbicacion;
        public bool ToggleSeleccionUbicacion
        {
            get { return _ToggleSeleccionUbicacion; }
            set
            {
                if (_ToggleSeleccionUbicacion != value)
                {
                    _ToggleSeleccionUbicacion = value;
                    OnPropertyChanged(nameof(ToggleSeleccionUbicacion));
                }
            }
        }
        private bool _ToggleEdicionCondicion;
        public bool ToggleEdicionCondicion
        {
            get { return _ToggleEdicionCondicion; }
            set
            {
                if (_ToggleEdicionCondicion != value)
                {
                    _ToggleEdicionCondicion = value;
                    OnPropertyChanged(nameof(ToggleEdicionCondicion));
                }
            }
        }
        private bool _ToggleSeleccionCondicion;
        public bool ToggleSeleccionCondicion
        {
            get { return _ToggleSeleccionCondicion; }
            set
            {
                if (_ToggleSeleccionCondicion != value)
                {
                    _ToggleSeleccionCondicion = value;
                    OnPropertyChanged(nameof(ToggleSeleccionCondicion));
                }
            }
        }
        private bool _ToggleColapsarSeccionEnvios;
        public bool ToggleColapsarSeccionEnvios
        {
            get { return _ToggleColapsarSeccionEnvios; }
            set
            {
                if (_ToggleColapsarSeccionEnvios != value)
                {
                    _ToggleColapsarSeccionEnvios = value;
                    OnPropertyChanged(nameof(ToggleColapsarSeccionEnvios));
                }
            }
        }
        private int _altoImagenSeleccionada;
        public int AltoImagenSeleccionada 
        {
            get { return _altoImagenSeleccionada; }
            set
            {
                if (_altoImagenSeleccionada != value)
                {
                    _altoImagenSeleccionada = value;
                    OnPropertyChanged(nameof(AltoImagenSeleccionada));
                }
            }
        }
        private int _anchoImagenSeleccionada;
        public int AnchoImagenSeleccionada
        {
            get { return _anchoImagenSeleccionada; }
            set
            {
                if (_anchoImagenSeleccionada != value)
                {
                    _anchoImagenSeleccionada = value;
                    OnPropertyChanged(nameof(AnchoImagenSeleccionada));
                }
            }
        }
        private string _iconoEdicion;
        public string iconoEdicion
        {
            get { return _iconoEdicion; }
            set
            {
                if (_iconoEdicion != value)
                {
                    _iconoEdicion = value;
                    OnPropertyChanged(nameof(iconoEdicion));
                }
            }
        }
        private string _iconoEdicionFormato;
        public string iconoEdicionFormato
        {
            get { return _iconoEdicionFormato; }
            set
            {
                if (_iconoEdicionFormato != value)
                {
                    _iconoEdicionFormato = value;
                    OnPropertyChanged(nameof(iconoEdicionFormato));
                }
            }
        }
        private string _iconoEdicionCategoria;
        public string iconoEdicionCategoria
        {
            get { return _iconoEdicionCategoria; }
            set
            {
                if (_iconoEdicionCategoria != value)
                {
                    _iconoEdicionCategoria = value;
                    OnPropertyChanged(nameof(iconoEdicionCategoria));
                }
            }
        }
        private string _iconoEdicionUbicacion;
        public string iconoEdicionUbicacion
        {
            get { return _iconoEdicionUbicacion; }
            set
            {
                if (_iconoEdicionUbicacion != value)
                {
                    _iconoEdicionUbicacion = value;
                    OnPropertyChanged(nameof(iconoEdicionUbicacion));
                }
            }
        }
        private string _iconoToggleSeccionEnvios;
        public string iconoToggleSeccionEnvios
        {
            get { return _iconoToggleSeccionEnvios; }
            set
            {
                if (_iconoToggleSeccionEnvios != value)
                {
                    _iconoToggleSeccionEnvios = value;
                    OnPropertyChanged(nameof(iconoToggleSeccionEnvios));
                }
            }
        }
        private string _leyendaBotonImagen;
        public string leyendaBotonImagen
        {
            get { return _leyendaBotonImagen; }
            set
            {
                if (_leyendaBotonImagen != value)
                {
                    _leyendaBotonImagen = value;
                    OnPropertyChanged(nameof(leyendaBotonImagen));
                }
            }
        }
        private string _iconoSeleccionRapida;
        public string iconoSeleccionRapida
        {
            get { return _iconoSeleccionRapida; }
            set
            {
                if (_iconoSeleccionRapida != value)
                {
                    _iconoSeleccionRapida = value;
                    OnPropertyChanged(nameof(iconoSeleccionRapida));
                }
            }
        }
        private string _iconoEdicionCondicion;
        public string iconoEdicionCondicion
        {
            get { return _iconoEdicionCondicion; }
            set
            {
                if (_iconoEdicionCondicion != value)
                {
                    _iconoEdicionCondicion = value;
                    OnPropertyChanged(nameof(iconoEdicionCondicion));
                }
            }
        }


        //---- Propiedades de registro -----
        private string _nombreProducto;
        public string NombreProducto
        {
            get { return _nombreProducto; }
            set
            {
                if (_nombreProducto != value)
                {
                    _nombreProducto = value;
                    OnPropertyChanged(nameof(NombreProducto));
                    ValidarBotonSubmit();
                }
            }
        }
        private string _rutaImagenSeleccionada;
        public string RutaImagenSeleccionada
        {
            get { return _rutaImagenSeleccionada; }
            set
            {
                if (_rutaImagenSeleccionada != value)
                {
                    _rutaImagenSeleccionada = value;
                    OnPropertyChanged(nameof(RutaImagenSeleccionada));
                    CargarDimensionesImagen(value);
                }
            }
        }
        private DateTime _fechaCreacionProducto;
        public DateTime FechaCreacionProducto
        {
            get { return _fechaCreacionProducto; }
            set
            {
                if (_fechaCreacionProducto != value)
                {
                    _fechaCreacionProducto = value;
                    OnPropertyChanged(nameof(FechaCreacionProducto));
                }
            }
        }
        private DateTime _fechaModificacionProducto;
        public DateTime FechaModificacionProducto
        {
            get { return _fechaModificacionProducto; }
            set
            {
                if (_fechaModificacionProducto != value)
                {
                    _fechaModificacionProducto = value;
                    OnPropertyChanged(nameof(FechaModificacionProducto));
                }
            }
        }


        //CADENA
        private string _ean;
        public string EAN
        {
            get { return _ean; }
            set
            {
                if (_ean != value)
                {
                    _ean = value;
                    OnPropertyChanged(nameof(EAN));
                }
            }
        }
        private string _formatoNombre;
        public string FormatoNombre
        {
            get { return _formatoNombre; }
            set
            {
                if (_formatoNombre != value)
                {
                    _formatoNombre = value;
                    OnPropertyChanged(nameof(FormatoNombre));
                    ValidarBotonSubmit();
                }
            }
        }
        private string _marcaNombre;
        public string MarcaNombre
        {
            get { return _marcaNombre; }
            set
            {
                if (_marcaNombre != value)
                {
                    _marcaNombre = value;
                    OnPropertyChanged(nameof(MarcaNombre));
                    ValidarBotonSubmit();
                }
            }
        }
        private string _categoriaNombre;
        public string CategoriaNombre
        {
            get { return _categoriaNombre; }
            set
            {
                if (_categoriaNombre != value)
                {
                    _categoriaNombre = value;
                    OnPropertyChanged(nameof(CategoriaNombre));
                    ValidarBotonSubmit();
                }
            }
        }
        private string _ubicacionNombre;
        public string UbicacionNombre
        {
            get { return _ubicacionNombre; }
            set
            {
                if (_ubicacionNombre != value)
                {
                    _ubicacionNombre = value;
                    OnPropertyChanged(nameof(UbicacionNombre));
                    ValidarBotonSubmit();
                }
            }
        }
        private string _condicionNombre;
        public string CondicionNombre
        {
            get { return _condicionNombre; }
            set
            {
                if (_condicionNombre != value)
                {
                    _condicionNombre = value;
                    OnPropertyChanged(nameof(CondicionNombre));
                    ValidarBotonSubmit();
                }
            }
        }


        //NUMERICAS
        private decimal _precioProducto;
        public decimal PrecioProducto
        {
            get { return _precioProducto; }
            set
            {
                if (_precioProducto != value)
                {
                    _precioProducto = value;
                    OnPropertyChanged(nameof(PrecioProducto));
                }
            }
        }
        private int _cantidadEnStock;
        public int CantidadEnStock
        {
            get { return _cantidadEnStock; }
            set
            {
                if (_cantidadEnStock != value)
                {
                    _cantidadEnStock = value;
                    OnPropertyChanged(nameof(CantidadEnStock));
                    ValidarBotonSubmit();
                }
            }
        }
        private decimal _alto;
        public decimal Alto
        {
            get { return _alto; }
            set
            {
                if (_alto != value)
                {
                    _alto = value;
                    OnPropertyChanged(nameof(Alto));
                }
            }
        }
        private decimal _profundidad;
        public decimal Profundidad
        {
            get { return _profundidad; }
            set
            {
                if (_profundidad != value)
                {
                    _profundidad = value;
                    OnPropertyChanged(nameof(Profundidad));
                }
            }
        }
        private decimal _largo;
        public decimal Largo
        {
            get { return _largo; }
            set
            {
                if (_largo != value)
                {
                    _largo = value;
                    OnPropertyChanged(nameof(Largo));
                }
            }
        }
        private decimal _peso;
        public decimal Peso
        {
            get { return _peso; }
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged(nameof(Peso));
                }
            }
        }


        //BOOLEANAS
        private bool _visibilidadWeb;
        public bool VisibilidadWeb
        {
            get { return _visibilidadWeb; }
            set
            {
                if (_visibilidadWeb != value)
                {
                    _visibilidadWeb = value;
                    OnPropertyChanged(nameof(VisibilidadWeb));
                }
            }
        }
        private bool _precioPublico;
        public bool PrecioPublico
        {
            get { return _precioPublico; }
            set
            {
                if (_precioPublico != value)
                {
                    _precioPublico = value;
                    OnPropertyChanged(nameof(PrecioPublico));
                }
            }
        }
        private bool _esProductoEliminado;
        public bool EsProductoEliminado
        {
            get { return _esProductoEliminado; }
            set
            {
                if (_esProductoEliminado != value)
                {
                    _esProductoEliminado = value;
                    OnPropertyChanged(nameof(EsProductoEliminado));
                }
            }
        }


        //CLAVES
        private string _marcaProductoID;
        public string MarcaProductoID
        {
            get { return _marcaProductoID; }
            set
            {
                if (_marcaProductoID != value)
                {
                    Marcas _registro = Marcas.FirstOrDefault(m => m.ID == value);
                    MarcaNombre = _registro.Nombre;
                    _marcaProductoID = value;
                    OnPropertyChanged(nameof(MarcaProductoID));
                }
            }
        }
        private string _formato;
        public string FormatoID
        {
            get { return _formato; }
            set
            {
                if (_formato != value)
                {
                    _formato = value;
                    Formatos _registro = Formatos.FirstOrDefault(f => f.ID == value);
                    Alto = _registro.Alto;
                    Largo = _registro.Largo;
                    Profundidad = _registro.Profundidad;
                    Peso = _registro.Peso;
                    FormatoNombre = _registro.Nombre;

                    OnPropertyChanged(nameof(FormatoID));
                }
            }
        }
        private string _productoVersionID;
        public string ProductoVersionID
        {
            get { return _productoVersionID; }
            set
            {
                if (_productoVersionID != value)
                {
                    _productoVersionID = value;
                    OnPropertyChanged(nameof(ProductoVersionID));
                }
            }
        }
        private string _ubicacionID;
        public string UbicacionID
        {
            get { return _ubicacionID; }
            set
            {
                if (_ubicacionID != value)
                {
                    Ubicaciones _registro = Ubicaciones.FirstOrDefault(u => u.ID == value);
                    UbicacionNombre = _registro.Nombre;
                    _ubicacionID = value;
                    OnPropertyChanged(nameof(UbicacionID));
                }
            }
        }
        private string _categoriaProductoID;
        public string CategoriaProductoID
        {
            get { return _categoriaProductoID; }
            set
            {
                if (_categoriaProductoID != value)
                {
                    Categorias _registro = Categorias.FirstOrDefault(f => f.ID == value);
                    CategoriaNombre = _registro.Nombre;
                    _categoriaProductoID = value;
                    OnPropertyChanged(nameof(CategoriaProductoID));
                }
            }
        }
        private string _condicionID;
        public string CondicionID
        {
            get { return _condicionID; }
            set
            {
                if (_condicionID != value)
                {
                    Condiciones _registro = Condiciones.FirstOrDefault(c => c.ID == value);
                    CondicionNombre = _registro.Nombre;
                    _condicionID = value;
                    OnPropertyChanged(nameof(CondicionID));
                }
            }
        }
        private string _productoID;
        public string ProductoID
        {
            get { return _productoID; }
            set
            {
                if (_productoID != value)
                {
                    _productoID = value;
                    OnPropertyChanged(nameof(ProductoID));
                }
            }
        }
        public string ProductoSKU { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CierreSolicitado;
        public ICommand ModificarMarcaCommand{ get; }
        public ICommand ElegirImagenCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand BotonPresionadoCommand { get; }
        public ICommand CerrarVistaCommand { get; }
        public ICommand ModificarCantidadStockCommand { get; }
        public ICommand ModificarFormatoCommand { get; }
        public ICommand ModificarCategoriaCommand { get; }
        public ICommand ModificarUbicacionCommand { get; }
        public ICommand ModificarCondicionCommand { get; }
        public ICommand ColapsarSeccionEnviosCommand { get; }
        public ICommand SeleccionRapidaFormatosCommand { get; }
        public ICommand SeleccionRapidaMarcasCommand { get; }
        public ICommand SeleccionRapidaCategoriasCommand { get; }
        public ICommand SeleccionRapidaUbicacionesCommand { get; }
        public ICommand SeleccionRapidaCondicionesCommand { get; }

        public AniadirProductoViewModel(OrquestadorProductos _orquestador, IConmutadorEntidadGenerica<Formatos> _servicioFormatos,
            IConmutadorEntidadGenerica<Marcas> _servicioMarcas, IConmutadorEntidadGenerica<Categorias> _servicioCategorias,
            IConmutadorEntidadGenerica<Ubicaciones> _servicioUbicaciones, IConmutadorEntidadGenerica<Condiciones> _servicioCondiciones)
        {
            //Imagen
            RutaImagenSeleccionada = string.Empty;
            AnchoImagenSeleccionada = 0;
            AltoImagenSeleccionada = 0;
            CalculoAlturaMarco = 0;
            CalculoAnchoMarco = 0;
            ProductoSKU = null;

            //Entidad
            NombreProducto = string.Empty;
            CategoriaNombre = string.Empty;
            PrecioProducto = 0;
            VisibilidadWeb = false;
            PrecioPublico = false;
            CantidadEnStock = 0;
            EAN = "";

            NombreDeVentana = "Añadir Producto";
            _ToggleEdicionMarca = false;
            _ToggleSeleccionMarca = true;
            _ToggleEdicionFormato = false;
            _ToggleSeleccionFormato = true;
            _ToggleEdicionCategoria = false;
            _ToggleSeleccionCategoria = true;
            _ToggleEdicionUbicacion = false;
            _ToggleSeleccionUbicacion = true;
            _ToggleColapsarSeccionEnvios = false;
            _ToggleEdicionCondicion = false;
            _ToggleSeleccionCondicion = true;

            _iconoEdicion = "/iconos/lapizEdicion.png";
            _iconoEdicionFormato = "/iconos/lapizEdicion.png";
            _iconoEdicionCategoria = "/iconos/lapizEdicion.png";
            _iconoEdicionUbicacion = "/iconos/lapizEdicion.png";
            _iconoToggleSeccionEnvios = "/iconos/abajo1.png";
            _iconoSeleccionRapida = "/iconos/lista1.png";
            _iconoEdicionCondicion = "/iconos/lapizEdicion.png";

            _leyendaBotonImagen = "Añadir imagen";

            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
            BotonPresionadoCommand = new RelayCommand<object>(BotonPresionado);
            ModificarCantidadStockCommand = new RelayCommand<object>(ModificarCantidadStock);
            ModificarMarcaCommand = new RelayCommand<object>(ModificarMarca);
            ModificarFormatoCommand = new RelayCommand<object>(ModificarFormato);
            ModificarCategoriaCommand = new RelayCommand<object>(ModificarCategoria);
            ModificarUbicacionCommand = new RelayCommand<object>(ModificarUbicacion);
            ModificarCondicionCommand = new RelayCommand<object>(ModificarCondicion);
            ColapsarSeccionEnviosCommand = new RelayCommand<object> (ToggleSeccionEnvios);
            SeleccionRapidaUbicacionesCommand = new RelayCommand<object>(SeleccionRapida);

            Messenger.Default.Subscribir<VistaAniadirProductosCantidadModificada>(OnCantidadModificada);

            //Servicios
            OrquestadorProductos = _orquestador;
            servicioFormatos = _servicioFormatos;
            servicioMarcas = _servicioMarcas;
            servicioCategorias = _servicioCategorias;
            servicioUbicaciones = _servicioUbicaciones;
            servicioCondiciones = _servicioCondiciones;

            Conflicto = true;
            BotonHabilitado = false;
            EsModoEdicion = false;
        }

        public void BotonPresionado(object parameter)
        {
            if(BotonHabilitado)
            {
                if (EsModoEdicion)
                {
                    EditarProducto(0);
                }
                else
                {
                    AniadirProducto(0);
                }
            }
            else
            {
                // El proposito es forzar la animación
                Conflicto = false;
                Conflicto = true;
                Notificacion _notificacion = new Notificacion { Mensaje = "Debe completar todos los campos requeridos", Titulo = "Operación Interrumpida", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        private async Task CargarFormatos() { 
            await foreach (var formato in servicioFormatos.RecuperarStreamAsync()) 
            { 
                Formatos.Add(formato); 
            } 
        }
        private async Task CargarCondiciones()
        {
            await foreach (var condicion in servicioCondiciones.RecuperarStreamAsync())
            {
                Condiciones.Add(condicion);
            }
        }
        private async Task CargarMarcas()
        {
            await foreach (var marca in servicioMarcas.RecuperarStreamAsync())
            {
                Marcas.Add(marca);
            }
        }
        private async Task CargarCategorias()
        {
            await foreach (var categoria in servicioCategorias.RecuperarStreamAsync())
            {
                Categorias.Add(categoria);
            }
        }
        private async Task CargarUbicaciones()
        {
            await foreach (var ubicacion in servicioUbicaciones.RecuperarStreamAsync())
            {
                Ubicaciones.Add(ubicacion);
            }
        }
        public async Task InicializarFormulario()
        {
            // Poblar colecciones para seleccion asistida
            await CargarFormatos();
            await CargarMarcas();
            await CargarCategorias();
            await CargarUbicaciones();
            await CargarCondiciones();
        }
        public async Task ConfigurarEdicionDeProducto(ProductoCatalogo Producto)
        {
            // Configurar Bindings
            EsModoEdicion = true;
            NombreDeVentana = "Editar Producto";
            leyendaBotonImagen = "Cambiar imagen";

            //--PK--
            ProductoSKU = Producto.ProductoSKU;

            //Cadena
            RutaImagenSeleccionada = Producto.RutaImagen;
            NombreProducto = Producto.Nombre;
            EAN = Producto.EAN;
            CategoriaNombre = Producto.CategoriaNombre;
            MarcaNombre = Producto.MarcaNombre;
            CondicionNombre = Producto.CondicionNombre;

            //Fecha
            FechaCreacionProducto = Producto.FechaCreacion;
            FechaModificacionProducto = Producto.FechaModificacion;

            //Claves
            CategoriaProductoID = Producto.Categoria;
            ProductoVersionID = Producto.ProductoVersionID;
            MarcaProductoID = Producto.MarcaID;
            UbicacionID = Producto.UbicacionID;
            FormatoID = Producto.FormatoProductoID;
            ProductoID = Producto.ID;
            CondicionID = Producto.CondicionID;

            //Booleanas
            VisibilidadWeb = Producto.VisibilidadWeb;
            PrecioPublico = Producto.PrecioPublico;
            EsProductoEliminado = Producto.EsEliminado;

            //Numericas
            CantidadEnStock = Producto.Haber;
            PrecioProducto = Producto.Precio;
            Alto = Producto.Alto;
            Peso = Producto.Peso;
            Profundidad = Producto.Profundidad;
            Largo = Producto.Largo;

            // Obtener dimensiones de imagen
            CargarDimensionesImagen(RutaImagenSeleccionada);
            LadoMasLargo lado = new LadoMasLargo();
            int UBound = 0;
            int LBound = 0;
            int TamanioMaximo = 200;

            // Calcular relación de aspecto del marco
            if (CalculoAlturaMarco > CalculoAnchoMarco)
            {
                UBound = CalculoAlturaMarco;
                LBound = CalculoAnchoMarco;
                lado = LadoMasLargo.Alto;
            }
            else
            {
                UBound = CalculoAnchoMarco;
                LBound = CalculoAlturaMarco;
                lado = LadoMasLargo.Ancho;
            }

            // Aplicar reducción
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = UBound / (float)LBound;
                switch (lado)
                {
                    case LadoMasLargo.Alto:
                        AltoImagenSeleccionada = TamanioMaximo;
                        AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                    case LadoMasLargo.Ancho:
                        AnchoImagenSeleccionada = TamanioMaximo;
                        AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                }
            }

            Conflicto = false;
            BotonHabilitado = true;
        }
        public void EditarProducto(object parameter)
        {
            if(RutaImagenSeleccionada != string.Empty)
            {
                //comprobar si la imagen elegida existe en la carpeta de miniaturas
                string NombreArchivo = Path.GetFileNameWithoutExtension(RutaImagenSeleccionada);
                string Extension = Path.GetExtension(RutaImagenSeleccionada);
                string Destino = ".\\datos\\miniaturas\\" + NombreArchivo + Extension;
                bool SalirDelBucle = false;
                if (!File.Exists(Destino))
                {
                    try
                    {
                        File.Copy(RutaImagenSeleccionada, Destino, false);
                        RutaImagenSeleccionada = "./datos/miniaturas/" + NombreArchivo + Extension;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    RutaImagenSeleccionada = "./datos/miniaturas/" + NombreArchivo + Extension;
                }
            }

            // Solicitar cambio a servicio de Productos y mostrar resultado
            ProductoCatalogo ProductoModificado = new ProductoCatalogo {
                ID = ProductoID,
                UbicacionID = UbicacionID,
                ProductoSKU = ProductoSKU,
                FormatoProductoID = FormatoID,
                Categoria = CategoriaProductoID,
                ProductoVersionID = ProductoVersionID,
                MarcaID = MarcaProductoID,
                CondicionID = CondicionID,

                Nombre = NombreProducto,
                RutaImagen = string.IsNullOrWhiteSpace(RutaImagenSeleccionada) ? string.Empty : Path.GetFullPath(RutaImagenSeleccionada),
                CategoriaNombre = CategoriaNombre,
                UbicacionNombre = UbicacionNombre,
                FormatoNombre = FormatoNombre,
                MarcaNombre = MarcaNombre,
                CondicionNombre = CondicionNombre,
                EAN = EAN,

                VisibilidadWeb = VisibilidadWeb,
                PrecioPublico = PrecioPublico,
                EsEliminado = EsProductoEliminado,

                FechaCreacion = FechaCreacionProducto,
                FechaModificacion = FechaModificacionProducto,

                Precio = PrecioProducto,
                Haber = CantidadEnStock,
                Alto = Alto,
                Profundidad = Profundidad,
                Largo = Largo,
                Peso = Peso

            };

            if (OrquestadorProductos.ModificarProducto(ProductoModificado))
            {
                ProductoModificado.FechaModificacion = DateTime.Now;
                ProductoModificado.Categoria = ProductoModificado.CategoriaNombre;
                //ProductoModificado.RutaImagen = string.IsNullOrWhiteSpace(RutaImagenSeleccionada) ? string.Empty : Path.GetFullPath(RutaImagenSeleccionada);
                Messenger.Default.Publish(new ProductoModificadoMensaje { ProductoModificado = ProductoModificado });
                CerrarVistaCommand.Execute(0);
                Notificacion _notificacion = new Notificacion { Mensaje = "Item editado exitosamente", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                CerrarVistaCommand.Execute(0);
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo editar el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        public void ElegirImagen(object parameter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Todos los archivos (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Title = "Seleccionar imagen";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            bool? resultado = openFileDialog.ShowDialog();

            if (resultado == true)
            {
                int UBound = 0;
                int LBound = 0;
                int TamanioMaximo = 200;
                string rutaArchivo = openFileDialog.FileName;
                RutaImagenSeleccionada = rutaArchivo;
                LadoMasLargo lado = new LadoMasLargo();
                
                // Calcular relación de aspecto del marco
                if (CalculoAlturaMarco > CalculoAnchoMarco)
                {
                    UBound = CalculoAlturaMarco;
                    LBound = CalculoAnchoMarco;
                    lado = LadoMasLargo.Alto;
                }
                else
                {
                    UBound = CalculoAnchoMarco;
                    LBound = CalculoAlturaMarco;
                    lado = LadoMasLargo.Ancho;
                }

                // Aplicar reducción
                if (UBound > TamanioMaximo)
                {
                    float _RelacionAspecto = UBound / (float)LBound;
                    switch (lado)
                    {
                        case LadoMasLargo.Alto:
                            AltoImagenSeleccionada = TamanioMaximo;
                            AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                            break;
                        case LadoMasLargo.Ancho:
                            AnchoImagenSeleccionada = TamanioMaximo;
                            AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                            break;
                    }
                }
            }
        }
        public void RedimensionarImagen(object parameter)
        {
            int UBound = 0;
            int LBound = 0;
            int TamanioMaximo = 200;
            LadoMasLargo lado = new LadoMasLargo();

            // Calcular relación de aspecto del marco
            if (CalculoAlturaMarco > CalculoAnchoMarco)
            {
                UBound = CalculoAlturaMarco;
                LBound = CalculoAnchoMarco;
                lado = LadoMasLargo.Alto;
            }
            else
            {
                UBound = CalculoAnchoMarco;
                LBound = CalculoAlturaMarco;
                lado = LadoMasLargo.Ancho;
            }

            // Aplicar reducción
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = UBound / (float)LBound;
                switch (lado)
                {
                    case LadoMasLargo.Alto:
                        AltoImagenSeleccionada = TamanioMaximo;
                        AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                    case LadoMasLargo.Ancho:
                        AnchoImagenSeleccionada = TamanioMaximo;
                        AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                }
            }
        }
        public void AniadirProducto(object parameter)
        {
            string RutaImagenSalida = string.Empty;
            if (RutaImagenSeleccionada != string.Empty)
            { 
                string NombreArchivo = Path.GetFileNameWithoutExtension(RutaImagenSeleccionada);
                string Extension = Path.GetExtension(RutaImagenSeleccionada);
                string Destino = ".\\datos\\miniaturas\\" + NombreArchivo + Extension;
                bool SalirDelBucle = false;
                int NumeroIntento = 0;

                try
                {
                    File.Copy(RutaImagenSeleccionada, Destino,false);
                    RutaImagenSalida = "./datos/miniaturas/" + NombreArchivo + Extension;
                    SalirDelBucle = true;
                }
                catch(Exception ex)
                {
                    while (!SalirDelBucle)
                    {
                        if (File.Exists(Destino) && NumeroIntento < 100)
                        { 
                            string DestinoPrueba = ".\\datos\\miniaturas\\" + NombreArchivo + NumeroIntento + Extension;
                            try
                            { 
                                File.Copy(RutaImagenSeleccionada, DestinoPrueba); 
                            }
                            catch (Exception ex2)
                            { 
                                if(File.Exists(DestinoPrueba))
                                {
                                    SalirDelBucle = true;
                                    RutaImagenSalida = "./datos/miniaturas/" + NombreArchivo + NumeroIntento + Extension;
                                }
                                else
                                {
                                    NumeroIntento += 1;
                                }
                            }
                        }
                        else
                        {
                            SalirDelBucle = true;
                            RutaImagenSalida = string.Empty;
                        }
                    }
                }
            }

            ProductoCatalogo _nuevoProducto = new ProductoCatalogo
            {
                ID = ProductoID,
                UbicacionID = UbicacionID,
                ProductoSKU = ProductoSKU,
                FormatoProductoID = FormatoID,
                Categoria = CategoriaProductoID,
                ProductoVersionID = ProductoVersionID,
                MarcaID = MarcaProductoID,

                Nombre = NombreProducto,
                RutaImagen = RutaImagenSalida,
                CategoriaNombre = CategoriaNombre,
                MarcaNombre = MarcaNombre,
                FormatoNombre = FormatoNombre,
                UbicacionNombre = UbicacionNombre,
                EAN = EAN,

                VisibilidadWeb = VisibilidadWeb,
                PrecioPublico = PrecioPublico,
                EsEliminado = EsProductoEliminado,

                FechaCreacion = FechaCreacionProducto,
                FechaModificacion = FechaModificacionProducto,

                Precio = PrecioProducto,
                Haber = CantidadEnStock,
                Alto = Alto,
                Profundidad = Profundidad,
                Largo = Largo,
                Peso = Peso
            };
            ProductoBase ProductoMensaje = _nuevoProducto;

            string NuevoProductoSKU = OrquestadorProductos.CrearProducto(_nuevoProducto);
            if (NuevoProductoSKU != null)
            {
                ProductoMensaje.FechaCreacion = DateTime.Now;
                ProductoMensaje.Categoria = _nuevoProducto.CategoriaNombre;
                ProductoMensaje.RutaImagen = string.IsNullOrWhiteSpace(ProductoMensaje.RutaImagen)  ? string.Empty : Path.GetFullPath(ProductoMensaje.RutaImagen); ;
                Messenger.Default.Publish(new ProductoAniadidoMensaje { NuevoProducto = ProductoMensaje });
                CerrarVistaCommand.Execute(0);
                Notificacion _notificacion = new Notificacion { Mensaje = "Item añadido con exito", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo añadir el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }

        }
        public void ModificarCantidadStock(object parameter)
        {
            string caso = parameter as string;

            if (caso == "SUMAR")
            {
                CantidadEnStock++;
            }
            else
            {
                if (CantidadEnStock > 0)
                {
                    CantidadEnStock--;
                }
            }
        }
        public void ModificarMarca(object parameter)
        {
            ToggleEdicionMarca = !ToggleEdicionMarca;
            ToggleSeleccionMarca = !ToggleSeleccionMarca;

            if(ToggleEdicionMarca)
            {
                iconoEdicion = "/iconos/seleccion1.png";
            }
            else
            {
                iconoEdicion = "/iconos/lapizEdicion.png";
            }
        }
        public void ModificarFormato(object parameter)
        {
            ToggleEdicionFormato = !ToggleEdicionFormato;
            ToggleSeleccionFormato = !ToggleSeleccionFormato;

            if (ToggleEdicionFormato)
            {
                iconoEdicionFormato = "/iconos/seleccion1.png";
            }
            else
            {
                iconoEdicionFormato = "/iconos/lapizEdicion.png";
            }
        }
        public void ModificarCategoria(object parameter)
        {
            ToggleEdicionCategoria = !ToggleEdicionCategoria;
            ToggleSeleccionCategoria = !ToggleSeleccionCategoria;

            if (ToggleEdicionCategoria)
            {
                iconoEdicionCategoria = "/iconos/seleccion1.png";
            }
            else
            {
                iconoEdicionCategoria = "/iconos/lapizEdicion.png";
            }
        }
        public void ModificarUbicacion(object parameter)
        {
            ToggleEdicionUbicacion = !ToggleEdicionUbicacion;
            ToggleSeleccionUbicacion = !ToggleSeleccionUbicacion;

            if (ToggleEdicionUbicacion)
            {
                iconoEdicionUbicacion = "/iconos/seleccion1.png";
            }
            else
            {
                iconoEdicionUbicacion = "/iconos/lapizEdicion.png";
            }
        }
        public void ModificarCondicion(object parameter)
        {
            ToggleEdicionCondicion = !ToggleEdicionCondicion;
            ToggleSeleccionCondicion = !ToggleSeleccionCondicion;

            if (ToggleEdicionCondicion)
            {
                iconoEdicionCondicion = "/iconos/seleccion1.png";
            }
            else
            {
                iconoEdicionCondicion = "/iconos/lapizEdicion.png";
            }
        }
        public void ToggleSeccionEnvios(object parameter)
        {
            ToggleColapsarSeccionEnvios = !ToggleColapsarSeccionEnvios;

            if (ToggleColapsarSeccionEnvios)
            {
                iconoToggleSeccionEnvios = "/iconos/arriba1.png";
            }
            else
            {
                iconoToggleSeccionEnvios = "/iconos/abajo1.png";
            }
        }
        public void OnCantidadModificada(VistaAniadirProductosCantidadModificada _mensaje)
        {
            // Invocar como dialogo (modal) entrada de usuario
            InputUsuarioViewModel _viewModel = new InputUsuarioViewModel("Ingrese Cantidad de Stock");
            InputUsuario _vista = new InputUsuario(_viewModel);
            _vista.ShowDialog();
            int Cantidad;

            if (int.TryParse(_viewModel.Entrada, out Cantidad))
            {
                CantidadEnStock = Cantidad;
            }
        }
        public void SeleccionRapida(object parameter)
        {
            return;
        }
        public void ValidarBotonSubmit()
        {
            ProductoCatalogo ProductoVigente = new ProductoCatalogo
            {
                ID = ProductoID,
                UbicacionID = UbicacionID,
                ProductoSKU = ProductoSKU,
                FormatoProductoID = FormatoID,
                Categoria = CategoriaProductoID,
                ProductoVersionID = ProductoVersionID,
                MarcaID = MarcaProductoID,

                Nombre = NombreProducto,
                RutaImagen = RutaImagenSeleccionada,
                CategoriaNombre = CategoriaNombre,
                UbicacionNombre = UbicacionNombre,
                FormatoNombre = FormatoNombre,
                MarcaNombre = MarcaNombre,
                EAN = EAN,

                VisibilidadWeb = VisibilidadWeb,
                PrecioPublico = PrecioPublico,
                EsEliminado = EsProductoEliminado,

                FechaCreacion = FechaCreacionProducto,
                FechaModificacion = FechaModificacionProducto,

                Precio = PrecioProducto,
                Haber = CantidadEnStock,
                Alto = Alto,
                Profundidad = Profundidad,
                Largo = Largo,
                Peso = Peso

            };

            bool tituloValido = !string.IsNullOrEmpty(ProductoVigente.Nombre);
            bool categoriaValida = !string.IsNullOrEmpty(ProductoVigente.CategoriaNombre);
            bool cantidadValida = ProductoVigente.Haber > 0;
            bool marcaValida = !string.IsNullOrEmpty(ProductoVigente.MarcaNombre);
            bool formatoValido = !string.IsNullOrEmpty(FormatoNombre);
            bool ubicacionValida = !string.IsNullOrEmpty(ProductoVigente.UbicacionNombre);

            BotonHabilitado = tituloValido && categoriaValida && cantidadValida && marcaValida && formatoValido && ubicacionValida;
            Conflicto = !BotonHabilitado;
        }
        public void CerrarVista(object parameter)
        {
            CierreSolicitado?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Obtiene y almacena las dimensiones de una imagen seleccionada. Se utilizan para reducir la imagen sin alterar su relación de aspecto.
        /// </summary>
        /// <param name="rutaArchivo">La ruta a una imagen elegida por el usuario</param>
        private void CargarDimensionesImagen(string rutaArchivo)
        {
            if (!string.IsNullOrEmpty(rutaArchivo))
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(rutaArchivo);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    CalculoAnchoMarco = bitmapImage.PixelWidth;
                    CalculoAlturaMarco = bitmapImage.PixelHeight;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    CalculoAnchoMarco = 0;
                    CalculoAlturaMarco = 0;
                }
            }
            else
            {
                CalculoAnchoMarco = 0;
                CalculoAlturaMarco = 0;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
