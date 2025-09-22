using WPFApp1.Entidades;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public interface IConmutadorEntidadGenerica<T> where T : EntidadBase
    {
        bool Eliminar(string ID);
        bool Modificar(T registroModificado);
        string Insertar(T nuevoRegistro);
        T Recuperar(string ID);
        IAsyncEnumerable<T> RecuperarStreamAsync();
        List<T> RecuperarList();
    }
    public class ConmutadorFormatos : IConmutadorEntidadGenerica<Formatos>
    {
        private readonly RepoFormatosSQLite repoLocal;
        private readonly RepoFormatosSQLServer repoServer;
        public ConmutadorFormatos(RepoFormatosSQLite _repoLocal, RepoFormatosSQLServer _repoServer)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Formatos> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var producto in repoServer.RecuperarStreamAsync())
                {
                    yield return producto;
                }
            }
            else
            {
                await foreach (var producto in repoLocal.RecuperarStreamAsync())
                {
                    yield return producto;
                }
            }
        }
        public List<Formatos> RecuperarList()
        {
            if(repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.RecuperarList();
            }
            else
            {
                return repoLocal.RecuperarList();
            }
        }
        public bool Eliminar(string ID) => throw new NotImplementedException();
        public bool Modificar(Formatos registroModificado) => throw new NotImplementedException();
        public string Insertar(Formatos nuevoRegistro) => throw new NotImplementedException();
        public Formatos Recuperar(string ID) => throw new NotImplementedException();
    }
}
