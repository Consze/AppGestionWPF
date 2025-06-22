namespace WPFApp1
{
    public class Productos
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int Precio { get; set; }
        public string RutaImagen { get; set; }

        public Productos()
        {
            this.Nombre = "";
            this.Categoria = "";
            this.Precio = 0 ;
            this.RutaImagen = "";
        }
    }
}
