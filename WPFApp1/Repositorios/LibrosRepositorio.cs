using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class LibrosAccesoSQLite : IAccesoDatosLibros
    {
        public string InsertarLibro(Libro LibroNuevo)
        {
            return "";
        }
        public bool EliminarLibro(string LibroEliminarID)
        {
            return false;
        }
        public bool ModificarLibro(Libro LibroModificado)
        {
            return false;
        }
        public List<Libro> RecuperarTodosLosLibros()
        {
            return new List<Libro>();
        }
        public Libro RecuperarLibroPorID(string LibroID)
        {
            return new Libro();
        }
    }

    public class LibrosAccesoSQLServer : IAccesoDatosLibros
    {
        public string InsertarLibro(Libro LibroNuevo)
        {
            return "";
        }
        public bool EliminarLibro(string LibroEliminarID)
        {
            return false;
        }
        public bool ModificarLibro(Libro LibroModificado)
        {
            return false;
        }
        public List<Libro> RecuperarTodosLosLibros()
        {
            return new List<Libro>();
        }
        public Libro RecuperarLibroPorID(string LibroID)
        {
            return new Libro();
        }
    }
}
