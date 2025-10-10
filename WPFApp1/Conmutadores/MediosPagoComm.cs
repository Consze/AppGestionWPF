using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;
using WPFApp1.Enums;

namespace WPFApp1.Conmutadores
{
    public class MediosPagoConmutador : IConmutadorEntidadGenerica<Medios_Pago>
    {
        private readonly RepoMediosPagoSQLite repoLocal;
        private readonly RepoMediosPagoSQLServer repoServer;
        public MediosPagoConmutador(RepoMediosPagoSQLServer _repoServer, RepoMediosPagoSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Medios_Pago> RecuperarStreamAsync()
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                await foreach (var medioPago in repoServer.RecuperarStreamAsync())
                {
                    yield return medioPago;
                }
            }
            else
            {
                await foreach (var medioPago in repoLocal.RecuperarStreamAsync())
                {
                    yield return medioPago;
                }
            }
        }
        public List<Medios_Pago> RecuperarList()
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
        public bool Modificar(Medios_Pago medioPagoModificado)
        {
            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                return repoServer.Modificar(medioPagoModificado);
            }
            else
            {
                return repoLocal.Modificar(medioPagoModificado);
            }
        }
        public string Insertar(Medios_Pago nuevoMedioPago)
        {
            Guid id = Guid.NewGuid();
            nuevoMedioPago.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevoMedioPago);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevoMedioPago);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevoMedioPago);
            }
        }
        public Medios_Pago Recuperar(string ID)
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
