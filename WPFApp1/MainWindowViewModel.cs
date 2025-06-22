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

        public MainWindowViewModel()
        {
            VerListaCommand = new RelayCommand(VerLista);
            AniadirPersonaCommand = new RelayCommand(AniadirPersona);
            EliminarPersonaCommand = new RelayCommand(EliminarPersona);
            EditarPersonaCommand = new RelayCommand(EditarPersona);
            VerCatalogoCommand = new RelayCommand(VerCatalogo);
        }

        private void VerLista()
        {
            ListaPersonas lista = new ListaPersonas();
            lista.Show();
        }

        private void VerCatalogo()
        {
            CatalogoViewModel catalogoViewModel = new CatalogoViewModel();
            Catalogo _catalogo = new Catalogo(catalogoViewModel);
            _catalogo.Show();
        }
        private void AniadirPersona()
        {
            AniadirPersona _AniadirPersona = new AniadirPersona();
            _AniadirPersona.Show();
        }

        private void EliminarPersona()
        {
            EliminarPersona _EliminarPersona = new EliminarPersona();
            _EliminarPersona.Show();
        }

        private void EditarPersona()
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
