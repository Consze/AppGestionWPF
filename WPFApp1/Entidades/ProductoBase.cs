namespace WPFApp1.Entidades
{
    public class ProductoBase
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string RutaImagen { get; set; }
        public int Precio { get; set; }
        public string ID { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public ProductoBase() { }
    }
}
