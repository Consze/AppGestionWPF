using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class FacturasConmutador : IConmutadorEntidadGenerica<Factura>
    {
        private readonly RepoFacturaSQLite repoLocal;
        private readonly RepoFacturaSQLServer repoServer;
        public FacturasConmutador(RepoFacturaSQLServer _repoServer, RepoFacturaSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Factura> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var Factura in repoServer.RecuperarStreamAsync())
                {
                    yield return Factura;
                }
            }
            else
            {
                await foreach (var Factura in repoLocal.RecuperarStreamAsync())
                {
                    yield return Factura;
                }
            }
        }
        public List<Factura> RecuperarList()
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
        public bool Modificar(Factura marcaModificada)
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
        public string Insertar(Factura nuevaFactura)
        {
            Guid id = Guid.NewGuid();
            nuevaFactura.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevaFactura);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevaFactura);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevaFactura);
            }
        }
        public Factura Recuperar(string ID)
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
