using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class ConfigurarSQLServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AceptarEntradaCommand { get; private set; }
        public ICommand CancelarEntradaCommand { get; private set; }
        public ICommand CambiarModoAutenticacionCommand { get; private set; }
        private string _nombreComputadora;
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

        public ConfigurarSQLServerViewModel()
        {
            this._botonesActivos = true;
            this._procesando = false;
            this.CadenaValida = false;
            this.ToggleActivado = false;
            this.TextoToggle = "Autenticación Windows";
            AceptarEntradaCommand = new RelayCommand<object>(async (param) => await PresentarEntrada());
            CancelarEntradaCommand = new RelayCommand<object>(CancelarEntrada);
            CambiarModoAutenticacionCommand = new RelayCommand<object>(CambiarModoAutenticacion);
        }
        public void CambiarModoAutenticacion(object parameter)
        {
            this.ToggleActivado = !this.ToggleActivado;
            if(this.ToggleActivado)
            {
                this.TextoToggle = "Autenticación SQL";
            }
            else
            {
                this.TextoToggle = "Autenticación Windows";
            }   
        }
        public void CancelarEntrada(object parameter)
        {
            this.CadenaConexionAServidor = null;
            this.CadenaValida = false;
        }

        public async Task PresentarEntrada()
        {
            this.Procesando = true;
            await PresentarEntradaAsync().ConfigureAwait(false);
        }
        public async Task PresentarEntradaAsync()
        {
            if (this.ToggleActivado) //Autenticación SQL
            {
                if (this.NombreBaseDatos != null && this.NombreComputadora != null && this.NombreInstanciaServidor != null && this.NombreUsuario != null && this.ClaveUsuario != null)
                { 
                    this.CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};User ID={NombreUsuario};Password={ClaveUsuario}";
                    this.CadenaValida = true;
                }
                else
                {
                    this.CadenaValida = false;
                }
            }
            else //Auntenticación Windows
            {
                if (this.NombreBaseDatos != null && this.NombreComputadora != null && this.NombreInstanciaServidor != null)
                {
                    this.CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};Integrated Security=True;";
                    this.CadenaValida = true;
                }
                else
                {
                    this.CadenaValida = false;
                }
            }

            if (this.CadenaValida)
            {
                this.BotonesActivos = false;
                string cadenaConexion = this.CadenaConexionAServidor;
                ConexionDBSQLServer _configuracionServidor = new ConexionDBSQLServer();
                _configuracionServidor.CadenaConexion = cadenaConexion;
                bool conexionExitosa = await Task.Run(() => _configuracionServidor.ProbarConexion(cadenaConexion));
                _configuracionServidor.ConexionValida = conexionExitosa;
                await Task.Run(() => _configuracionServidor.GuardarEstadoConexion());
                this.Procesando = false;
                this.BotonesActivos = true;
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
                System.Windows.MessageBox.Show("No se ingresaron los datos suficientes para establecer la conexión", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
