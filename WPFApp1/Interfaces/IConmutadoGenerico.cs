using WPFApp1.Entidades;
using WPFApp1.Repositorios;

namespace WPFApp1.Interfaces
{
    public interface IConmutadorEntidadGenerica<T> where T : EntidadBase
    {
        bool Eliminar(string ID, TipoEliminacion Caso);
        bool Modificar(T registroModificado);
        string Insertar(T nuevoRegistro);
        T Recuperar(string ID);
        IAsyncEnumerable<T> RecuperarStreamAsync();
        List<T> RecuperarList();
    }
}
