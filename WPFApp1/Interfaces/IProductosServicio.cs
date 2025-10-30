using WPFApp1.DTOS;
using WPFApp1.Enums;
using WPFApp1.Repositorios;

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
        bool ModificacionMasiva(List<ProductoEditar_Propiedad_Valor> lista);
        List<ProductoCatalogo> RecuperarLotePorIDS(string propiedadNombre, List<string> IDs);
        List<ProductoCatalogo> RecuperarLotePorPropiedades(List<Propiedad_Valor> Busqueda);
    }
}
