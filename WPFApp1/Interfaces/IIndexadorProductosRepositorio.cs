using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IIndexadorProductosRepositorio
    {
        List<PalabrasTitulosProductos> BuscarPalabra(string Palabra);
        bool InsertarRegistro(string Palabra, int ID);
        List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(int producto_id);
        bool EliminarIndicesPorID(List<int> indicesID);
    }
}
