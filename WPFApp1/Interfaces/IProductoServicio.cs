using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IProductoServicio
    {
        bool ActualizarProducto(Productos producto);
        string CrearProducto(Productos producto);
        Productos RecuperarProductoPorID(string producto_id);
        bool EliminarProducto(string producto_id);
        List<Productos> LeerProductos();
        IAsyncEnumerable<Productos> LeerProductosAsync();
        bool CrearLibro(List<Productos> Productos);
    }
}
