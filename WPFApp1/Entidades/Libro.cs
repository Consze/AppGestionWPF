namespace WPFApp1.Entidades
{
    public class Libro : ProductoBase
    {
        public string Categoria_id { get; set; }
        public string Sinopsis { get; set; }
        public Libro() { }
    }
}
