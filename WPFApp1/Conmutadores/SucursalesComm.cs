using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class SucursalesConmutador : IConmutadorEntidadGenerica<Sucursal>
    {
        private readonly RepoSucursalesSQLite repoLocal;
        private readonly RepoSucursalesSQLServer repoServer;
        public SucursalesConmutador(RepoSucursalesSQLServer _repoServer, RepoSucursalesSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Sucursal> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var Sucursal in repoServer.RecuperarStreamAsync())
                {
                    yield return Sucursal;
                }
            }
            else
            {
                await foreach (var Sucursal in repoLocal.RecuperarStreamAsync())
                {
                    yield return Sucursal;
                }
            }
        }
        public List<Sucursal> RecuperarList()
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
        public bool Modificar(Sucursal sucursalModificada)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(sucursalModificada);
            }
            else
            {
                return repoLocal.Modificar(sucursalModificada);
            }
        }
        public string Insertar(Sucursal nuevaSucursal)
        {
            Guid id = Guid.NewGuid();
            nuevaSucursal.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevaSucursal);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevaSucursal);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevaSucursal);
            }
        }
        public Sucursal Recuperar(string ID)
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
