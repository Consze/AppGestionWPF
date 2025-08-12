using System.Collections.ObjectModel;

namespace WPFApp1.Entidades
{
    public class Libro : ProductoBase
    {
        public string Genero { get; set; }
        public string Autor { get; set; }
        public string Edicion_ID { get; set; }
        public string Sinopsis { get; set; }
        public ObservableCollection<Libro_ediciones> Ediciones { get; set; }
        public Libro() 
        {
            Ediciones = new ObservableCollection<Libro_ediciones>();
        }
    }
}
