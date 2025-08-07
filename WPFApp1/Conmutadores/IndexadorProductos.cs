using WPFApp1.Interfaces;
using WPFApp1.Servicios;
using WPFApp1.DTOS;

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
        public bool InsertarRegistro(string Palabra, int ID)
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
        public List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(int producto_id)
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
