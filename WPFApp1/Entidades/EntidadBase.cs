using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFApp1.Entidades
{
    public class EntidadBase : INotifyPropertyChanged
    {
        private bool _esEliminado;
        public bool EsEliminado
        {
            get => _esEliminado;
            set
            {
                if (_esEliminado != value)
                {
                    _esEliminado = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _fechaModificacion;
        public DateTime FechaModificacion
        {
            get => _fechaModificacion;
            set
            {
                if (_fechaModificacion != value)
                {
                    _fechaModificacion = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _fechaCreacion;
        public DateTime FechaCreacion
        {
            get => _fechaCreacion;
            set
            {
                if (_fechaCreacion != value)
                {
                    _fechaCreacion = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _id;
        public string ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
