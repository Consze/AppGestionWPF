namespace WPFApp1
{
    public class Productos
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int Precio { get; set; }
        public string RutaImagen { get; set; }

        public Productos(int ProductoID, string NombreDeProducto, string CategoriaDeProducto, int PrecioDeProducto, string RutaAImagen)
        {
            this.ID = ProductoID;
            this.Nombre = NombreDeProducto;
            this.Categoria = CategoriaDeProducto;
            this.Precio = PrecioDeProducto ;
            this.RutaImagen = RutaAImagen;
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
