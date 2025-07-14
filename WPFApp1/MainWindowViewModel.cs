using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
            ConfigurarServidorCommand = new RelayCommand<object>(async (param) => await ConfigurarServidorAsync());
        }

        private void AniadirProducto(object parameter)
        {
            AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
            AniadirProducto _AniadirProducto = new AniadirProducto(_viewModel);
            _AniadirProducto.Show();
        }
        private void VerExportarProductos(object parameter)
        {
            if (ExportarProductos.Instancias < 1) // Limitar cantidad de Instancias de vista
            {
                ExportarProductosViewModel ExportarViewModel = new ExportarProductosViewModel();
                ExportarProductos VistaExportar = new ExportarProductos(ExportarViewModel);
                VistaExportar.Show();
            }
            else
            {
                ExportarProductos.VentanaExportarProductosVigente.Activate();
            }
        }
        private void VerLista(object parameter)
        {
            ListaPersonas lista = new ListaPersonas();
            lista.Show();
        }
        private async Task ConfigurarServidorAsync()
        {
            InputUsuarioViewModel viewModel = new InputUsuarioViewModel("Ingrese el nombre de instancia de SQLServer:");
            InputUsuario dialogo = new InputUsuario(viewModel);
            bool? resultado = dialogo.ShowDialog();

            InputUsuarioViewModel viewModel2 = new InputUsuarioViewModel("Ingrese el nombre de la base de datos:");
            InputUsuario dialogo2 = new InputUsuario(viewModel2);
            bool? resultado2 = dialogo2.ShowDialog();

            if (resultado == true && resultado2 == true)
            {
                this.Procesando = true;
                string nombreServidor = viewModel.Entrada;
                string nombreDB = viewModel2.Entrada;
                string cadenaConexion = $"Server={Environment.MachineName}\\{nombreServidor};Database={nombreDB};Integrated Security=True;";
                ConexionDBSQLServer _configuracionServidor = new ConexionDBSQLServer();
                _configuracionServidor.CadenaConexion = cadenaConexion;
                bool conexionExitosa = await Task.Run(() => _configuracionServidor.ProbarConexion(cadenaConexion));
                _configuracionServidor.ConexionValida = conexionExitosa;
                await Task.Run(() => _configuracionServidor.GuardarEstadoConexion());
                this.Procesando = false;
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
                System.Windows.MessageBox.Show("No se ingresaron datos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void VerCatalogo(object parameter)
        {
            if (Catalogo.Instancias < 1)
            {
                CatalogoViewModel catalogoViewModel = new CatalogoViewModel();
                Catalogo _catalogo = new Catalogo(catalogoViewModel);
                _catalogo.Show();
            }
            else
            {
                Catalogo.VentanaCatalogoVigente.Activate();
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
