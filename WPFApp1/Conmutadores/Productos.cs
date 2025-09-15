using WPFApp1.DTOS;
using WPFApp1.Factories;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;
using System.IO;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class ProductoServicio : IProductoServicioObsoleto
    {
        private readonly SqliteRepositorioProductosFactory _sqliteFactory;
        private readonly SqlServerRepositorioProductosFactory _sqlServerFactory;
        private ConexionDBSQLServer RepositorioServidor;
        public ProductoServicio(SqliteRepositorioProductosFactory sqliteFactory, SqlServerRepositorioProductosFactory sqlServerFactory, ConexionDBSQLServer ConexionServidor)
        {
            try
            {
                RepositorioServidor = ConexionServidor;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            _sqliteFactory = sqliteFactory;
            _sqlServerFactory = sqlServerFactory;
        }
        public string CrearProducto(Productos producto)
        {
            Guid nuevaID = Guid.NewGuid();
            producto.ID = nuevaID.ToString();
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
                    Console.WriteLine($"Error al intentar usar SQL Server (ActualizarProducto): {ex.Message}.");
                    return false;
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.ActualizarProducto(producto);
            }
        }
        public Productos RecuperarProductoPorID(string producto_id)
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
                    Console.WriteLine($"Error al intentar usar SQL Server (RecuperarProductoPorID): {ex.Message}.");
                    return null;
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                return sqliteRepo.RecuperarProductoPorID(producto_id);
            }
        }
        public bool EliminarProducto(string producto_id)
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
                    Console.WriteLine($"Error al intentar usar SQL Server (EliminarProducto): {ex.Message}.");
                    return false;
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
                    Notificacion _notificacion = new Notificacion { Mensaje = $"Error al intentar usar SQL Server (Leer Productos): {ex.Message}. Intentando con SQLite.", Titulo = "Operación Fallida", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                    Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                    //Console.WriteLine($"Error al intentar usar SQL Server (Leer Productos): {ex.Message}. Intentando con SQLite.");
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
        public async IAsyncEnumerable<Productos> LeerProductosAsync()
        {
            if (RepositorioServidor.LeerConfiguracionManual())
            {
                var sqlServerRepo = _sqlServerFactory.CrearRepositorio();
                await foreach (var producto in sqlServerRepo.LeerProductosAsync())
                {
                    yield return producto;
                }
            }
            else
            {
                var sqliteRepo = _sqliteFactory.CrearRepositorio();
                await foreach (var producto in sqliteRepo.LeerProductosAsync())
                {
                    yield return producto;
                }
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