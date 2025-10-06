namespace WPFApp1.Entidades
{
    public class Ventas : EntidadBase
    {
        private string _medioPagoID;
        public string MedioPagoID
        {
            get => _medioPagoID;
            set
            {
                if (_medioPagoID != value)
                {
                    _medioPagoID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _productoSKU;
        public string ProductoSKU
        {
            get => _productoSKU;
            set
            {
                if (_productoSKU != value)
                {
                    _productoSKU = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _precioVenta;
        public decimal precioVenta
        {
            get => _precioVenta;
            set
            {
                if (_precioVenta != value)
                {
                    _precioVenta = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _cantidad;
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged();
                }
            }
        }
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
    }
}
