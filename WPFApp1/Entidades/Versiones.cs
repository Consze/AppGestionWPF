namespace WPFApp1.Entidades
{
    public class Versiones : EntidadBase
    {
        private string _productoID;
        public string ProductoID
        {
            get => _productoID;
            set
            {
                if (_productoID != value)
                {
                    _productoID = value;
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
        private string _formatoID;
        public string FormatoID
        {
            get => _formatoID;
            set
            {
                if (_formatoID != value)
                {
                    _formatoID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _rutaRelativaImagen;
        public string RutaRelativaImagen
        {
            get => _rutaRelativaImagen;
            set
            {
                if (_rutaRelativaImagen != value)
                {
                    _rutaRelativaImagen = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
