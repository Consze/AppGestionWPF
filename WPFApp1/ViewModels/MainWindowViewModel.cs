using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Vistas;
using WPFApp1.Servicios;
using System.IO;

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
        public ICommand DobleClickNotificacionCommand { get; }
        public ICommand ColapsarPanelCommand { get; }
        public ICommand ColapsarPanelSecundarioCommand { get; }
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
        private IPanelContextualVM _VistaPanelSecundario;
        public IPanelContextualVM VistaPanelSecundario
        {
            get { return _VistaPanelSecundario; }
            set
            {
                if (_VistaPanelSecundario != value)
                {
                    _VistaPanelSecundario = value;
                    OnPropertyChanged(nameof(VistaPanelSecundario));
                    if (value != null)
                    {
                        ToggleIconoPanelSecundario = true;
                    }
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
        private string _TituloPanelSecundario;
        public string TituloPanelSecundario
        {
            get { return _TituloPanelSecundario; }
            set
            {
                if (_TituloPanelSecundario != value)
                {
                    _TituloPanelSecundario = value;
                    OnPropertyChanged(nameof(TituloPanelSecundario));
                }
            }
        }
        private bool _toggleMostrarPanel;
        public bool ToggleMostrarPanel
        {
            get { return _toggleMostrarPanel; }
            set
            {
                if (_toggleMostrarPanel != value)
                {
                    _toggleMostrarPanel = value;
                    OnPropertyChanged(nameof(ToggleMostrarPanel));
                }
            }
        }
        private bool _TogglePanelSecundario;
        public bool TogglePanelSecundario
        {
            get { return _TogglePanelSecundario; }
            set
            {
                if (_TogglePanelSecundario != value)
                {
                    _TogglePanelSecundario = value;
                    OnPropertyChanged(nameof(TogglePanelSecundario));
                }
            }
        }
        private bool _ToggleIconoPanelSecundario;
        public bool ToggleIconoPanelSecundario
        {
            get { return _ToggleIconoPanelSecundario; }
            set
            {
                if (_ToggleIconoPanelSecundario != value)
                {
                    _ToggleIconoPanelSecundario = value;
                    OnPropertyChanged(nameof(ToggleIconoPanelSecundario));
                }
            }
        }
        private string _iconoTogglePanel;
        public string iconoTogglePanel
        {
            get { return _iconoTogglePanel; }
            set
            {
                if (_iconoTogglePanel != value)
                {
                    _iconoTogglePanel = value;
                    OnPropertyChanged(nameof(iconoTogglePanel));
                }
            }
        }
        private string _iconoTogglePanelSecundario;
        public string iconoTogglePanelSecundario
        {
            get { return _iconoTogglePanelSecundario; }
            set
            {
                if (_iconoTogglePanelSecundario != value)
                {
                    _iconoTogglePanelSecundario = value;
                    OnPropertyChanged(nameof(iconoTogglePanelSecundario));
                }
            }
        }
        private int _panelNotificacionesColumnas;
        public int panelNotificacionesColumnas
        {
            get { return _panelNotificacionesColumnas; }
            set
            {
                if (_panelNotificacionesColumnas != value)
                {
                    _panelNotificacionesColumnas = value;
                    OnPropertyChanged(nameof(panelNotificacionesColumnas));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindowViewModel()
        {
            _tituloActivo = "Menú";
            _isAniadirProductoActivo = false;
            _vistaActual = null;
            _VistaPanelSecundario = null;
            _procesando = false;
            _iconoTogglePanel = "/iconos/layout2.png";
            _iconoTogglePanelSecundario = "/iconos/layout2.png";
            _toggleMostrarPanel = true;
            _panelNotificacionesColumnas = 1;
            _TogglePanelSecundario = false;
            _ToggleIconoPanelSecundario = false;
            _TituloPanelSecundario = string.Empty;

            // Notificaciones
            ColeccionNotificaciones = new ObservableCollection<Notificacion>();

            this.NotificacionViewModel = new UCNotificacionViewModel();
            AbrirConfiguracionesCommand = new RelayCommand<object>(AbrirConfiguraciones);
            VerCatalogoCommand = new RelayCommand<object>(async (param) => await VerCatalogoAsync());
            CambiarVistaCommand = new RelayCommand<object>(async (vista) => await CambiarVistaAsync(vista));
            VerExportarProductosCommand = new RelayCommand<object>(VerExportarProductos);
            ConfigurarServidorCommand = new RelayCommand<object>(ConfigurarServidor);
            DobleClickNotificacionCommand = new RelayCommand<object>(InspeccionarNotificacion);
            ColapsarPanelCommand = new RelayCommand<object>(ColapsarPanel);
            ColapsarPanelSecundarioCommand = new RelayCommand<object>(ColapsarPanelSecundario);

            Messenger.Default.Subscribir<AbrirVistaAniadirProductoMensaje>(OnAbrirAniadirProducto);
            Messenger.Default.Subscribir<CerrarVistaAniadirProductoMensaje>(OnCerrarAniadirProducto);
            Messenger.Default.Subscribir<NotificacionEmergente>(OnNotificacionEmergenteAsync);
            Messenger.Default.Subscribir<TogglePanelSecundarioMW>(OnAlternarPanelSecundario);
            Messenger.Default.Subscribir<PanelSecundarioBoxing>(OnPresentarPanelSecundario);
            Messenger.Default.Subscribir<PanelSecundarioStatusRequest>(OnSolicitudEstadoCarrito);
        }
        private void InspeccionarNotificacion(object NotificacionClickeada)
        {
            //TODO: Implementación de accesos directos segun clasificacion de urgencia-importancia y contenido de notificación
            if(NotificacionClickeada is Notificacion _notificacion)
            {
                switch(_notificacion.Urgencia)
                {
                    case MatrizEisenhower.C1:
                        break;
                    case MatrizEisenhower.C2:
                        break;
                    case MatrizEisenhower.C3:
                        break;
                }
            }
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
        private void ColapsarPanel(object parameter)
        {
            ToggleMostrarPanel = !ToggleMostrarPanel;
            if(ToggleMostrarPanel)
            {
                iconoTogglePanel = "/iconos/layout2.png";
                panelNotificacionesColumnas = 1;
            }
            else
            {
                iconoTogglePanel = "/iconos/layout1.png";
                panelNotificacionesColumnas = 2;
            }
        }
        private void ColapsarPanelSecundario(object parameter)
        {
            TogglePanelSecundario = !TogglePanelSecundario;
            if (TogglePanelSecundario)
            {
                iconoTogglePanelSecundario = "/iconos/layout2.png";
            }
            else
            {
                iconoTogglePanelSecundario = "/iconos/layout1.png";
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
            Procesando = true;
            await Task.Delay(2);
            VistaActual = nuevaVista;
            Procesando = false;
        }
        private void OnSolicitudEstadoCarrito(PanelSecundarioStatusRequest mensaje)
        {
            mensaje.PanelSecundarioExiste = TogglePanelSecundario;
        }
        private async void OnNotificacionEmergenteAsync(NotificacionEmergente Notificacion)
        {
            if(Notificacion?.NuevaNotificacion != null) // Agregar notificacion
            {
                Notificacion.NuevaNotificacion.IconoRuta = Path.GetFullPath(Notificacion.NuevaNotificacion.IconoRuta);
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
        private void OnAlternarPanelSecundario(TogglePanelSecundarioMW mensaje)
        {
            if (TogglePanelSecundario == mensaje.MostrarPanel)
                return;

            TogglePanelSecundario = mensaje.MostrarPanel;
        }
        private void OnPresentarPanelSecundario(PanelSecundarioBoxing Vista)
        {
            VistaPanelSecundario = (IPanelContextualVM)Vista.ViewModelGenerico;
            TituloPanelSecundario = Vista.TituloPanel;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
