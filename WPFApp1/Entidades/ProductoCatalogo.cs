namespace WPFApp1.Entidades
{
    public class ProductoCatalogo : ProductoBase
    {
        private bool _precioPublico;
        public bool PrecioPublico
        {
            get => _precioPublico;
            set
            {
                if (_precioPublico != value)
                {
                    _precioPublico = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _visibilidadWeb;
        public bool VisibilidadWeb
        {
            get => _visibilidadWeb;
            set
            {
                if (_visibilidadWeb != value)
                {
                    _visibilidadWeb = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _haber;
        public int Haber
        {
            get => _haber;
            set
            {
                if (_haber != value)
                {
                    _haber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _productoEdicionID;
        public string ProductoEdicionID
        {
            get => _productoEdicionID;
            set
            {
                if (_productoEdicionID != value)
                {
                    _productoEdicionID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ubicacionID;
        public string UbicacionID
        {
            get => _ubicacionID;
            set
            {
                if (_ubicacionID != value)
                {
                    _ubicacionID = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _peso;
        public int Peso
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
        private int _altura;
        public int Altura
        {
            get => _altura;
            set
            {
                if (_altura != value)
                {
                    _altura = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _largo;
        public int Largo
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
        private int _ancho;
        public int Ancho
        {
            get => _ancho;
            set
            {
                if (_ancho != value)
                {
                    _ancho = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ean;
        public string EAN
        {
            get => _ean;
            set
            {
                if (_ean != value)
                {
                    _ean = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _marcaID;
        public string MarcaID
        {
            get => _marcaID;
            set
            {
                if (_marcaID != value)
                {
                    _marcaID = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
