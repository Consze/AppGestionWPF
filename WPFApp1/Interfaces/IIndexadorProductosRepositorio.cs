using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IIndexadorProductosRepositorio
    {
        List<PalabrasTitulosProductos> BuscarPalabra(string Palabra);
        bool InsertarRegistro(string Palabra, string ID);
        List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(string producto_id);
        bool EliminarIndicesPorID(List<int> indicesID);
    }
}
