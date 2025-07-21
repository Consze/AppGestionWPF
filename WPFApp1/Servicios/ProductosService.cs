using WPFApp1.DTOS;
using WPFApp1.Factories;
using WPFApp1.Interfaces;

namespace WPFApp1.Servicios
{
    public class ProductosServicio : IProductosAccesoDatos
    {
        private readonly SqliteRepositorioProductosFactory _sqliteFactory;
        private readonly SqlServerRepositorioProductosFactory _sqlServerFactory;

        public ProductosServicio(SqliteRepositorioProductosFactory sqliteFactory, SqlServerRepositorioProductosFactory sqlServerFactory)
        {
            _sqliteFactory = sqliteFactory;
            _sqlServerFactory = sqlServerFactory;
        }

        public int CrearProducto(Productos producto)
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

        public bool ActualizarProducto(Productos producto)
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

        public Productos RecuperarProductoPorID(int producto_id)
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

        public bool EliminarProducto(int producto_id)
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
    }
}