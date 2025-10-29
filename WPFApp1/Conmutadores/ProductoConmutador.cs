using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;
using WPFApp1.Enums;

namespace WPFApp1.Conmutadores
{
    public class ProductoConmutador : IProductosServicio
    {
        private readonly ProductosAccesoDatosSQLServer _repoServer;
        private readonly ProductosAcessoDatosSQLite _repoLocal;
        public ProductoConmutador(ProductosAccesoDatosSQLServer repoServer, ProductosAcessoDatosSQLite repoLocal)
        {
            _repoServer = repoServer;
            _repoLocal = repoLocal;
        }
        public ProductoCatalogo RecuperarProductoPorID(string productoID)
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.RecuperarProductoPorID(productoID);
            }
            else
            {
                return _repoLocal.RecuperarProductoPorID(productoID);
            }
        }
        public string CrearProducto(ProductoCatalogo nuevoProducto)
        {
            Guid guid = Guid.NewGuid();
            nuevoProducto.ProductoSKU = guid.ToString();
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                try
                {
                    return _repoServer.CrearProducto(nuevoProducto);
                }
                catch
                {
                    return _repoLocal.CrearProducto(nuevoProducto);
                }
            }
            else
            {
                return _repoLocal.CrearProducto(nuevoProducto);
            }
        }
        public async IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync()
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                await foreach (var producto in _repoServer.LeerProductosAsync())
                {
                    yield return producto;
                }
            }
            else
            {
                await foreach (var producto in _repoLocal.LeerProductosAsync())
                {
                    yield return producto;
                }
            }
        }
        public List<ProductoCatalogo> LeerProductos()
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.LeerProductos();
            }
            else
            {
                return _repoLocal.LeerProductos();
            }
        }
        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.ModificarProducto(productoModificado);
            }
            else
            {
                return _repoLocal.ModificarProducto(productoModificado);
            }
        }
        public bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion)
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.EliminarProducto(ProductoID, TipoEliminacion.Logica);
            }
            else
            {
                return _repoLocal.EliminarProducto(ProductoID, TipoEliminacion.Logica);
            }
        }
        public bool ModificacionMasiva(List<ProductoEditar_Propiedad_Valor> lista)
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.ModificacionMasiva(lista);
            }
            else
            {
                return _repoLocal.ModificacionMasiva(lista);
            }
        }
        public List<ProductoCatalogo> RecuperarLotePorIDS(string propiedadNombre ,List<string> IDs)
        {
            if (_repoServer._accesoDB.LeerConfiguracionManual())
            {
                return _repoServer.RecuperarLotePorIDS(propiedadNombre, IDs);
            }
            else
            {
                return _repoLocal.RecuperarLotePorIDS(propiedadNombre, IDs);
            }
        }
    }
}
