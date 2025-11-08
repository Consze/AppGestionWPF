using WPFApp1.Entidades;
using WPFApp1.Repositorios;
using WPFApp1.Interfaces;
using WPFApp1.Enums;

namespace WPFApp1.Conmutadores
{
    public class ConmutadorCondiciones : IConmutadorEntidadGenerica<Condiciones>
    {
        private readonly RepoCondicionesSQLite repoLocal;
        private readonly RepoCondicionesSQLServer repoServer;
        public ConmutadorCondiciones(RepoCondicionesSQLite _repoLocal, RepoCondicionesSQLServer _repoServer)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Condiciones> RecuperarStreamAsync()
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
        public List<Condiciones> RecuperarList()
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
        public bool Modificar(Condiciones registroModificado)
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
        public string Insertar(Condiciones nuevoRegistro)
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
        public Condiciones Recuperar(string ID)
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
