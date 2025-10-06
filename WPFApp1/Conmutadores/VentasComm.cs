using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class VentasConmutador : IConmutadorEntidadGenerica<Ventas>
    {
        private readonly RepoVentasSQLite repoLocal;
        private readonly RepoVentasSQLServer repoServer;
        public VentasConmutador(RepoVentasSQLServer _repoServer, RepoVentasSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Ventas> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var venta in repoServer.RecuperarStreamAsync())
                {
                    yield return venta;
                }
            }
            else
            {
                await foreach (var venta in repoLocal.RecuperarStreamAsync())
                {
                    yield return venta;
                }
            }
        }
        public List<Ventas> RecuperarList()
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
        public bool Modificar(Ventas ventaModificada)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(ventaModificada);
            }
            else
            {
                return repoLocal.Modificar(ventaModificada);
            }
        }
        public string Insertar(Ventas nuevaVenta)
        {
            Guid id = Guid.NewGuid();
            nuevaVenta.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevaVenta);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevaVenta);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevaVenta);
            }
        }
        public Ventas Recuperar(string ID)
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
