using WPFApp1;

namespace WPFApp1.Factories
{
    public class SqlServerRepositorioProductosFactory : IRepositorioProductosFactory
    {
        private readonly string _cadenaConexion;

        public SqlServerRepositorioProductosFactory(string CadenaConexion)
        {
            this._cadenaConexion = CadenaConexion;
        }

        public IProductosAccesoDatos CrearRepositorio()
        {
            return new SQLServerAccesoProductos(this._cadenaConexion);
        }
    }
}