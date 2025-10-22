using System.ComponentModel;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class NuevoValorNumericoViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        private string _valorString;
        private string _textoError;
        private readonly string nombrePropiedad;
        public event PropertyChangedEventHandler PropertyChanged;
        public string ValorString
        {
            get => _valorString;
            set
            {
                if (_valorString != value)
                {
                    _valorString = value;
                    OnPropertyChanged(nameof(ValorString));
                    OnPropertyChanged(nameof(ValidarInput));
                    OnPropertyChanged(nameof(MostrarError));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = ValidarInput });
                }
            }
        }
        public bool MostrarError
        {
            get { return !ValidarInput; }
        }
        public bool ValidarInput
        {
            get
            {
                if (string.IsNullOrEmpty(ValorString))
                    return true;

                if (!decimal.TryParse(ValorString, out decimal valorNumerico))
                {
                    return false;
                }
                
                if (nombrePropiedad.Equals("Haber", StringComparison.OrdinalIgnoreCase))
                {
                    return valorNumerico > 0;
                }

                return valorNumerico >= 0;
            }
        }
        public object InputUsuario
        {
            get
            {
                if (ValidarInput && decimal.TryParse(ValorString, out decimal valorNumerico))
                {
                    return valorNumerico;
                }
                return null;
            }
        }
        public string TextoError
        {
            get => _textoError;
            set
            {
                if (_textoError != value)
                {
                    _textoError = value;
                    OnPropertyChanged(nameof(TextoError));
                }
            }
        }
        public NuevoValorNumericoViewModel(string _nombrePropiedad)
        {
            nombrePropiedad = _nombrePropiedad;

            switch (nombrePropiedad)
            {
                case "Haber":
                    TextoError = "Valor inválido (debe ser numerico y mayor que 0).";
                    break;
                case "Precio":
                    TextoError = "Valor inválido (debe ser numerico y como valor minimo 0).";
                    break;
            }   
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}