using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;
using WPFApp1.Entidades;

namespace WPFApp1.Servicios
{
    public class ServicioIndexacionProductos
    {
        private readonly IndexadorGenericoService _indexadorServicio;
        private readonly IIndexadorProductosRepositorio _repositorioIndexacion;
        public ServicioIndexacionProductos(IndexadorGenericoService indexadorGenerico, IIndexadorProductosRepositorio repositorioIndexacion)
        {
            _indexadorServicio = indexadorGenerico;
            _repositorioIndexacion = repositorioIndexacion;
        }
        public List<ProductoBase> BuscarTituloProductos(string TituloBuscado)
        {
            List<string> TituloPalabras = _indexadorServicio.ObtenerPalabrasAtomicasFiltradas(TituloBuscado);
            List<ProductoBase> coleccionProductosResultado = _repositorioIndexacion.BuscarProductos(TituloPalabras);

            return coleccionProductosResultado;
        }
        public void IndexarProducto(ProductoBase Producto)
        {
            string titulo = Producto.Nombre;
            string ID = Producto.ID;
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                List<string> TituloPalabras = _indexadorServicio.ObtenerPalabrasAtomicasFiltradas(titulo);
                LimpiarReferenciasObsoletas(Producto.ID, TituloPalabras);

                foreach (string palabra in TituloPalabras)
                {
                    _repositorioIndexacion.InsertarRegistro(palabra, ID);
                }
            }
        }
        public void LimpiarReferenciasObsoletas(string producto_id, List<string> PalabrasTitulo)
        {
            List<IDX_Prod_Titulos> registrosIDX = _repositorioIndexacion.RecuperarIndicesPorProductoID(producto_id);
            List<int> registrosAEliminar = new List<int>();

            if(registrosIDX.Count > 0)
            {
                foreach(IDX_Prod_Titulos registro in registrosIDX)
                {
                    if(!PalabrasTitulo.Contains(registro.palabra))
                    {
                        registrosAEliminar.Add(registro.ID);
                    }
                }
                _repositorioIndexacion.EliminarIndicesPorID(registrosAEliminar);
            }
        }
    }
}
