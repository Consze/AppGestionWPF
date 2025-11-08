namespace WPFApp1.DTOS
{
    public class ProductoCatalogo : ProductoBase
    {
        //--------Para uso en Vistas----
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
        private string _displayPropiedadActiva;
        public string DisplayPropiedadActiva
        {
            get => _displayPropiedadActiva;
            set
            {
                if (_displayPropiedadActiva != value)
                {
                    _displayPropiedadActiva = value;
                    OnPropertyChanged(nameof(DisplayPropiedadActiva));
                }
            }
        }


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


        //--------Numericos---------
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
        private string _condicionID;
        public string CondicionID
        {
            get => _condicionID;
            set
            {
                if (_condicionID != value)
                {
                    _condicionID = value;
                    OnPropertyChanged();
                }
            }
        }


        //Cadenas
        private string _condicionNombre;
        public string CondicionNombre
        {
            get => _condicionNombre;
            set
            {
                if (_condicionNombre != value)
                {
                    _condicionNombre = value;
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
        private string _formatoNombre;
        public string FormatoNombre
        {
            get => _formatoNombre;
            set
            {
                if (_formatoNombre != value)
                {
                    _formatoNombre = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ubicacionNombre;
        public string UbicacionNombre
        {
            get => _ubicacionNombre;
            set
            {
                if (_ubicacionNombre != value)
                {
                    _ubicacionNombre = value;
                    OnPropertyChanged();
                }
            }
        }

    }
}
