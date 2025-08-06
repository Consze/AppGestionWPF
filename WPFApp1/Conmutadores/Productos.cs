using WPFApp1.DTOS;
using WPFApp1.Factories;
using WPFApp1.Interfaces;
using WPFApp1.Servicios;

namespace WPFApp1.Conmutadores
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly SqliteRepositorioProductosFactory _sqliteFactory;
        private readonly SqlServerRepositorioProductosFactory _sqlServerFactory;
        private ConexionDBSQLServer RepositorioServidor;
        public ProductoServicio(SqliteRepositorioProductosFactory sqliteFactory, SqlServerRepositorioProductosFactory sqlServerFactory, ConexionDBSQLServer ConexionServidor)
        {
            RepositorioServidor = ConexionServidor;
            _sqliteFactory = sqliteFactory;
            _sqlServerFactory = sqlServerFactory;
        }
        public int CrearProducto(Productos producto)
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.CrearProducto(producto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (CrearProducto): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.CrearProducto(producto);
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.CrearProducto(producto);
            }
        }
        public bool ActualizarProducto(Productos producto)
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.ActualizarProducto(producto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (ActualizarProducto): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.ActualizarProducto(producto);
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.ActualizarProducto(producto);
            }
        }
        public Productos RecuperarProductoPorID(int producto_id)
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.RecuperarProductoPorID(producto_id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (RecuperarProductoPorID): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.RecuperarProductoPorID(producto_id);
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.RecuperarProductoPorID(producto_id);
            }
        }
        public bool EliminarProducto(int producto_id)
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.EliminarProducto(producto_id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (EliminarProducto): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.EliminarProducto(producto_id);
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.EliminarProducto(producto_id);
            }
        }
        public List<Productos> LeerProductos()
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.LeerProductos();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (Leer Productos): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.LeerProductos();
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.LeerProductos();
            }
        }
        public bool CrearLibro(List<Productos> Productos)
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                try
                {
                    var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                    return sqlServerRepo.CrearLibro(Productos);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar usar SQL Server (Crear XLSX): {ex.Message}. Intentando con SQLite.");
                    var sqliteRepo = _sqliteFactory.CrearRepositorio();
                    return sqliteRepo.CrearLibro(Productos);
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.CrearLibro(Productos);
            }
        }
    }
}