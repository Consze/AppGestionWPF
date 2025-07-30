using WPFApp1.DTOS;

namespace WPFApp1.Interfaces
{
    public interface IIndexadorProductos
    {
        List<Productos> BuscarTitulo(string Titulo);
    }
}
