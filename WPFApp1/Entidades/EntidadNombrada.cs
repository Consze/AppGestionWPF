namespace WPFApp1.Entidades
{
    public class EntidadNombrada : EntidadBase
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
    }
}
