using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class NuevoValorUbicacionViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        private string _textoError;
        private string _ubicacionNombre;
        public string UbicacionNombre
        {
            get { return _ubicacionNombre; }
            set
            {
                if (_ubicacionNombre != value)
                {
                    _ubicacionNombre = value;
                    OnPropertyChanged(nameof(UbicacionNombre));

                    if(ToggleEdicionUbicacion)
                    {
                        Ubicaciones _registro = Coleccion.FirstOrDefault(u => u.Nombre == value);
                        if (_registro != null)
                        {
                            Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
                        }
                        else
                        {
                            Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = false });
                        }
                    }
                }
            }
        }
        private Ubicaciones _entidadElegida;
        public Ubicaciones EntidadElegida
        {
            get { return _entidadElegida; }
            set
            {
                if (_entidadElegida != value)
                {
                    _entidadElegida = value;
                    UbicacionNombre = value.Nombre;
                    OnPropertyChanged(nameof(EntidadElegida));
                    OnPropertyChanged(nameof(ValidarInput));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
                }
            }
        }
        private bool _ToggleCheckInsertarUbicacion;
        public bool ToggleCheckInsertarUbicacion
        {
            get { return _ToggleCheckInsertarUbicacion; }
            set
            {
                if (_ToggleCheckInsertarUbicacion != value)
                {
                    _ToggleCheckInsertarUbicacion = value;
                    OnPropertyChanged(nameof(ToggleCheckInsertarUbicacion));
                }
            }
        }
        private bool _ToggleEdicionUbicacion;
        public bool ToggleEdicionUbicacion
        {
            get { return _ToggleEdicionUbicacion; }
            set
            {
                if (_ToggleEdicionUbicacion != value)
                {
                    _ToggleEdicionUbicacion = value;
                    OnPropertyChanged(nameof(ToggleEdicionUbicacion));
                }
            }
        }
        private bool _ToggleSeleccionUbicacion;
        public bool ToggleSeleccionUbicacion
        {
            get { return _ToggleSeleccionUbicacion; }
            set
            {
                if (_ToggleSeleccionUbicacion != value)
                {
                    _ToggleSeleccionUbicacion = value;
                    OnPropertyChanged(nameof(ToggleSeleccionUbicacion));
                }
            }
        }
        private string _iconoSeleccionRapida;
        public string iconoSeleccionRapida
        {
            get { return _iconoSeleccionRapida; }
            set
            {
                if (_iconoSeleccionRapida != value)
                {
                    _iconoSeleccionRapida = value;
                    OnPropertyChanged(nameof(iconoSeleccionRapida));
                }
            }
        }
        public ObservableCollection<Ubicaciones> Coleccion { get; set; }
        public bool ValidarInput
        {
            get { return true; }
        }
        public object InputUsuario
        {
            get { return EntidadElegida?.ID; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IConmutadorEntidadGenerica<Ubicaciones> ServicioUbicaciones;
        public ICommand InsertarNuevaUbicacionCommand { get; }
        public ICommand EditarUbicacionCommand { get; }
        public NuevoValorUbicacionViewModel(IConmutadorEntidadGenerica<Ubicaciones> _servicio)
        {
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleCheckInsertarUbicacion = false;
            ToggleEdicionUbicacion = false;
            ToggleSeleccionUbicacion = true;
            Coleccion = new ObservableCollection<Ubicaciones>();
            ServicioUbicaciones = _servicio;
            _entidadElegida = null;
            EditarUbicacionCommand = new RelayCommand<object>(EditarUbicacion);
            InsertarNuevaUbicacionCommand = new RelayCommand<object>(InsertarNuevaUbicacion);
            IniciarColeccionWrapper();
        }
        private void EditarUbicacion(object parameter)
        {
            ToggleEdicionUbicacion = !ToggleEdicionUbicacion;
            ToggleSeleccionUbicacion = !ToggleSeleccionUbicacion;
            if(!ToggleEdicionUbicacion)
            {
                iconoSeleccionRapida = "/iconos/lapizEdicion.png";
                ToggleCheckInsertarUbicacion = false;
            }
            else
            {
                iconoSeleccionRapida = "/iconos/lista1.png";
                ToggleCheckInsertarUbicacion = true;
            }
        }
        private void InsertarNuevaUbicacion(object parameter)
        {
            Ubicaciones item = new Ubicaciones
            {
                Nombre = UbicacionNombre
            };
            if (EntidadElegida == null)
                EntidadElegida = item;
            
            EntidadElegida.ID = ServicioUbicaciones.Insertar(item);
            EntidadElegida.Nombre = item.Nombre;
            ToggleCheckInsertarUbicacion = false;
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleEdicionUbicacion = false;
            ToggleSeleccionUbicacion = true;

            //_servicioSFX.Confirmar();
            Notificacion _notificacion = new Notificacion { Mensaje = "Nueva ubicación registrada!", Titulo = "Operación Completada", IconoRuta = IconoNotificacion.OK, Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
        }
        private async Task IniciarColeccion()
        {
            Coleccion.Clear();

            await foreach (var entidad in ServicioUbicaciones.RecuperarStreamAsync())
            {
                Coleccion.Add(entidad);
            }   
        }
        private async void IniciarColeccionWrapper()
        {
            try
            {
                await IniciarColeccion();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar colección: {ex.Message}");
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
