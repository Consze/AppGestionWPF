using WPFApp1.Entidades;
using WPFApp1.Repositorios;
using WPFApp1.Interfaces;

namespace WPFApp1.Conmutadores
{
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
                await foreach (var formato in repoServer.RecuperarStreamAsync())
                {
                    yield return formato;
                }
            }
            else
            {
                await foreach (var formato in repoLocal.RecuperarStreamAsync())
                {
                    yield return formato;
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
        public bool Eliminar(string ID, TipoEliminacion Caso) => throw new NotImplementedException();
        public bool Modificar(Formatos registroModificado) => throw new NotImplementedException();
        public string Insertar(Formatos nuevoRegistro) => throw new NotImplementedException();
        public Formatos Recuperar(string ID) => throw new NotImplementedException();
    }
}
