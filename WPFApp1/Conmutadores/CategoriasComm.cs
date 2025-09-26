using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class CategoriasConmutador : IConmutadorEntidadGenerica<Categorias>
    {
        private readonly RepoCategoriasSQLite repoLocal;
        private readonly RepoCategoriasSQLServer repoServer;
        public CategoriasConmutador(RepoCategoriasSQLServer _repoServer, RepoCategoriasSQLite _repoLocal)
        {
            repoLocal = _repoLocal;
            repoServer = _repoServer;
        }
        public async IAsyncEnumerable<Categorias> RecuperarStreamAsync()
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
        public List<Categorias> RecuperarList()
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
        public bool Modificar(Categorias marcaModificada)
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
        public string Insertar(Categorias nuevaMarca)
        {
            Guid id = Guid.NewGuid();
            nuevaMarca.ID = id.ToString();

            if (repoServer.accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return repoServer.Insertar(nuevaMarca);
                }
                catch (Exception)
                {
                    return repoLocal.Insertar(nuevaMarca);
                }
            }
            else
            {
                return repoLocal.Insertar(nuevaMarca);
            }
        }
        public Categorias Recuperar(string ID)
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
