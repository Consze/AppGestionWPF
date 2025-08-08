namespace WPFApp1.Entidades
{
    public class Libro : ProductoBase
    {
        public string Categoria_id { get; set; }
        public string Sinopsis { get; set; }
        public bool EsEliminado { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string ID { get; set; }
        public Libro() { }
    }
}
