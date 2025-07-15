using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace WPFApp1
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
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindowViewModel()
        {
            this._procesando = false;
            VerListaCommand = new RelayCommand<object>(VerLista);
            AniadirPersonaCommand = new RelayCommand<object>(AniadirPersona);
            EliminarPersonaCommand = new RelayCommand<object>(EliminarPersona);
            EditarPersonaCommand = new RelayCommand<object>(EditarPersona);
            VerCatalogoCommand = new RelayCommand<object>(VerCatalogo);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            VerExportarProductosCommand = new RelayCommand<object>(VerExportarProductos);
            ConfigurarServidorCommand = new RelayCommand<object>(ConfigurarServidor);
        }

        private void AniadirProducto(object parameter)
        {
            AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
            AniadirProducto _AniadirProducto = new AniadirProducto(_viewModel);
            _AniadirProducto.Show();
        }
        private void VerExportarProductos(object parameter)
        {
            if(this.VistaActual is ExportarProductos)
            {
                this.VistaActual = null;
            }
            else
            {
                ExportarProductosViewModel _viewModel = new ExportarProductosViewModel();
                ExportarProductos _vista = new ExportarProductos(_viewModel);
                this.VistaActual = _vista;
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
                this.VistaActual = null;
            }
            else
            {
                ConfigurarSQLServerViewModel viewModel = new ConfigurarSQLServerViewModel();
                ConfigurarSQLServer vista = new ConfigurarSQLServer(viewModel);
                this.VistaActual = vista;
            }
        }
        private void VerCatalogo(object parameter)
        {
            if (this.VistaActual is Catalogo)
            {
                this.VistaActual = null;
            }
            else
            {
                CatalogoViewModel _viewModel = new CatalogoViewModel();
                Catalogo vista = new Catalogo(_viewModel);
                this.VistaActual = vista;
            }
        }
        private void AniadirPersona(object parameter)
        {
            AniadirPersona _AniadirPersona = new AniadirPersona();
            _AniadirPersona.Show();
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
                    System.Windows.MessageBox.Show("Debe ingresar un numero.", "Error", MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
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
                    System.Windows.MessageBox.Show("No existe un registro con ese ID.", "Error", MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }
                
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
