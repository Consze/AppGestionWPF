using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPFApp1.Conmutadores;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Repositorios;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class ExportarProductosViewModel : INotifyPropertyChanged
    {
        private bool _procesando;
        public bool Procesando
        {
            get { return _procesando; } 
            set
            {
                if (_procesando != value) {
                    _procesando = value;
                    OnPropertyChanged(nameof(Procesando));
                }
            }
        }
        public ICommand ExportarXLSXCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ProductoConmutador _productoServicio;
        //constructor
        public ExportarProductosViewModel(ProductoConmutador productoServicio)
        {
            _productoServicio = productoServicio;
            ExportarXLSXCommand = new RelayCommand<object>(async (param) => await ExportarXLSX());
            _procesando = false;
        }
        public async Task ExportarXLSX()
        {
            await ExportarXLSXAsync().ConfigureAwait(false);
        }
        public async Task ExportarXLSXAsync()
        {
            /**
            _servicioSFX.Swipe();
            Notificacion _notificacion = new Notificacion { Mensaje = "Exportando catalogo...", Titulo = "Procesando", IconoRuta = Path.GetFullPath(IconoNotificacion.NOTIFICACION), Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            if (!_procesando) 
            {
                Procesando = true;
                try { 
                    List <Productos> Productos = await Task.Run(() => _productoServicio.LeerProductos());
                    bool resultado = await Task.Run(() => _productoServicio.CrearLibro(Productos));
                    Procesando = false;
                    string CuerpoNotificacion = string.Empty;
                    string TituloNotificacion = string.Empty;
                    string IconoAUtilizar = string.Empty;
                    if (resultado)
                    {
                        TituloNotificacion = "Operación Completada";
                        CuerpoNotificacion = "Catalogo exportado correctamente!";
                        IconoAUtilizar = Path.GetFullPath(IconoNotificacion.OK);
                    }
                    else
                    {
                        TituloNotificacion = "Operación Completada";
                        CuerpoNotificacion = "Hubo un error al exportar";
                        IconoAUtilizar = Path.GetFullPath(IconoNotificacion.SUSPENSO1);
                    }

                    Notificacion resultadoExportacion = new Notificacion { Mensaje = CuerpoNotificacion, Titulo = TituloNotificacion, IconoRuta = IconoAUtilizar, Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = resultadoExportacion });
                }
                finally
                {
                    Procesando = false;
                }
            }
            else
            {
                Console.WriteLine("Ya hay una exportación en proceso");
            }
            */
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
