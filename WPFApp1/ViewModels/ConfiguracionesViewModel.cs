using System.ComponentModel;
using System.Windows.Input;

namespace WPFApp1.ViewModels
{
    
    public class ConfiguracionesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CambiarEstadoServidorCommand { get; }
        public ConfiguracionesViewModel()
        {
            
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
