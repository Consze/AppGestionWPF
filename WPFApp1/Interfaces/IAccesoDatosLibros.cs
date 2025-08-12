using WPFApp1.Entidades;

namespace WPFApp1.Interfaces
{
    public interface IAccesoDatosLibros
    {
        string InsertarLibro(Libro nuevoLibro);
        bool EliminarLibro(string libroID);
        bool ModificarLibro(Libro libroModificado);
        List<Libro> RecuperarTodosLosLibros();
        Libro RecuperarLibroPorID(string libroID);
    }
}
