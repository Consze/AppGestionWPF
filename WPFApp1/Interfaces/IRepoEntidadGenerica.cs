using WPFApp1.Entidades;

namespace WPFApp1.Interfaces
{
    public interface IRepoEntidadGenerica<T> where T : EntidadBase
    {
        bool Eliminar(string ID);
        bool Modificar(T registroModificado);
        string Insertar(T nuevoRegistro);
        T Recuperar(string ID);
        IAsyncEnumerable<T> RecuperarStreamAsync();
        List<T> RecuperarList();
    }
}
