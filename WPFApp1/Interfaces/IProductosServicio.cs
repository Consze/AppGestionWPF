using WPFApp1.Entidades;
using WPFApp1.Repositorios;

namespace WPFApp1.Interfaces
{
    public interface IProductosServicio
    {
        ProductoCatalogo RecuperarProductoPorID(string ProductoID);
        bool CrearProducto(ProductoCatalogo nuevoProducto);
        IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync();
        List<ProductoCatalogo> LeerProductos();
        bool ModificarProducto(ProductoCatalogo productoModificado);
        bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion);
    }
}
