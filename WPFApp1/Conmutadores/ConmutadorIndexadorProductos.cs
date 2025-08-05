using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;
using WPFApp1.Servicios;

namespace WPFApp1.Conmutadores
{
    public class ConmutadorIndexadorProductos: IIndexadorProductos
    {
        private readonly ConexionDBSQLServer _conexionDBSQLServer;
        private readonly IIndexadorProductos _repositorioLocal;
        private readonly IIndexadorProductos _repositorioRemoto;
        public ConmutadorIndexadorProductos(IIndexadorProductos repositorioLocal, IIndexadorProductos repositorioRemoto, ConexionDBSQLServer conexionDBSQLServer)
        {
            _conexionDBSQLServer = conexionDBSQLServer;
            _repositorioLocal = repositorioLocal;
            _repositorioRemoto = repositorioRemoto;
        }
        public List<PalabrasTitulosProductos> BuscarPalabra(string Palabra)
        {
            if (_conexionDBSQLServer.LeerConfiguracionManual())
            {
                //sql server
            }
            else
            {
                //sqlite
            }
                return null;
        }
        public bool InsertarRegistro(string Palabra, int ID)
        {
            if (_conexionDBSQLServer.LeerConfiguracionManual())
            {
                //sql server
            }
            else
            {
                //sqlite
            }
            return false;
        }
    }
}
