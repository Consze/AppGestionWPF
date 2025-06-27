using System.ComponentModel;

namespace WPFApp1
{
    public class Productos : INotifyPropertyChanged
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }
        private string _categoria;
        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; OnPropertyChanged(nameof(Categoria)); }
        }
        private int _precio;
        public int Precio 
        {
            get { return _precio; }
            set { _precio = value; OnPropertyChanged(nameof(Precio)); }
        }

        private string _rutaImagen;
        public string RutaImagen
        {
            get { return _rutaImagen; }
            set { _rutaImagen = value; OnPropertyChanged(nameof(RutaImagen)); }
        }

        public Productos(int ProductoID, string NombreDeProducto, string CategoriaDeProducto, int PrecioDeProducto, string RutaAImagen)
        {
            this.ID = ProductoID;
            this.Nombre = NombreDeProducto;
            this.Categoria = CategoriaDeProducto;
            this.Precio = PrecioDeProducto ;
            this.RutaImagen = RutaAImagen;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FlagsCambiosProductos
    {
        public bool NombreCambiado { get; set; }
        public bool CategoriaCambiada { get; set; }
        public bool PrecioCambiado { get; set; }
        public bool RutaImagenCambiada { get; set; }
        public int ContadorCambios { get; set; }

        public FlagsCambiosProductos()
        {
            this.NombreCambiado = false;
            this.CategoriaCambiada = false;
            this.PrecioCambiado = false;
            this.PrecioCambiado = false;
            this.RutaImagenCambiada = false;
            this.ContadorCambios = 0;
        }
    }
}
