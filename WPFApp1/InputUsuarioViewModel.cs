using System.ComponentModel;
using System.Windows.Input;

namespace WPFApp1
{
    public class InputUsuarioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<bool> DialogoCerrado;
        public string TituloHint { get; }
        public string EntradaTextoUsuario
        {
            get { return _entradaTextoUsuario; }
            set
            {
                _entradaTextoUsuario = value;
                OnPropertyChanged(nameof(EntradaTextoUsuario));
            }
        }
        private string _entradaTextoUsuario;
        public string Entrada { get; private set; }
        public ICommand AceptarEntradaCommand { get; private set; }
        public ICommand CancelarEntradaCommand { get; private set; }
        public InputUsuarioViewModel(string tituloHint) 
        {
            this.TituloHint = tituloHint;
            AceptarEntradaCommand = new RelayCommand<object>(PresentarEntrada);
            CancelarEntradaCommand = new RelayCommand<object>(PresentarEntrada);
        }

        public void CancelarEntrada(object parameter)
        {
            this.Entrada = null;
            CerrarVista(false);
        }
        public void PresentarEntrada(object parameter)
        {
            this.Entrada = EntradaTextoUsuario;
            CerrarVista(true);
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
