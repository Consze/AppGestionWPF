using System.ComponentModel;
using System.Windows.Input;

namespace WPFApp1
{
    public class ConfigurarSQLServerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<bool> DialogoCerrado;
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
        public ConfigurarSQLServerViewModel()
        {
            this.ToggleActivado = false;
            this.TextoToggle = "Autenticación SQL";
            AceptarEntradaCommand = new RelayCommand<object>(PresentarEntrada);
            CancelarEntradaCommand = new RelayCommand<object>(PresentarEntrada);
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
            CerrarVista(false);
        }
        public void PresentarEntrada(object parameter)
        {
            if (this.ToggleActivado) //Autenticación Windows
            {
                if (this.NombreBaseDatos != null && this.NombreComputadora != null && this.NombreInstanciaServidor != null && this.NombreUsuario != null && this.ClaveUsuario != null)
                { 
                    this.CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};User ID={NombreUsuario};Password={ClaveUsuario}";
                    CerrarVista(true);
                }
                else
                {
                    CerrarVista(false);
                }
            }
            else //Auntenticación SQL
            {
                if (this.NombreBaseDatos != null && this.NombreComputadora != null && this.NombreInstanciaServidor != null)
                {
                    this.CadenaConexionAServidor = $"Server={NombreComputadora}\\{NombreInstanciaServidor};Database={NombreBaseDatos};Integrated Security=True;";
                    CerrarVista(true);
                }
                else
                {
                    CerrarVista(false);
                }
            }
        }
        public void CerrarVista(bool resultado)
        {
            DialogoCerrado?.Invoke(this, resultado);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
