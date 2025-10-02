using WPFApp1.DTOS;
using WPFApp1.Enums;

namespace WPFApp1.Interfaces
{
    public interface IProductosServicio
    {
        ProductoCatalogo RecuperarProductoPorID(string ProductoID);
        string CrearProducto(ProductoCatalogo nuevoProducto);
        IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync();
        List<ProductoCatalogo> LeerProductos();
        bool ModificarProducto(ProductoCatalogo productoModificado);
        bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion);
    }
}
