using WPFApp1.Interfaces;
using WPFApp1.Servicios;
using WPFApp1.Repositorios;

namespace WPFApp1.Factories
{
    public class SqliteRepositorioProductosFactory : IProductosFactory
    {
        private readonly string _cadenaConexion;
        private readonly IndexadorProductoService _indexadorProductoService;
        private readonly ConexionDBSQLite _accesoDB;
        public SqliteRepositorioProductosFactory(string cadenaConexion, IndexadorProductoService indexadorProductoService, ConexionDBSQLite accesoDB)
        {
            this._cadenaConexion = cadenaConexion;
            this._indexadorProductoService = indexadorProductoService;
            this._accesoDB = accesoDB;
        }
        public IProductosAccesoDatos CrearRepositorio()
        {
            return new SQLiteAccesoProductos(_cadenaConexion,_indexadorProductoService,_accesoDB);
        }
    }
}
