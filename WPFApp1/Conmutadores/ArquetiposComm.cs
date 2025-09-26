using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class ArquetiposConmutador : IConmutadorEntidadGenerica<Arquetipos>
    {
        private readonly RepoArquetiposSQLite repoLocal;
        private readonly RepoArquetiposSQLServer repoServer;
        public ArquetiposConmutador(RepoArquetiposSQLServer _repoServer, RepoArquetiposSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Arquetipos> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var arquetipo in repoServer.RecuperarStreamAsync())
                {
                    yield return arquetipo;
                }
            }
            else
            {
                await foreach (var arquetipo in repoLocal.RecuperarStreamAsync())
                {
                    yield return arquetipo;
                }
            }
        }
        public List<Arquetipos> RecuperarList()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.RecuperarList();
            }
            else
            {
                return repoLocal.RecuperarList();
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Eliminar(ID, Caso);
            }
            else
            {
                return repoLocal.Eliminar(ID, Caso);
            }
        }
        public bool Modificar(Arquetipos registroModificado)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(registroModificado);
            }
            else
            {
                return repoLocal.Modificar(registroModificado);
            }
        }
        public string Insertar(Arquetipos nuevoRegistro)
        {
            Guid id = Guid.NewGuid();
            nuevoRegistro.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevoRegistro);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevoRegistro);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevoRegistro);
            }
        }
        public Arquetipos Recuperar(string ID)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Recuperar(ID);
            }
            else
            {
                return repoLocal.Recuperar(ID);
            }
        }
    }
}
