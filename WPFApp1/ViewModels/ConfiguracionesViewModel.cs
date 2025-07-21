using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class EstadoSQLServer()
    {
        public bool ServidorActivo { get; set; }
    }
    public class ConfiguracionesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
        public ConfiguracionesViewModel()
        {
            _repositorioServidor = new ConexionDBSQLServer();
            this.Seleccion = _repositorioServidor.LeerConfiguracionManual();
            if (this.Seleccion)
            {
                this.Descripcion = "Cambiar a SQLite";
            }
            else
            {
                this.Descripcion = "Cambiar a SQLServer";
            }
            CambiarEstadoServidorCommand = new RelayCommand<object>(async (param) => await CambiarEstadoServidorAsync());
        }
        private async Task CambiarEstadoServidorAsync()
        {
            this.Seleccion = !this.Seleccion;
            if (this.Seleccion)
            {
                this.Descripcion = "Cambiar a SQLite";
            }
            else
            {
                this.Descripcion = "Cambiar a SQLServer";
            }
            _repositorioServidor.GuardarConfiguracionManual(this.Seleccion);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
