using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class WrapperSeleccionPropiedad
    {
        public string Display { get; set; }
        public string PropiedadNombre { get; set; }
    }
    public class PanelSecundarioEdicionLoteViewModel : IPanelContextualVM, INotifyPropertyChanged, IDisposable
    {
        private string _propiedadElegida;
        public string PropiedadElegida
        {
            get { return _propiedadElegida; }
            set
            {
                if (_propiedadElegida != value)
                {
                    _propiedadElegida = value;
                    OnPropertyChanged(nameof(PropiedadElegida));
                    ActualizarContenidoControl();
                }
            }
        }
        private object _nuevoValor;
        public object NuevoValor
        {
            get { return _nuevoValor; }
            set
            {
                if (_nuevoValor != value)
                {
                    _nuevoValor = value;
                    OnPropertyChanged(nameof(NuevoValor));
                }
            }
        }
        private int _ContadorItemsElegidos;
        public int ContadorItemsElegidos
        {
            get { return _ContadorItemsElegidos; }
            set
            {
                if (_ContadorItemsElegidos != value)
                {
                    _ContadorItemsElegidos = value;
                    OnPropertyChanged(nameof(ContadorItemsElegidos));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _MostrarListaProductos;
        public bool MostrarListaProductos
        {
            get { return _MostrarListaProductos; }
            set
            {
                if (_MostrarListaProductos != value)
                {
                    _MostrarListaProductos = value;
                    OnPropertyChanged(nameof(MostrarListaProductos));
                }
            }
        }
        private bool _MostrarOpcionesFlag;
        public bool MostrarOpcionesFlag
        {
            get { return _MostrarOpcionesFlag; }
            set
            {
                if (_MostrarOpcionesFlag != value)
                {
                    _MostrarOpcionesFlag = value;
                    OnPropertyChanged(nameof(MostrarOpcionesFlag));
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
        private IControlValidadoVM _contenidoControl;
        public IControlValidadoVM ContenidoControl
        {
            get { return _contenidoControl; }
            set
            {
                if (_contenidoControl != value)
                {
                    _contenidoControl = value;
                    OnPropertyChanged(nameof(ContenidoControl));
                }
            }
        }
        public ICommand EliminarItemCommand { get; set; }
        public ICommand ModificarItemCommand { get; set; }
        public ICommand VerListaEdicionCommand { get; }
        public ICommand MostrarOpcionesCommand { get; }
        public ICommand EliminarListaCommand {get;}
        public ICommand GuardarCambiosCommand { get; }
        public ObservableCollection<ProductoCatalogo> ColeccionProductosEditar { get; set; }
        public ObservableCollection<WrapperSeleccionPropiedad> ColeccionPropiedadesProductos { get; set; }
        private readonly ServicioSFX servicioSFX;
        private readonly OrquestadorProductos Orquestador;
        public readonly Dictionary<string, string> MapeoPropiedades;
        public PanelSecundarioEdicionLoteViewModel(ServicioSFX servicioSFX, OrquestadorProductos _orquestador)
        {
            PropiedadElegida = string.Empty;
            _MostrarListaProductos = true;
            _MostrarOpcionesFlag = false;
            _BotonHabilitado = false;
            _nuevoValor = null;
            ContenidoControl = null;
            ColeccionProductosEditar = new ObservableCollection<ProductoCatalogo>();
            ColeccionPropiedadesProductos = new ObservableCollection<WrapperSeleccionPropiedad>();

            VerListaEdicionCommand = new RelayCommand<object>(VerListaEdicion);
            MostrarOpcionesCommand = new RelayCommand<object>(MostrarOpciones);
            ModificarItemCommand = new RelayCommand<ProductoCatalogo>(ModificarItem);
            EliminarItemCommand = new RelayCommand<ProductoCatalogo>(EliminarItem);
            EliminarListaCommand = new RelayCommand<object>(EliminarLista);
            GuardarCambiosCommand = new RelayCommand<object>(GuardarCambios);
            Messenger.Default.Subscribir<NuevoProductoEdicion>(OnProductoAniadidoEdicion);
            Messenger.Default.Subscribir<ToggleHabilitarBotonEdicion>(OnValidacionBoton);
            this.servicioSFX = servicioSFX;
            this.Orquestador = _orquestador;

            MapeoPropiedades = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Display
                {"UbicacionID", "Ubicacion" },
                {"Haber" , "Haber" },
                {"Precio", "Precio" },
                {"Categoria", "Categoria" },
                {"EsEliminado", "Eliminacion" },
                {"VisibilidadWeb","Se muestra Online" },
                {"PrecioPublico","Precio Publico" }
            };
        }
        public void ActualizarContenidoControl()
        {
            if (ContenidoControl is IDisposable disposableVM)
                disposableVM.Dispose();

            ContenidoControl = null;

            switch (PropiedadElegida)
            {
                case "Haber":
                case "Precio":
                    ContenidoControl = new NuevoValorNumericoViewModel(PropiedadElegida);
                    break;
                case "PrecioPublico":
                case "VisibilidadWeb":
                case "EsEliminado":
                    ContenidoControl = new NuevoValorBooleanoViewModel(PropiedadElegida);
                    break;
                case "UbicacionID":
                    //TODO Seleccion de entidad + creación en linea
                    break;
                case "Categoria":
                    //TODO Seleccion de entidad + creación en linea
                    break;
            }
        }
        public async Task InicializarVista()
        {
            if (ColeccionPropiedadesProductos.Count > 0)
                return;

            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ProductoSKU",
                "FechaCreacion",
                "ModoEdicionActivo",
                "ModoLecturaActivo",
                "FechaModificacion",
                "CategoriaNombre",
                "UbicacionNombre",
                "MarcaNombre",
                "EAN",
                "RutaImagen",
                "Nombre",
                "ID",
                "ProductoVersionID",
                "FormatoProductoID",
                "FormatoNombre",
                "MarcaID",
                "Alto",
                "Profundidad",
                "Largo",
                "Peso"
            };

            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            foreach (var propiedad in propiedadesEntidad)
            {
                if (listaExclusion.Contains(propiedad.Name))
                    continue;
                WrapperSeleccionPropiedad item = new WrapperSeleccionPropiedad
                {
                    PropiedadNombre = propiedad.Name,
                    Display = MapeoPropiedades[propiedad.Name]
                };
                ColeccionPropiedadesProductos.Add(item);
            }
        }
        private void GuardarCambios(object parameter)
        {
            List<ProductoSKU_Propiedad_Valor> listaModificacion = new List<ProductoSKU_Propiedad_Valor>();
            NuevoValor = ContenidoControl.InputUsuario;
            foreach (ProductoBase producto in ColeccionProductosEditar)
            {
                ProductoSKU_Propiedad_Valor item = new ProductoSKU_Propiedad_Valor
                {
                    ProductoSKU = producto.ProductoSKU,
                    PropiedadNombre = PropiedadElegida,
                    Valor = NuevoValor
                };

                listaModificacion.Add(item);
            }

            if (Orquestador.ModificarListaProductos(listaModificacion))
            {
                if (ContenidoControl is IDisposable disposableVM)
                {
                    ContenidoControl = null;
                    disposableVM.Dispose();
                    MostrarOpcionesFlag = false;
                }
                ContadorItemsElegidos = 0;
                ColeccionProductosEditar.Clear();
                servicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Lote de items modificados", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                servicioSFX.Suspenso();
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo registrar la modificación", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        private void OnValidacionBoton(ToggleHabilitarBotonEdicion mensaje)
        {
            if(ContenidoControl.InputUsuario != null && mensaje.Estado)
            {
                BotonHabilitado = mensaje.Estado;
            }
            else
            {
                BotonHabilitado = false;
                NuevoValor = null;
            }
        }
        private void EliminarLista(object parameter)
        {
            if (ColeccionProductosEditar.Count > 0)
            {
                ColeccionProductosEditar.Clear();
                ContadorItemsElegidos = 0;
            }
        }
        private void ModificarItem(ProductoCatalogo item)
        {
            item.ModoEdicionActivo = !item.ModoEdicionActivo;
        }
        private void EliminarItem(ProductoCatalogo ItemEliminar)
        {
            ProductoCatalogo itemLista = ColeccionProductosEditar.FirstOrDefault(V => V == ItemEliminar);
            if (itemLista != null)
            {
                ColeccionProductosEditar.Remove(ItemEliminar);
                ContadorItemsElegidos--;
            }

        }
        private void OnProductoAniadidoEdicion(NuevoProductoEdicion Producto)
        {
            ProductoBase nuevoItem = Producto.ProductoAniadido;

            ProductoBase registroVigente = ColeccionProductosEditar.FirstOrDefault(V => V.ProductoSKU == nuevoItem.ProductoSKU);
            if (registroVigente == null)
            {
                ProductoCatalogo item = Orquestador.RecuperarProductoPorID(nuevoItem.ProductoSKU);
                item.ModoEdicionActivo = false;
                ColeccionProductosEditar.Add(item);
                ContadorItemsElegidos++;
            }
            servicioSFX.Swipe();
        }
        private void VerListaEdicion(object parameter)
        {
            MostrarListaProductos = !MostrarListaProductos;
        }
        private void MostrarOpciones(object parameter)
        {
            MostrarOpcionesFlag = !MostrarOpcionesFlag;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Dispose()
        {
            //Messenger.Default.Unregister(this);
            GC.SuppressFinalize(this);
        }
    }
}
