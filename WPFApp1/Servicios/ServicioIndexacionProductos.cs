using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;

namespace WPFApp1.Servicios
{
    public class ServicioIndexacionProductos
    {
        private readonly IndexadorGenericoService _indexadorServicio;
        private readonly IIndexadorProductosRepositorio _repositorioIndexacion;
        private readonly Func<IProductoServicio> _productoServicioFactory;
        public ServicioIndexacionProductos(IndexadorGenericoService indexadorGenerico, IIndexadorProductosRepositorio repositorioIndexacion, Func<IProductoServicio> productoServicioFactory)
        {
            _indexadorServicio = indexadorGenerico;
            _repositorioIndexacion = repositorioIndexacion;
            _productoServicioFactory = productoServicioFactory;
        }
        public List<CoincidenciasBusqueda> RecuperarRegistros(string TerminoBusqueda)
        {
            Dictionary<int, CoincidenciasBusqueda> resultadosPorProductoId = new Dictionary<int, CoincidenciasBusqueda>();
            List<string> TituloPalabras = _indexadorServicio.ObtenerPalabrasAtomicasFiltradas(TerminoBusqueda);

            foreach (string palabra in TituloPalabras)
            {
                // Recuperar cada registro de asociación
                List<PalabrasTitulosProductos> palabraCoincidencias = _repositorioIndexacion.BuscarPalabra(palabra);
                foreach (PalabrasTitulosProductos coincidencia in palabraCoincidencias)
                {
                    if (resultadosPorProductoId.TryGetValue(coincidencia.producto_id, out CoincidenciasBusqueda productoEncontrado))
                    {
                        productoEncontrado.CantidadPalabrasCoincidentes++;
                    }
                    else
                    {
                        resultadosPorProductoId.Add(coincidencia.producto_id, new CoincidenciasBusqueda
                        {
                            producto_id = coincidencia.producto_id,
                            palabra = coincidencia.palabra,
                            CantidadPalabrasCoincidentes = 1
                        });
                    }
                }
            }

            return resultadosPorProductoId.Values.OrderByDescending(c => c.CantidadPalabrasCoincidentes).ToList();
        }
        public List<Productos> RecuperarProductos(List<CoincidenciasBusqueda> CoincidenciasProductos)
        {
            var _productoServicio = _productoServicioFactory();
            List<Productos> ProductosCoincidentes = new List<Productos>();

            foreach (CoincidenciasBusqueda Coincidencia in CoincidenciasProductos)
            {
                Productos registro = _productoServicio.RecuperarProductoPorID(Coincidencia.producto_id);
                if (registro != null && registro.ID != 0)
                {
                    ProductosCoincidentes.Add(registro);
                }
            }

            return ProductosCoincidentes;
        }
        public List<Productos> BuscarTituloProductos(string TituloBuscado)
        {
            List<CoincidenciasBusqueda> coincidencias = RecuperarRegistros(TituloBuscado);
            List<Productos> ColeccionProductosCoincidentes = RecuperarProductos(coincidencias);
            return ColeccionProductosCoincidentes;
        }
        public void IndexarProducto(Productos Producto)
        {
            string titulo = Producto.Nombre;
            int ID = Producto.ID;
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                List<string> TituloPalabras = _indexadorServicio.ObtenerPalabrasAtomicasFiltradas(titulo);

                foreach (string palabra in TituloPalabras)
                {
                    _repositorioIndexacion.InsertarRegistro(palabra, ID);
                }
            }
        }
    }
}
