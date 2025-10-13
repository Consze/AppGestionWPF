namespace WPFApp1.Entidades
{
    public class Factura : EntidadBase
    {
        private string _sucursalID;
        public string SucursalID
        {
            get => _sucursalID;
            set
            {
                if (_sucursalID != value)
                {
                    _sucursalID = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _fechaVenta;
        public DateTime FechaVenta
        {
            get => _fechaVenta;
            set
            {
                if (_fechaVenta != value)
                {
                    _fechaVenta = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
