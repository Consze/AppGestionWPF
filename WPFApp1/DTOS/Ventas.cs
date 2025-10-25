using WPFApp1.Entidades;

namespace WPFApp1.DTOS
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
        private ProductoCatalogo _itemVendido;
        public ProductoCatalogo ItemVendido
        {
            get => _itemVendido;
            set
            {
                if (_itemVendido != value)
                {
                    _itemVendido = value;
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
        private bool _modoEdicionActivo;
        public bool ModoEdicionActivo
        {
            get => _modoEdicionActivo;
            set
            {
                if (_modoEdicionActivo != value)
                {
                    _modoEdicionActivo = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ModoLecturaActivo));
                }
            }
        }
        public bool ModoLecturaActivo => !ModoEdicionActivo;
    }
}
