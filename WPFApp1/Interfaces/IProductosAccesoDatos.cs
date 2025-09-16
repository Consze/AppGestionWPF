using WPFApp1.Entidades;
using WPFApp1.Repositorios;

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
    }
}
