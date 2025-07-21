using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand VerListaCommand { get; }
        public ICommand AniadirPersonaCommand { get; }
        public ICommand EliminarPersonaCommand { get; }
        public ICommand EditarPersonaCommand { get; }
        public ICommand VerCatalogoCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand VerExportarProductosCommand { get; }
        public ICommand ConfigurarServidorCommand { get; }
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
            VerListaCommand = new RelayCommand<object>(VerLista);
            AniadirPersonaCommand = new RelayCommand<object>(AniadirPersona);
            EliminarPersonaCommand = new RelayCommand<object>(EliminarPersona);
            EditarPersonaCommand = new RelayCommand<object>(EditarPersona);
            VerCatalogoCommand = new RelayCommand<object>(async (param) => await VerCatalogoAsync());
            CambiarVistaCommand = new RelayCommand<object>(async (vista) => await CambiarVistaAsync(vista));
            VerExportarProductosCommand = new RelayCommand<object>(VerExportarProductos);
            ConfigurarServidorCommand = new RelayCommand<object>(ConfigurarServidor);

            Messenger.Default.Subscribir<AbrirVistaAniadirProductoMensaje>(OnAbrirAniadirProducto);
            Messenger.Default.Subscribir<CerrarVistaAniadirProductoMensaje>(OnCerrarAniadirProducto);
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
                ExportarProductosViewModel _viewModel = new ExportarProductosViewModel();
                ExportarProductos _vista = new ExportarProductos(_viewModel);
                CambiarVistaAsync(_vista);
            }
        }
        private void VerLista(object parameter)
        {
            ListaPersonas lista = new ListaPersonas();
            lista.Show();
        }
        private void ConfigurarServidor(object parameter)
        {
            if(VistaActual is ConfigurarSQLServer)
            {
                VistaActual = null;
                TituloActivo = "Menú";
            }
            else
            {
                TituloActivo = "Conectarse a Servidor";
                ConfigurarSQLServerViewModel viewModel = new ConfigurarSQLServerViewModel();
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
                CatalogoViewModel _viewModel = await Task.Run(()=> new CatalogoViewModel());
                Catalogo vista = new Catalogo(_viewModel);
                CambiarVistaAsync(vista);
            }
            Procesando = false;
        }
        private void AniadirPersona(object parameter)
        {
            AniadirPersona _AniadirPersona = new AniadirPersona();
            _AniadirPersona.Show();
        }
        private async Task CambiarVistaAsync(object nuevaVista)
        {
            CargandoVista = true;
            await Task.Delay(200);
            VistaActual = nuevaVista;
            CargandoVista = false;
        }
        private void EliminarPersona(object parameter)
        {
            EliminarPersona _EliminarPersona = new EliminarPersona();
            _EliminarPersona.Show();
        }
        private void EditarPersona(object parameter)
        {
            EntradaUsuario ventanaEntrada = new EntradaUsuario();
            bool? resultado = ventanaEntrada.ShowDialog();
            if (resultado.HasValue && resultado.Value == true)
            {
                if (!int.TryParse(ventanaEntrada.NumeroElegido, out int Numero))
                {
                    System.Windows.MessageBox.Show("Debe ingresar un numero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Persona _registro = personaRepository.RecuperarRegistro(Numero);
                if (_registro.id > 0 )
                {
                    EditarPersona _EditarPersona = new EditarPersona(_registro);
                    _EditarPersona.Show();
                }
                else
                {
                    System.Windows.MessageBox.Show("No existe un registro con ese ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
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
