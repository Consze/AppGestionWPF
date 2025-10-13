using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class PanelSecundarioCatalogoViewModel : IPanelContextualVM, INotifyPropertyChanged
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
                    /**
                    if(value > 0)
                    {
                        BotonHabilitado = true;
                    }
                    else
                    {
                        BotonHabilitado = false;
                    }*/
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
        public ObservableCollection<Ventas> ColeccionProductosVenta { get; set; }
        public ObservableCollection<Medios_Pago> ColeccionMediosPago { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand VerListaCarritoCommand { get; }
        public ICommand EliminarItemCommand { get; }
        public ICommand RegistrarVentaCommand { get; }
        private readonly OrquestadorProductos Orquestador;
        private readonly IConmutadorEntidadGenerica<Medios_Pago> servicioMediosPago;
        public PanelSecundarioCatalogoViewModel(OrquestadorProductos _orquestador, IConmutadorEntidadGenerica<Medios_Pago> _servicioMediosPago)
        {
            servicioMediosPago = _servicioMediosPago;
            Orquestador = _orquestador;
            _ContadorItemsElegidos = 0;
            _MostrarListaProductos = false;
            _BotonHabilitado = false;
            ColeccionProductosVenta = new ObservableCollection<Ventas>();
            ColeccionMediosPago = new ObservableCollection<Medios_Pago>();
            VerListaCarritoCommand = new RelayCommand<object>(VerListaCarrito);
            EliminarItemCommand = new RelayCommand<Ventas>(EliminarItem);
            RegistrarVentaCommand = new RelayCommand<object>(VenderLista);

            ColeccionProductosVenta.CollectionChanged += ColeccionProductosVenta_ColeccionModificada;
            Messenger.Default.Subscribir<NuevoProductoCarritoMessage>(OnProductoAniadidoCarrito);
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
            if (!ColeccionProductosVenta.Any())
            {
                BotonHabilitado = false;
                return;
            }

            bool hayItemsSinPago = ColeccionProductosVenta
                .Any(v => string.IsNullOrEmpty(v.MedioPagoID));

            BotonHabilitado = !hayItemsSinPago;
        }
        private void VenderLista(object parameter)
        {
            //Orquestador.VenderProductos();
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
            Ventas registroVigente = ColeccionProductosVenta.FirstOrDefault(V => V.ItemVendido == nuevoItemVendido.ItemVendido);
            if (registroVigente != null)
            {
                registroVigente.Cantidad++;
            }
            else
            {
                ColeccionProductosVenta.Add(nuevoItemVendido);
                ContadorItemsElegidos++;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
