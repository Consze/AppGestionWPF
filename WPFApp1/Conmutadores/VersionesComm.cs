using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class VersionesConmutador : IConmutadorEntidadGenerica<Versiones>
    {
        private readonly RepoVersionesSQLite repoLocal;
        private readonly RepoVersionesSQLServer repoServer;
        public VersionesConmutador(RepoVersionesSQLServer _repoServer, RepoVersionesSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Versiones> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var version in repoServer.RecuperarStreamAsync())
                {
                    yield return version;
                }
            }
            else
            {
                await foreach (var version in repoLocal.RecuperarStreamAsync())
                {
                    yield return version;
                }
            }
        }
        public List<Versiones> RecuperarList()
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
        public bool Modificar(Versiones registroModificado)
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
        public string Insertar(Versiones nuevoRegistro)
        {
            Guid id = Guid.NewGuid();
            nuevoRegistro.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevoRegistro);
                }
                catch(Exception)
                {
                    return repoLocal.Insertar(nuevoRegistro);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevoRegistro);
            }
        }
        public Versiones Recuperar(string ID)
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
        public List<Versiones> BuscarEan(string EanBuscado)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.BuscarEan(EanBuscado);
            }
            else
            {
                return repoLocal.BuscarEan(EanBuscado);
            }
        }
        public List<Versiones> RecuperarLotePorIDS(string propiedadNombre, List<string> IDs)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.RecuperarLotePorIDS(propiedadNombre, IDs);
            }
            else
            {
                return repoLocal.RecuperarLotePorIDS(propiedadNombre, IDs);
            }
        }
    }
}
