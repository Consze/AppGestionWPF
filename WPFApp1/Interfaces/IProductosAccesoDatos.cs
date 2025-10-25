using WPFApp1.DTOS;
using WPFApp1.Enums;

namespace WPFApp1.Interfaces
{
    public interface IProductosAccesoDatos
    {
        ProductoCatalogo RecuperarProductoPorID(string ProductoID);
        string CrearProducto(ProductoCatalogo nuevoProducto);
        IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync();
        List<ProductoCatalogo> LeerProductos();
        bool ModificarProducto(ProductoCatalogo productoModificado);
        bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion);
        bool ModificacionMasiva(List<ProductoEditar_Propiedad_Valor> lista);
        List<ProductoCatalogo> RecuperarLotePorID(List<string> SKUs);
    }
}
