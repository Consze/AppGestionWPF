using WPFApp1.Entidades;

namespace WPFApp1.Interfaces
{
    public interface IRepoEntidadGenerica
    {
        bool Eliminar(string ID);
        bool Modificar(EntidadBase registroModificado);
        string Insertar(EntidadBase nuevoRegistro);
        EntidadBase Recuperar(string ID);
        IAsyncEnumerable<EntidadBase> RecuperarStreamAsync();
    }
}
