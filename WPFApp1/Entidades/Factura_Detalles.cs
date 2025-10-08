namespace WPFApp1.Entidades
{
    public class Factura_Detalles : EntidadBase
    {
        private string _facturaID;
        public string FacturaID
        {
            get => _facturaID;
            set
            {
                if (_facturaID != value)
                {
                    _facturaID = value;
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
        public decimal PrecioVenta
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
    }
}
