using System.ComponentModel;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class NuevoValorBooleanoViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        private readonly string nombrePropiedad;
        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        private bool _inputBindingToggle;
        public bool InputBindingToggle
        {
            get { return _inputBindingToggle; }
            set
            {
                if (_inputBindingToggle != value)
                {
                    _inputBindingToggle = value;
                    OnPropertyChanged(nameof(InputBindingToggle));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
                }
            }
        }
        public bool ValidarInput { get { return true; } }
        public object InputUsuario
        {
            get { return InputBindingToggle; }
        }
        private string _textoCheckbox;
        public string TextoCheckbox
        {
            get { return _textoCheckbox; }
            set
            {
                if (_textoCheckbox != value)
                {
                    _textoCheckbox = value;
                    OnPropertyChanged(nameof(TextoCheckbox));
                }
            }
        }
        public NuevoValorBooleanoViewModel(string _nombrePropiedad) 
        {
            _inputBindingToggle = false;
            nombrePropiedad = _nombrePropiedad;

            switch (nombrePropiedad)
            {
                case "PrecioPublico":
                    TextoCheckbox = "Mostrar precio en catalogo";
                    break;
                case "VisibilidadWeb":
                    TextoCheckbox = "Mostrar producto en catalogo";
                    break;
                case "EsEliminado":
                    TextoCheckbox = "Eliminar producto";
                    break;
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
