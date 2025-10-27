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
    public class NuevoValorMarcaViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        public bool MostrarError
        {
            get { return !ValidarInput; }
        }
        private string _marcaNombre;
        public string MarcaNombre
        {
            get { return _marcaNombre; }
            set
            {
                if (_marcaNombre != value)
                {
                    _marcaNombre = value;
                    OnPropertyChanged(nameof(MarcaNombre));
                    OnPropertyChanged(nameof(ValidarInput));
                    OnPropertyChanged(nameof(MostrarError));

                    if (ToggleEdicionMarca)
                    {
                        Marcas _registro = Coleccion.FirstOrDefault(u => u.Nombre == value);
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
        private Marcas _entidadElegida;
        public Marcas EntidadElegida
        {
            get { return _entidadElegida; }
            set
            {
                if (_entidadElegida != value)
                {
                    _entidadElegida = value;
                    MarcaNombre = value.Nombre;
                    OnPropertyChanged(nameof(EntidadElegida));
                    OnPropertyChanged(nameof(ValidarInput));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
                }
            }
        }
        private bool _ToggleCheckInsertarMarca;
        public bool ToggleCheckInsertarMarca
        {
            get { return _ToggleCheckInsertarMarca; }
            set
            {
                if (_ToggleCheckInsertarMarca != value)
                {
                    _ToggleCheckInsertarMarca = value;
                    OnPropertyChanged(nameof(ToggleCheckInsertarMarca));
                }
            }
        }
        private bool _ToggleEdicionMarca;
        public bool ToggleEdicionMarca
        {
            get { return _ToggleEdicionMarca; }
            set
            {
                if (_ToggleEdicionMarca != value)
                {
                    _ToggleEdicionMarca = value;
                    OnPropertyChanged(nameof(ToggleEdicionMarca));
                }
            }
        }
        private bool _ToggleSeleccionMarca;
        public bool ToggleSeleccionMarca
        {
            get { return _ToggleSeleccionMarca; }
            set
            {
                if (_ToggleSeleccionMarca != value)
                {
                    _ToggleSeleccionMarca = value;
                    OnPropertyChanged(nameof(ToggleSeleccionMarca));
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
        public ObservableCollection<Marcas> Coleccion { get; set; }
        public bool ValidarInput
        {
            get
            {
                if (string.IsNullOrEmpty(MarcaNombre))
                    return false;

                return true;
            }
        }
        public object InputUsuario
        {
            get { return EntidadElegida?.ID; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IConmutadorEntidadGenerica<Marcas> ServicioMarcas;
        public ICommand InsertarNuevaMarcaCommand { get; }
        public ICommand EditarMarcaCommand { get; }
        public NuevoValorMarcaViewModel(IConmutadorEntidadGenerica<Marcas> _servicio)
        {
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleCheckInsertarMarca = false;
            ToggleEdicionMarca = false;
            ToggleSeleccionMarca = true;
            Coleccion = new ObservableCollection<Marcas>();
            ServicioMarcas = _servicio;
            _entidadElegida = null;
            EditarMarcaCommand = new RelayCommand<object>(EditarCategoria);
            InsertarNuevaMarcaCommand = new RelayCommand<object>(InsertarNuevaCategoria);
            IniciarColeccionWrapper();
        }
        private void EditarCategoria(object parameter)
        {
            ToggleEdicionMarca = !ToggleEdicionMarca;
            ToggleSeleccionMarca = !ToggleSeleccionMarca;
            if (!ToggleEdicionMarca)
            {
                iconoSeleccionRapida = "/iconos/lapizEdicion.png";
                ToggleCheckInsertarMarca = false;
            }
            else
            {
                iconoSeleccionRapida = "/iconos/lista1.png";
                ToggleCheckInsertarMarca = true;
            }
        }
        private void InsertarNuevaCategoria(object parameter)
        {
            if (string.IsNullOrEmpty(MarcaNombre))
            {
                Notificacion operacionCancelada = new Notificacion { Mensaje = "No se puede registrar una Marca sin nombre!", Titulo = "Operación Cancelada", IconoRuta = IconoNotificacion.SUSPENSO1, Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = operacionCancelada });
                return;
            }


            Marcas item = new Marcas
            {
                Nombre = MarcaNombre
            };
            item.ID = ServicioMarcas.Insertar(item);
            Marcas _registro = Coleccion.FirstOrDefault(u => u.ID == item.ID);

            if (_registro == null)
            {
                Coleccion.Add(item);
                EntidadElegida = item;
            }
            else
            {
                EntidadElegida = _registro;
            }

            ToggleCheckInsertarMarca = false;
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleEdicionMarca = false;
            ToggleSeleccionMarca = true;

            Notificacion _notificacion = new Notificacion { Mensaje = "Nueva marca registrada!", Titulo = "Operación Completada", IconoRuta = IconoNotificacion.OK, Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
        }
        private async Task IniciarColeccion()
        {
            Coleccion.Clear();

            await foreach (var entidad in ServicioMarcas.RecuperarStreamAsync())
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
