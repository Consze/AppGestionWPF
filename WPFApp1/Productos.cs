namespace WPFApp1
{
    public class Productos
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int Precio { get; set; }
        public string RutaImagen { get; set; }

        public Productos(string NombreDeProducto, string CategoriaDeProducto, int PrecioDeProducto, string RutaAImagen)
        {
            this.Nombre = NombreDeProducto;
            this.Categoria = CategoriaDeProducto;
            this.Precio = PrecioDeProducto ;
            this.RutaImagen = RutaAImagen;
        }
    }
}
