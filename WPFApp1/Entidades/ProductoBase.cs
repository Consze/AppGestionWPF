using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFApp1.Entidades
{
    public class ProductoBase : INotifyPropertyChanged
    {
        private string _nombre;
        public string Nombre {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _categoria;
        public string Categoria {
            get => _categoria;
            set
            {
                if (_categoria != value)
                {
                    _categoria = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _rutaImagen;
        public string RutaImagen {
            get => _rutaImagen;
            set
            {
                if (_rutaImagen != value)
                {
                    _rutaImagen = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _precio;
        public int Precio {
            get => _precio;
            set
            {
                if (_precio != value)
                {
                    _precio = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _id;
        public string ID {
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
        private bool _esEliminado;
        public bool EsEliminado {
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
        public DateTime FechaModificacion {
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
        public DateTime FechaCreacion {
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
        public ProductoBase() { }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
