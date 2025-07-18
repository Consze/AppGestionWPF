﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;

namespace WPFApp1
{
    public enum VistaElegida
    {
        Tabla,
        Galeria,
        Ninguna
    }
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

        public CatalogoViewModel()
        {
            this._mostrarVistaTabular = false;
            this._mostrarVistaGaleria = true;
            ColeccionProductos = new ObservableCollection<Productos>();
            ItemDoubleClickCommand = new RelayCommand<object>(EjecutarDobleClickItem);
            AniadirProductoCommand = new RelayCommand<object>(MostrarAniadirProducto);
            AlternarFormatoVistaCommand = new RelayCommand<object>(async (param) => await AlternarFormatoVista());
            Messenger.Default.Subscribir<ProductoAniadidoMensaje>(OnNuevoProductoAniadido);
            Messenger.Default.Subscribir<ProductoModificadoMensaje>(OnProductoModificado);
            
            Task.Run(async () => await CargarEstadoInicialAsync());
            Task.Run(async () => await CargarProductosAsync());
        }

        public async Task CargarEstadoInicialAsync()
        {
            this.Procesando = true;
            VistaElegida vista = PersistenciaConfiguracion.LeerUltimaVista();
            switch(vista)
            {
                case VistaElegida.Ninguna:
                case VistaElegida.Galeria:
                    this.MostrarVistaGaleria = true;
                    this.MostrarVistaTabular = false;
                    break;
                case VistaElegida.Tabla:
                    this.MostrarVistaGaleria = false;
                    this.MostrarVistaTabular = true;
                    break;
            }
            this.Procesando = false;
        }

        public async Task AlternarFormatoVista()
        {
            this.Procesando = true;
            await AlternarFormatoVistaAsync().ConfigureAwait(false);
        }

        public async Task AlternarFormatoVistaAsync()
        {
            VistaElegida vista = new VistaElegida();
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (this.MostrarVistaGaleria)
                {
                    vista = VistaElegida.Tabla;
                    this.MostrarVistaGaleria = false;
                    this.MostrarVistaTabular = true;
                }
                else
                {
                    vista = VistaElegida.Galeria;
                    this.MostrarVistaGaleria = true;
                    this.MostrarVistaTabular = false;
                }
                PersistenciaConfiguracion.GuardarUltimaVista(vista);
            });
            this.Procesando = false;
        }
        public void MostrarAniadirProducto(object parameter)
        {
            if (AniadirProducto.Instancias < 1)
            {
                Messenger.Default.Publish(new AbrirVistaAniadirProductoMensaje());
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
        private async Task CargarProductosAsync()
        {
            List<Productos> registros = await Task.Run(() => ProductosRepository.LeerProductos());
            System.Windows.Application.Current.Dispatcher.Invoke(() => 
            {
            foreach (var producto in registros)
                {
                    ColeccionProductos.Add(producto);
                }
                this.Procesando = false;
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
