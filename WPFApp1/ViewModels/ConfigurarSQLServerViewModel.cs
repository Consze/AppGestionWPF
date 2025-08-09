using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Mensajes;
using WPFApp1.Repositorios;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class ConfigurarSQLServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AceptarEntradaCommand { get; private set; }
        public ICommand CancelarEntradaCommand { get; private set; }
        public ICommand CambiarModoAutenticacionCommand { get; private set; }
        public ICommand CambiarEstadoServidorCommand { get; }
        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    OnPropertyChanged(nameof(Descripcion));
                }
            }
        }
        public bool Seleccion { get; set; }
        private ConexionDBSQLServer _repositorioServidor;
        private string _nombreComputadora;
        private ServicioSFX _servicioSFX { get; set; }
        public string NombreComputadora
        {
            get {  return _nombreComputadora; }
            set
            {
                if(_nombreComputadora != value)
                {
                    _nombreComputadora = value;
                    OnPropertyChanged(nameof(NombreComputadora));
                }
            }
        }
        private string _nombreInstanciaServidor;
        public string NombreInstanciaServidor
        {
            get { return _nombreInstanciaServidor; }
            set
            {
                if (_nombreInstanciaServidor != value)
                {
                    _nombreInstanciaServidor = value;
                    OnPropertyChanged(nameof(NombreInstanciaServidor));
                }
            }
        }
        private string _nombreBaseDatos;
        public string NombreBaseDatos
        {
            get { return _nombreBaseDatos; }
            set
            {
                if (_nombreBaseDatos != value)
                {
                    _nombreBaseDatos = value;
                    OnPropertyChanged(nameof(NombreBaseDatos));
                }
            }
        }
        private string _nombreUsuario;
        public string NombreUsuario
        {
            get { return _nombreUsuario; }
            set
            {
                if (_nombreUsuario != value)
                {
                    _nombreUsuario = value;
                    OnPropertyChanged(nameof(NombreUsuario));
                }
            }
        }
        private bool _toggleActivado;
        public bool ToggleActivado
        {
            get { return _toggleActivado; }
            set
            {
                if (_toggleActivado != value)
                {
                    _toggleActivado = value;
                    OnPropertyChanged(nameof(ToggleActivado));
                }
            }
        }
        private string _textoToggle;
        public string TextoToggle
        {
            get { return _textoToggle; }
            set
            {
                if (_textoToggle != value)
                {
                    _textoToggle = value;
                    OnPropertyChanged(nameof(TextoToggle));
                }
            }
        }
        private string _claveUsuario;
        public string ClaveUsuario
        {
            get { return _claveUsuario; }
            set
            {
                if (_claveUsuario != value)
                {
                    _claveUsuario = value;
                    OnPropertyChanged(nameof(ClaveUsuario));
                }
            }
        }
        public string CadenaConexionAServidor { get; private set; }
        public bool CadenaValida { get; private set; }
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
        private bool _botonesActivos;
        public bool BotonesActivos
        {
            get { return _botonesActivos; }
            set
            {
                if (_botonesActivos != value)
                {
                    _botonesActivos = value;
                    OnPropertyChanged(nameof(BotonesActivos));
                }
            }
        }

        public ConfigurarSQLServerViewModel(ConexionDBSQLServer RepositorioServidor)
        {
            //Configuración inicial de toggle
            _repositorioServidor = RepositorioServidor;
            _servicioSFX = new ServicioSFX();
            this.Seleccion = _repositorioServidor.LeerConfiguracionManual();
            if (this.Seleccion)
            {
                this.Descripcion = "Cambiar a SQLite";
            }
            else
            {
                this.Descripcion = "Cambiar a SQLServer";
            }

            _botonesActivos = true;
            _procesando = false;
            CadenaValida = false;
            ToggleActivado = false;
            TextoToggle = "Autenticación Windows";
            
            AceptarEntradaCommand = new RelayCommand<object>(async (param) => await PresentarEntrada());
            CancelarEntradaCommand = new RelayCommand<object>(CancelarEntrada);
            CambiarModoAutenticacionCommand = new RelayCommand<object>(CambiarModoAutenticacion);
            CambiarEstadoServidorCommand = new RelayCommand<object>(async (param) => await CambiarEstadoServidorAsync());
        }
        private async Task CambiarEstadoServidorAsync()
        {
            this.Seleccion = !this.Seleccion;
            string DBMSElegido = string.Empty;
            if (this.Seleccion)
            {
                this.Descripcion = "Cambiar a SQLite";
                DBMSElegido = "Las operaciones se realizaran en el servidor.";
            }
            else
            {
                this.Descripcion = "Cambiar a SQLServer";
                DBMSElegido = "Las operaciones se realizaran de manera local.";
            }
            _servicioSFX.Confirmar();
            Notificacion _notificacion = new Notificacion { Mensaje = DBMSElegido, Titulo = "Cambio de DBMS", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            _repositorioServidor.GuardarConfiguracionManual(this.Seleccion);
        }
        
        public void CambiarModoAutenticacion(object parameter)
        {
            ToggleActivado = !ToggleActivado;
            if(ToggleActivado)
            {
                TextoToggle = "Autenticación SQL";
            }
            else
            {
                TextoToggle = "Autenticación Windows";
            }   
        }
        public void CancelarEntrada(object parameter)
        {
            CadenaConexionAServidor = null;
            CadenaValida = false;
        }

        public async Task PresentarEntrada()
        {
            Procesando = true;
            await PresentarEntradaAsync().ConfigureAwait(false);
        }
        public async Task PresentarEntradaAsync()
        {
            if (ToggleActivado) //Autenticación SQL
            {
                if (NombreBaseDatos != null && NombreComputadora != null && NombreInstanciaServidor != null && NombreUsuario != null && ClaveUsuario != null)
                { 
                    CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};User ID={NombreUsuario};Password={ClaveUsuario}";
                    CadenaValida = true;
                }
                else
                {
                    CadenaValida = false;
                }
            }
            else //Autenticación Windows
            {
                if (NombreBaseDatos != null && NombreComputadora != null && NombreInstanciaServidor != null)
                {
                    CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};Integrated Security=True;";
                    CadenaValida = true;
                }
                else
                {
                    CadenaValida = false;
                }
            }

            if (CadenaValida)
            {
                BotonesActivos = false;
                string cadenaConexion = CadenaConexionAServidor;
                _repositorioServidor.CadenaConexion = cadenaConexion;
                bool conexionExitosa = await Task.Run(() => _repositorioServidor.ProbarConexion(cadenaConexion));
                _repositorioServidor.ConexionValida = conexionExitosa;
                await Task.Run(() => _repositorioServidor.GuardarEstadoConexion());
                Procesando = false;
                BotonesActivos = true;
                if (conexionExitosa)
                {
                    System.Windows.MessageBox.Show($"Conexion Exitosa a: {cadenaConexion}", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("No se pudo establecer conexion con la cadena ingresada", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                Procesando = false;
                System.Windows.MessageBox.Show("No se ingresaron los datos suficientes para establecer la conexión", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
