namespace WPFApp1.Entidades
{
    public class Sucursal : EntidadNombrada
    {
        private string _localidad;
        public string Localidad
        {
            get => _localidad;
            set
            {
                if (_localidad != value)
                {
                    _localidad = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _calle;
        public string Calle
        {
            get => _calle;
            set
            {
                if (_calle != value)
                {
                    _calle = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _alturaCalle;
        public int alturaCalle
        {
            get => _alturaCalle;
            set
            {
                if (_alturaCalle != value)
                {
                    _alturaCalle = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _telefono;
        public string Telefono
        {
            get => _telefono;
            set
            {
                if (_telefono != value)
                {
                    _telefono = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _latitud;
        public decimal Latitud
        {
            get => _latitud;
            set
            {
                if (_latitud != value)
                {
                    _latitud = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _longitud;
        public decimal Longitud
        {
            get => _longitud;
            set
            {
                if (_longitud != value)
                {
                    _longitud = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
