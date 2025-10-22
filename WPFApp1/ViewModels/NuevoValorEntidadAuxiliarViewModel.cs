using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.ViewModels
{
    public class NuevoValorEntidadAuxiliarViewModel : INotifyPropertyChanged, IControlValidadoVM
    {
        private string _valorString;
        private string _textoError;
        private readonly string nombrePropiedad;
        public ObservableCollection<EntidadNombrada> Coleccion { get; set; }
        //TODO IMPLEMENTACION
        public bool ValidarInput
        {
            get;set;
        }
        public object InputUsuario
        {
            get;set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public NuevoValorEntidadAuxiliarViewModel(string _nombrePropiedad)
        {
            Coleccion = new ObservableCollection<EntidadNombrada>();
            nombrePropiedad = _nombrePropiedad;
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
