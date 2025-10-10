using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class CuentasContablesConmutador : IConmutadorEntidadGenerica<Cuentas_Contables>
    {
        private readonly RepoCuentasContablesSQLite repoLocal;
        private readonly RepoCuentasContablesSQLServer repoServer;
        public CuentasContablesConmutador(RepoCuentasContablesSQLServer _repoServer, RepoCuentasContablesSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Cuentas_Contables> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var cuentaContable in repoServer.RecuperarStreamAsync())
                {
                    yield return cuentaContable;
                }
            }
            else
            {
                await foreach (var cuentaContable in repoLocal.RecuperarStreamAsync())
                {
                    yield return cuentaContable;
                }
            }
        }
        public List<Cuentas_Contables> RecuperarList()
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
        public bool Modificar(Cuentas_Contables cuentaContableModificada)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(cuentaContableModificada);
            }
            else
            {
                return repoLocal.Modificar(cuentaContableModificada);
            }
        }
        public string Insertar(Cuentas_Contables nuevaCuentaContable)
        {
            Guid id = Guid.NewGuid();
            nuevaCuentaContable.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevaCuentaContable);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevaCuentaContable);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevaCuentaContable);
            }
        }
        public Cuentas_Contables Recuperar(string ID)
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
