using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private Ubicaciones _entidadElegida;
        public Ubicaciones EntidadElegida
        {
            get { return _entidadElegida; }
            set
            {
                if (_entidadElegida != value)
                {
                    _entidadElegida = value;
                    OnPropertyChanged(nameof(EntidadElegida));
                    OnPropertyChanged(nameof(ValidarInput));
                    Messenger.Default.Publish(new ToggleHabilitarBotonEdicion { Estado = true });
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
        public NuevoValorUbicacionViewModel(IConmutadorEntidadGenerica<Ubicaciones> _servicio)
        {
            Coleccion = new ObservableCollection<Ubicaciones>();
            ServicioUbicaciones = _servicio;
            _entidadElegida = null;

            IniciarColeccionWrapper();
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
