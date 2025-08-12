
namespace WPFApp1.Entidades
{
    public class Libro_ediciones : EntidadBase
    {
        public string LibroID { get; set; }
        public string ISBN { get; set; }
        public string EditorialID { get; set; }
        public int CantidadPaginas { get; set; }
        public int AnioPublicacion { get; set; }
    }
}
