using WPFApp1;

namespace WPFApp1.Factories
{
    public class SqliteRepositorioProductosFactory : IRepositorioProductosFactory
    {
        private readonly string _cadenaConexion;

        public SqliteRepositorioProductosFactory(string CadenaConexion)
        {
            this._cadenaConexion = CadenaConexion;
        }

        public IProductosAccesoDatos CrearRepositorio()
        {
            return new SQLiteAccesoProductos(this._cadenaConexion);
        }
    }
}
