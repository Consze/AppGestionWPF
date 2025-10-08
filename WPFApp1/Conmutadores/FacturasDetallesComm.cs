using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class FacturasDetallesConmutador : IConmutadorEntidadGenerica<Factura_Detalles>
    {
        private readonly RepoFacturaDetallesSQLite repoLocal;
        private readonly RepoFacturaDetallesSQLServer repoServer;
        public FacturasDetallesConmutador(RepoFacturaDetallesSQLServer _repoServer, RepoFacturaDetallesSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Factura_Detalles> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var FacturaDetalle in repoServer.RecuperarStreamAsync())
                {
                    yield return FacturaDetalle;
                }
            }
            else
            {
                await foreach (var FacturaDetalle in repoLocal.RecuperarStreamAsync())
                {
                    yield return FacturaDetalle;
                }
            }
        }
        public List<Factura_Detalles> RecuperarList()
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
        public bool Modificar(Factura_Detalles marcaModificada)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(marcaModificada);
            }
            else
            {
                return repoLocal.Modificar(marcaModificada);
            }
        }
        public string Insertar(Factura_Detalles nuevoDetalleFactura)
        {
            Guid id = Guid.NewGuid();
            nuevoDetalleFactura.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevoDetalleFactura);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevoDetalleFactura);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevoDetalleFactura);
            }
        }
        public Factura_Detalles Recuperar(string ID)
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
