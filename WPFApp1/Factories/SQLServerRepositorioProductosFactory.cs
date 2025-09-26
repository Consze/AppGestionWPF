using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Factories
{
    public class SqlServerRepositorioProductosFactory
    {
        private readonly string _cadenaConexion;

        public SqlServerRepositorioProductosFactory(string CadenaConexion)
        {
            this._cadenaConexion = CadenaConexion;
        }
    }
}