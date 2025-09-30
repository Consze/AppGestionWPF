using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Conmutadores
{
    public class IndexadorProductos : IIndexadorProductosRepositorio
    {
        private readonly ConexionDBSQLServer _conexionServidor;
        private readonly IIndexadorProductosRepositorio _repositorioLocal;
        private readonly IIndexadorProductosRepositorio _repositorioRemoto;
        public IndexadorProductos(ConexionDBSQLServer conexionServidor, IIndexadorProductosRepositorio repositorioLocal, IIndexadorProductosRepositorio repositorioRemoto) 
        {
            _conexionServidor = conexionServidor;
            _repositorioLocal = repositorioLocal;
            _repositorioRemoto = repositorioRemoto;
        }
        public List<PalabrasTitulosProductos> BuscarPalabra(string Palabra)
        {
            List<PalabrasTitulosProductos> Palabras = new List<PalabrasTitulosProductos>();
           if(_conexionServidor.LeerConfiguracionManual())
            {
                Palabras = _repositorioRemoto.BuscarPalabra(Palabra);
            }
           else
            {
                Palabras = _repositorioLocal.BuscarPalabra(Palabra);
            }
            return Palabras;
        }
        public List<ProductoBase> BuscarProductos(List<string> ColeccionPalabras)
        {
            if (_conexionServidor.LeerConfiguracionManual())
            {
                return _repositorioRemoto.BuscarProductos(ColeccionPalabras);
            }
            else
            {
                return _repositorioLocal.BuscarProductos(ColeccionPalabras);
            }
        }
        public bool InsertarRegistro(string Palabra, string ID)
        {
            if (_conexionServidor.LeerConfiguracionManual())
            {
                return _repositorioRemoto.InsertarRegistro(Palabra, ID);
            }
            else
            {
                return _repositorioLocal.InsertarRegistro(Palabra, ID);
            }
        }
        public List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(string producto_id)
        {
            if (_conexionServidor.LeerConfiguracionManual())
            {
                return _repositorioRemoto.RecuperarIndicesPorProductoID(producto_id);
            }
            else
            {
                return _repositorioLocal.RecuperarIndicesPorProductoID(producto_id);
            }
        }
        public bool EliminarIndicesPorID(List<int> indicesID)
        {
            if (_conexionServidor.LeerConfiguracionManual())
            {
                return _repositorioRemoto.EliminarIndicesPorID(indicesID);
            }
            else
            {
                return _repositorioLocal.EliminarIndicesPorID(indicesID);
            }
        }
    }
}
