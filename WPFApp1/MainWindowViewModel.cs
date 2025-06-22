using System.Windows;
using System.Windows.Input;

namespace WPFApp1
{
    public class MainWindowViewModel 
    {
        public ICommand VerListaCommand { get; }
        public ICommand AniadirPersonaCommand { get; }
        public ICommand EliminarPersonaCommand { get; }
        public ICommand EditarPersonaCommand { get; }
        public ICommand VerCatalogoCommand{ get; }
        public ICommand AniadirProductoCommand { get; }

        public MainWindowViewModel()
        {
            VerListaCommand = new RelayCommand<object>(VerLista);
            AniadirPersonaCommand = new RelayCommand<object>(AniadirPersona);
            EliminarPersonaCommand = new RelayCommand<object>(EliminarPersona);
            EditarPersonaCommand = new RelayCommand<object>(EditarPersona);
            VerCatalogoCommand = new RelayCommand<object>(VerCatalogo);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
        }

        private void AniadirProducto(object parameter)
        {
            AniadirProductoViewModel _viewModel = new AniadirProductoViewModel();
            AniadirProducto _AniadirProducto = new AniadirProducto(_viewModel);
            _AniadirProducto.Show();
        }
        private void VerLista(object parameter)
        {
            ListaPersonas lista = new ListaPersonas();
            lista.Show();
        }

        private void VerCatalogo(object parameter)
        {
            CatalogoViewModel catalogoViewModel = new CatalogoViewModel();
            Catalogo _catalogo = new Catalogo(catalogoViewModel);
            _catalogo.Show();
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
    }
}
