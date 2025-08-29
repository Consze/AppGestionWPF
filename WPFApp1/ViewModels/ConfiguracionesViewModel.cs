using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
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
        public ICommand IniciarServidorCommand { get; }
        private ServicioSFX _servicioSFX { get; set; }
        public ConfiguracionesViewModel()
        {
            CrearNotificacionCommand = new RelayCommand<object>(CrearNotificacion);
            IniciarServidorCommand = new RelayCommand<object>(IniciarServidor);
            _servicioSFX = new ServicioSFX();
        }
        public void CrearNotificacion(object parameter)
        {
            _servicioSFX.Swipe();
            Notificacion _notificacionPrueba = new Notificacion { Mensaje = "Este es el cuerpo de la notificación de prueba", Titulo = "Notificación", IconoRuta = Path.GetFullPath(IconoNotificacion.NOTIFICACION), Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacionPrueba });
        }
        public void IniciarServidor(object parameter)
        {
            return;
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string rutaWebHost = Path.Combine(baseDir, "..", "WebHost", "WebHost.exe");

                if (File.Exists(rutaWebHost))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = rutaWebHost,
                        WorkingDirectory = Path.GetDirectoryName(rutaWebHost),
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process webHostProcess = Process.Start(startInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar el Servidor: {ex.Message}");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
