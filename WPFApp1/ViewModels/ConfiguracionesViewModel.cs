using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    
    public class ConfiguracionesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand CrearNotificacionCommand { get; }
        public ConfiguracionesViewModel()
        {
            CrearNotificacionCommand = new RelayCommand<object>(CrearNotificacion);
        }
        public void CrearNotificacion(object parameter)
        {
            Notificacion _notificacionPrueba = new Notificacion { Mensaje = "Este es el cuerpo de la notificación de prueba", Titulo = "Notificación", Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacionPrueba });
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
