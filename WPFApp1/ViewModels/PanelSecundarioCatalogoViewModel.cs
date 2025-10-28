using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;
using System.IO;

namespace WPFApp1.ViewModels
{
    public class PanelSecundarioCatalogoViewModel : IPanelContextualVM, INotifyPropertyChanged, IDisposable
    {
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
        private bool _mostrarOpciones;
        public bool MostrarOpcionesFlag
        {
            get { return _mostrarOpciones; }
            set
            {
                if (_mostrarOpciones != value)
                {
                    _mostrarOpciones = value;
                    OnPropertyChanged(nameof(MostrarOpcionesFlag));
                }
            }
        }
        private string _sucursalID;
        public string SucursalID
        {
            get { return _sucursalID; }
            set
            {
                if (_sucursalID != value)
                {
                    _sucursalID = value;
                    OnPropertyChanged(nameof(SucursalID));
                    ValidarSeleccionMediosPago();
                }
            }
        }
        private bool Inicializado;
        private DateTime _FechaElegida;
        public DateTime FechaElegida
        {
            get { return _FechaElegida; }
            set
            {
                if (_FechaElegida != value)
                {
                    _FechaElegida = value;
                    OnPropertyChanged(nameof(FechaElegida));
                }
            }
        }
        public ObservableCollection<Sucursal> ColeccionSucursales { get; set; }
        public ObservableCollection<Ventas> ColeccionProductosVenta { get; set; }
        public ObservableCollection<Medios_Pago> ColeccionMediosPago { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand VerListaCarritoCommand { get; }
        public ICommand EliminarItemCommand { get; }
        public ICommand RegistrarVentaCommand { get; }
        public ICommand ModificarItemCommand { get; }
        public ICommand MostrarOpcionesCommand { get; }
        public ICommand EliminarListaCommand { get; }
        private readonly OrquestadorProductos Orquestador;
        private readonly IConmutadorEntidadGenerica<Medios_Pago> servicioMediosPago;
        private readonly IConmutadorEntidadGenerica<Sucursal> servicioSucursales;
        private readonly ServicioSFX servicioSFX;
        public PanelSecundarioCatalogoViewModel(OrquestadorProductos _orquestador, IConmutadorEntidadGenerica<Medios_Pago> _servicioMediosPago, 
            ServicioSFX _servicioSFX, IConmutadorEntidadGenerica<Sucursal> _servicioSucursales)
        {
            Inicializado = false;
            FechaElegida = DateTime.Today;
            _sucursalID = string.Empty;
            servicioSucursales = _servicioSucursales;
            servicioSFX = _servicioSFX;
            servicioMediosPago = _servicioMediosPago;
            Orquestador = _orquestador;
            _ContadorItemsElegidos = 0;
            _MostrarListaProductos = true;
            _mostrarOpciones = false;
            _BotonHabilitado = false;
            ColeccionSucursales = new ObservableCollection<Sucursal>();
            ColeccionProductosVenta = new ObservableCollection<Ventas>();
            ColeccionMediosPago = new ObservableCollection<Medios_Pago>();
            VerListaCarritoCommand = new RelayCommand<object>(VerListaCarrito);
            EliminarItemCommand = new RelayCommand<Ventas>(EliminarItem);
            RegistrarVentaCommand = new RelayCommand<object>(VenderLista);
            ModificarItemCommand = new RelayCommand<Ventas>(ModificarItem);
            MostrarOpcionesCommand = new RelayCommand<object>(MostrarOpciones);
            EliminarListaCommand = new RelayCommand<object>(EliminarLista);

            ColeccionProductosVenta.CollectionChanged += ColeccionProductosVenta_ColeccionModificada;
            Messenger.Default.Subscribir<NuevoProductoCarritoMessage>(OnProductoAniadidoCarrito);
        }
        private void EliminarLista(object parameter)
        {
            if (ColeccionProductosVenta.Count > 0)
            {
                ColeccionProductosVenta.Clear();
                ContadorItemsElegidos = 0;
            }
        }
        private void ColeccionProductosVenta_ColeccionModificada(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (Ventas item in e.OldItems)
                {
                    item.PropertyChanged -= VentaItem_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Ventas item in e.NewItems)
                {
                    item.PropertyChanged += VentaItem_PropertyChanged;
                }
            }

            ValidarSeleccionMediosPago();
        }
        private void VentaItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Ventas.MedioPagoID))
            {
                ValidarSeleccionMediosPago();
            }
        }
        private void ValidarSeleccionMediosPago()
        {
            if (!ColeccionProductosVenta.Any() || SucursalID == string.Empty)
            {
                BotonHabilitado = false;
                return;
            }

            bool hayItemsSinPago = ColeccionProductosVenta
                .Any(v => string.IsNullOrEmpty(v.MedioPagoID));

            BotonHabilitado = !hayItemsSinPago;
        }
        private void MostrarOpciones(object parameter)
        {
            MostrarOpcionesFlag = !MostrarOpcionesFlag;
        }
        private void ModificarItem(Ventas ItemModificar)
        {
            ItemModificar.ModoEdicionActivo = !ItemModificar.ModoEdicionActivo;
        }
        private void VenderLista(object parameter)
        {
            List<Ventas> ListaVentas = new List<Ventas>();
            foreach(Ventas registro in ColeccionProductosVenta)
            {
                registro.SucursalID = SucursalID;
                registro.FechaVenta = FechaElegida;
                ListaVentas.Add(registro);
            }
            if (Orquestador.VenderProductos(ListaVentas))
            {
                ContadorItemsElegidos = 0;
                ColeccionProductosVenta.Clear();
                Notificacion _notificacion = new Notificacion { Mensaje = "Venta registrada con exito", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo registrar la venta", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
        }
        public async Task InicializarVM()
        {
            if (Inicializado)
                return;
            
            await CargarMediosPago();
            await CargarSucursales();
            Inicializado = true;
        }
        public async Task CargarSucursales()
        {
            await foreach (Sucursal sucursal in servicioSucursales.RecuperarStreamAsync())
            {
                ColeccionSucursales.Add(sucursal);
            }
        }
        public async Task CargarMediosPago()
        {
            await foreach (Medios_Pago medioPago in servicioMediosPago.RecuperarStreamAsync())
            {
                ColeccionMediosPago.Add(medioPago);
            }
        }
        private void VerListaCarrito (object parameter)
        {
            MostrarListaProductos = !MostrarListaProductos;
        }
        private void EliminarItem(Ventas ItemEliminar)
        {
            Ventas itemLista = ColeccionProductosVenta.FirstOrDefault(V => V.ItemVendido == ItemEliminar.ItemVendido);
            if (itemLista != null)
            {
                if(itemLista.Cantidad == 1)
                {
                    ColeccionProductosVenta.Remove(ItemEliminar);
                    ContadorItemsElegidos--;
                }
                else
                {
                    itemLista.Cantidad--;
                }
            }
            
        }
        private void OnProductoAniadidoCarrito(NuevoProductoCarritoMessage Producto)
        {
            Ventas nuevoItemVendido = Producto.VentaDTO;
            nuevoItemVendido.precioVenta = nuevoItemVendido.ItemVendido.Precio;
            nuevoItemVendido.ModoEdicionActivo = false;
            Ventas registroVigente = ColeccionProductosVenta.FirstOrDefault(V => V.ItemVendido.ProductoSKU == nuevoItemVendido.ItemVendido.ProductoSKU);
            if (registroVigente != null)
            {
                registroVigente.Cantidad++;
            }
            else
            {
                ColeccionProductosVenta.Add(nuevoItemVendido);
                ContadorItemsElegidos++;
            }
            servicioSFX.Swipe();
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
