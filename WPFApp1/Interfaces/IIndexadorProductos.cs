using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IIndexadorProductos
    {
        List<PalabrasTitulosProductos> BuscarPalabra(string Palabra);
        bool InsertarRegistro(string Palabra, int ID);
    }
}
