using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Org.BouncyCastle.Bcpg;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.ViewModels
{
    public class NuevoValorCategoriaViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        public bool MostrarError
        {
            get { return !ValidarInput; }
        }
        private string _categoriaNombre;
        public string CategoriaNombre
        {
            get { return _categoriaNombre; }
            set
            {
                if (_categoriaNombre != value)
                {
                    _categoriaNombre = value;
                    OnPropertyChanged(nameof(CategoriaNombre));
                    OnPropertyChanged(nameof(ValidarInput));
                    OnPropertyChanged(nameof(MostrarError));

                    if (ToggleEdicionCategoria)
                    {
                        Categorias _registro = Coleccion.FirstOrDefault(u => u.Nombre == value);
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
        private Categorias _entidadElegida;
        public Categorias EntidadElegida
        {
            get { return _entidadElegida; }
            set
            {
                if (_entidadElegida != value)
                {
                    _entidadElegida = value;
                    CategoriaNombre = value.Nombre;
                    OnPropertyChanged(nameof(EntidadElegida));
                    OnPropertyChanged(nameof(ValidarInput));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
                }
            }
        }
        private bool _ToggleCheckInsertarCategoria;
        public bool ToggleCheckInsertarCategoria
        {
            get { return _ToggleCheckInsertarCategoria; }
            set
            {
                if (_ToggleCheckInsertarCategoria != value)
                {
                    _ToggleCheckInsertarCategoria = value;
                    OnPropertyChanged(nameof(ToggleCheckInsertarCategoria));
                }
            }
        }
        private bool _ToggleEdicionCategoria;
        public bool ToggleEdicionCategoria
        {
            get { return _ToggleEdicionCategoria; }
            set
            {
                if (_ToggleEdicionCategoria != value)
                {
                    _ToggleEdicionCategoria = value;
                    OnPropertyChanged(nameof(ToggleEdicionCategoria));
                }
            }
        }
        private bool _ToggleSeleccionCategoria;
        public bool ToggleSeleccionCategoria
        {
            get { return _ToggleSeleccionCategoria; }
            set
            {
                if (_ToggleSeleccionCategoria != value)
                {
                    _ToggleSeleccionCategoria = value;
                    OnPropertyChanged(nameof(ToggleSeleccionCategoria));
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
        public ObservableCollection<Categorias> Coleccion { get; set; }
        public bool ValidarInput
        {
            get
            {
                if (string.IsNullOrEmpty(CategoriaNombre))
                    return false;

                return true;
            }
        }
        public object InputUsuario
        {
            get { return EntidadElegida?.ID; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private IConmutadorEntidadGenerica<Categorias> ServicioCategorias;
        public ICommand InsertarNuevaCategoriaCommand { get; }
        public ICommand EditarCategoriaCommand { get; }
        public NuevoValorCategoriaViewModel(IConmutadorEntidadGenerica<Categorias> _servicio)
        {
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleCheckInsertarCategoria = false;
            ToggleEdicionCategoria = false;
            ToggleSeleccionCategoria = true;
            Coleccion = new ObservableCollection<Categorias>();
            ServicioCategorias = _servicio;
            _entidadElegida = null;
            EditarCategoriaCommand = new RelayCommand<object>(EditarCategoria);
            InsertarNuevaCategoriaCommand = new RelayCommand<object>(InsertarNuevaCategoria);
            IniciarColeccionWrapper();
        }
        private void EditarCategoria(object parameter)
        {
            ToggleEdicionCategoria = !ToggleEdicionCategoria;
            ToggleSeleccionCategoria = !ToggleSeleccionCategoria;
            if (!ToggleEdicionCategoria)
            {
                iconoSeleccionRapida = "/iconos/lapizEdicion.png";
                ToggleCheckInsertarCategoria = false;
            }
            else
            {
                iconoSeleccionRapida = "/iconos/lista1.png";
                ToggleCheckInsertarCategoria = true;
            }
        }
        private void InsertarNuevaCategoria(object parameter)
        {
            if (string.IsNullOrEmpty(CategoriaNombre))
            {
                Notificacion operacionCancelada = new Notificacion { Mensaje = "No se puede registrar una Categoría sin nombre!", Titulo = "Operación Cancelada", IconoRuta = IconoNotificacion.SUSPENSO1, Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = operacionCancelada });
                return;
            }
                

            Categorias item = new Categorias
            {
                Nombre = CategoriaNombre
            };
            item.ID = ServicioCategorias.Insertar(item);
            Categorias _registro = Coleccion.FirstOrDefault(u => u.ID == item.ID);

            if (_registro == null)
            {
                Coleccion.Add(item);
                EntidadElegida = item;
            }
            else
            {
                EntidadElegida = _registro;
            }

            ToggleCheckInsertarCategoria = false;
            iconoSeleccionRapida = "/iconos/lapizEdicion.png";
            ToggleEdicionCategoria = false;
            ToggleSeleccionCategoria = true;

            //_servicioSFX.Confirmar();
            Notificacion _notificacion = new Notificacion { Mensaje = "Nueva categoría registrada!", Titulo = "Operación Completada", IconoRuta = IconoNotificacion.OK, Urgencia = MatrizEisenhower.C1 };
            Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
        }
        private async Task IniciarColeccion()
        {
            Coleccion.Clear();

            await foreach (var entidad in ServicioCategorias.RecuperarStreamAsync())
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
