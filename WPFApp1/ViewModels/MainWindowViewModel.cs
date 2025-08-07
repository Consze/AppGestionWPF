using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand VerCatalogoCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand VerExportarProductosCommand { get; }
        public ICommand ConfigurarServidorCommand { get; }
        public ICommand AbrirConfiguracionesCommand { get; }
        public ICommand CambiarVistaCommand { get; }
        private bool _procesando;
        public bool Procesando
        {
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
        private object _vistaActual;
        public object VistaActual
        {
            get { return _vistaActual; }
            set
            {
                if (_vistaActual != value)
                {
                    _vistaActual = value;
                    OnPropertyChanged(nameof(VistaActual));
                }
            }
        }
        private bool _isAniadirProductoActivo = false;
        public bool IsAniadirProductoActivo
        {
            get { return _isAniadirProductoActivo; }
            set
            {
                if (_isAniadirProductoActivo != value)
                {
                    _isAniadirProductoActivo = value;
                    OnPropertyChanged(nameof(IsAniadirProductoActivo));
                }
            }
        }
        private bool _cargandoVista;
        public bool CargandoVista
        {
            get { return _cargandoVista; }
            set
            {
                if (_cargandoVista != value)
                {
                    _cargandoVista = value;
                    OnPropertyChanged(nameof(CargandoVista));
                }
            }
        }
        private string _tituloActivo;
        private UCNotificacionViewModel _notificacionViewModel;
        public UCNotificacionViewModel NotificacionViewModel
        {
            get { return _notificacionViewModel; }
            set
            {
                if (_notificacionViewModel != value)
                {
                    _notificacionViewModel = value;
                    OnPropertyChanged(nameof(NotificacionViewModel));
                }
            }
        }
        public ObservableCollection<Notificacion> ColeccionNotificaciones { get; set; }
        public string TituloActivo
        {
            get { return _tituloActivo; }
            set
            {
                if (_tituloActivo != value)
                {
                    _tituloActivo = value;
                    OnPropertyChanged(nameof(TituloActivo));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindowViewModel()
        {
            _tituloActivo = "Menú";
            _isAniadirProductoActivo = false;
            _vistaActual = null;
            _procesando = false;
            _cargandoVista = false;

            // Notificaciones
            ColeccionNotificaciones = new ObservableCollection<Notificacion>();

            this.NotificacionViewModel = new UCNotificacionViewModel();
            AbrirConfiguracionesCommand = new RelayCommand<object>(AbrirConfiguraciones);
            VerCatalogoCommand = new RelayCommand<object>(async (param) => await VerCatalogoAsync());
            CambiarVistaCommand = new RelayCommand<object>(async (vista) => await CambiarVistaAsync(vista));
            VerExportarProductosCommand = new RelayCommand<object>(VerExportarProductos);
            ConfigurarServidorCommand = new RelayCommand<object>(ConfigurarServidor);

            Messenger.Default.Subscribir<AbrirVistaAniadirProductoMensaje>(OnAbrirAniadirProducto);
            Messenger.Default.Subscribir<CerrarVistaAniadirProductoMensaje>(OnCerrarAniadirProducto);
            Messenger.Default.Subscribir<NotificacionEmergente>(OnNotificacionEmergenteAsync);
        }
        private void AbrirConfiguraciones(object parameter)
        {
            if(VistaActual is Configuraciones)
            {
                TituloActivo = "Menú";
                this.VistaActual = null;
            }
            else
            {
                TituloActivo = "Configuración";
                ConfiguracionesViewModel viewModel = new ConfiguracionesViewModel();
                Configuraciones vista = new Configuraciones(viewModel);
                CambiarVistaAsync(vista);
            }
        }
        private void VerExportarProductos(object parameter)
        {
            if(VistaActual is ExportarProductos)
            {
                VistaActual = null;
                TituloActivo = "Menú";
            }
            else
            {
                TituloActivo = "Exportar Productos";
                var _viewModel = App.GetService<ExportarProductosViewModel>();
                ExportarProductos _vista = new ExportarProductos(_viewModel);
                CambiarVistaAsync(_vista);
            }
        }
        private void ConfigurarServidor(object parameter)
        {
            if(VistaActual is ConfigurarSQLServer)
            {
                TituloActivo = "Menú";
                this.VistaActual = null;
            }
            else
            {
                TituloActivo = "Conectarse a Servidor";
                ConfigurarSQLServerViewModel viewModel = App.GetService<ConfigurarSQLServerViewModel>();
                ConfigurarSQLServer vista = new ConfigurarSQLServer(viewModel);
                CambiarVistaAsync(vista);
            }
        }
        private async Task VerCatalogoAsync()
        {
            Procesando = true;
            if (VistaActual is Catalogo)
            {
                VistaActual = null;
                TituloActivo = "Menú";
            }
            else
            {
                TituloActivo = "Catálogo";
                var _viewModel = App.GetService<CatalogoViewModel>();
                //CatalogoViewModel _viewModel = await Task.Run(()=> new CatalogoViewModel());
                Catalogo vista = new Catalogo(_viewModel);
                CambiarVistaAsync(vista);
            }
            Procesando = false;
        }
        private async Task CambiarVistaAsync(object nuevaVista)
        {
            CargandoVista = true;
            await Task.Delay(200);
            VistaActual = nuevaVista;
            CargandoVista = false;
        }
        private async void OnNotificacionEmergenteAsync(NotificacionEmergente Notificacion)
        {
            if(Notificacion?.NuevaNotificacion != null) // Agregar notificacion
            {
                this.ColeccionNotificaciones.Add(Notificacion.NuevaNotificacion);
                string[] palabras = Notificacion.NuevaNotificacion.Mensaje.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int cantidadPalabras = palabras.Length;
                double segundosCalculados = Math.Ceiling(cantidadPalabras / 3.0);
                int segundosDisplay = (int)Math.Max(segundosCalculados, 5.0);
                await Task.Delay(TimeSpan.FromSeconds(segundosDisplay));

                if (this.ColeccionNotificaciones.Contains(Notificacion.NuevaNotificacion))
                {
                    this.ColeccionNotificaciones.Remove(Notificacion.NuevaNotificacion);
                }
            }
        }
        private void OnAbrirAniadirProducto(AbrirVistaAniadirProductoMensaje mensaje)
        {
            IsAniadirProductoActivo = true;
        }
        private void OnCerrarAniadirProducto(CerrarVistaAniadirProductoMensaje mensaje)
        {
            IsAniadirProductoActivo = false;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
