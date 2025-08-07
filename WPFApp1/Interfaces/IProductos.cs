using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IProductosAccesoDatos
    {
        bool ActualizarProducto(Productos producto);
        int CrearProducto(Productos producto);
        Productos RecuperarProductoPorID(int producto_id);
        bool EliminarProducto(int producto_id);
        List<Productos> LeerProductos();
        bool CrearLibro(List<Productos> Productos);
    }
}
