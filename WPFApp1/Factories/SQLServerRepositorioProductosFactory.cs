using WPFApp1.Interfaces;
using WPFApp1.Servicios;

namespace WPFApp1.Factories
{
    public class SqlServerRepositorioProductosFactory : IProductosFactory
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