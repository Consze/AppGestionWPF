using WPFApp1.Interfaces;
using WPFApp1.Servicios;

namespace WPFApp1.Factories
{
    public class SqliteRepositorioProductosFactory : IProductosFactory
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
