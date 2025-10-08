using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class FacturasPagosConmutador : IConmutadorEntidadGenerica<Factura_pagos>
    {
        private readonly RepoFacturaPagosSQLite repoLocal;
        private readonly RepoFacturaPagosSQLServer repoServer;
        public FacturasPagosConmutador(RepoFacturaPagosSQLServer _repoServer, RepoFacturaPagosSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Factura_pagos> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var FacturaPago in repoServer.RecuperarStreamAsync())
                {
                    yield return FacturaPago;
                }
            }
            else
            {
                await foreach (var FacturaPago in repoLocal.RecuperarStreamAsync())
                {
                    yield return FacturaPago;
                }
            }
        }
        public List<Factura_pagos> RecuperarList()
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
        public bool Modificar(Factura_pagos marcaModificada)
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
        public string Insertar(Factura_pagos nuevoPagoFactura)
        {
            Guid id = Guid.NewGuid();
            nuevoPagoFactura.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevoPagoFactura);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevoPagoFactura);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevoPagoFactura);
            }
        }
        public Factura_pagos Recuperar(string ID)
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
