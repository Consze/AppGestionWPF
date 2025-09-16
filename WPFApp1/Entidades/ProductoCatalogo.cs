namespace WPFApp1.Entidades
{
    public class ProductoCatalogo : ProductoBase
    {
        //--------Booleanas---------
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


        //--------Enteros---------
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


        //-----Claves Foraneas-----
        private string _productoVersionID;
        public string ProductoVersionID
        {
            get => _productoVersionID;
            set
            {
                if (_productoVersionID != value)
                {
                    _productoVersionID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _formatoProductoID;
        public string FormatoProductoID
        {
            get => _formatoProductoID;
            set
            {
                if (_formatoProductoID != value)
                {
                    _formatoProductoID = value;
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


        //Cadenas
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
        private string _marcaNombre;
        public string MarcaNombre
        {
            get => _marcaNombre;
            set
            {
                if (_marcaNombre != value)
                {
                    _marcaNombre = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _categoriaNombre;
        public string CategoriaNombre
        {
            get => _categoriaNombre;
            set
            {
                if (_categoriaNombre != value)
                {
                    _categoriaNombre = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
