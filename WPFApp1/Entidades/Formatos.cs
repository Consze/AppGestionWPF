namespace WPFApp1.Entidades
{
    public class Formatos : EntidadNombrada
    {
        private decimal _alto;
        public decimal Alto
        {
            get => _alto;
            set
            {
                if (_alto != value)
                {
                    _alto = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _largo;
        public decimal Largo
        {
            get => _largo;
            set
            {
                if (_largo != value)
                {
                    _largo = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _profundidad;
        public decimal Profundidad
        {
            get => _profundidad;
            set
            {
                if (_profundidad != value)
                {
                    _profundidad = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _peso;
        public decimal Peso
        {
            get => _peso;
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
