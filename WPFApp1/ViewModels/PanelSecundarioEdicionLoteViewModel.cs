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
    public class PanelSecundarioEdicionLoteViewModel : IPanelContextualVM, INotifyPropertyChanged, IDisposable
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
        public ICommand EliminarItemCommand { get; set; }
        public ICommand ModificarItemCommand { get; set; }
        public ICommand VerListaEdicionCommand { get; }
        public ICommand MostrarOpcionesCommand { get; }
        public ICommand EliminarListaCommand {get;}
        public ICommand GuardarCambiosCommand { get; }
        public ObservableCollection<ProductoCatalogo> ColeccionProductosEditar { get; set; }
        private readonly ServicioSFX servicioSFX;
        private readonly OrquestadorProductos Orquestador;
        public PanelSecundarioEdicionLoteViewModel(ServicioSFX servicioSFX, OrquestadorProductos _orquestador)
        {
            _MostrarListaProductos = true;
            _MostrarOpcionesFlag = false;
            _BotonHabilitado = false;
            ColeccionProductosEditar = new ObservableCollection<ProductoCatalogo>();

            VerListaEdicionCommand = new RelayCommand<object>(VerListaEdicion);
            MostrarOpcionesCommand = new RelayCommand<object>(MostrarOpciones);
            ModificarItemCommand = new RelayCommand<ProductoCatalogo>(ModificarItem);
            EliminarItemCommand = new RelayCommand<ProductoCatalogo>(EliminarItem);
            EliminarListaCommand = new RelayCommand<object>(EliminarLista);
            GuardarCambiosCommand = new RelayCommand<object>(GuardarCambios);
            Messenger.Default.Subscribir<NuevoProductoEdicion>(OnProductoAniadidoEdicion);
            this.servicioSFX = servicioSFX;
            this.Orquestador = _orquestador;
        }
        private void GuardarCambios(object parameter)
        {

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
