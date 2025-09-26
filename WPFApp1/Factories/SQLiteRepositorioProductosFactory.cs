using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Factories
{
    public class SqliteRepositorioProductosFactory 
    {
        private readonly ConexionDBSQLite _accesoDB;
        public SqliteRepositorioProductosFactory(ConexionDBSQLite accesoDB)
        {
            this._accesoDB = accesoDB;
        }
    }
}
