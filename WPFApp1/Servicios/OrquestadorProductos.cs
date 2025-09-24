using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Servicios
{
    public class OrquestadorProductos
    {
        private readonly IProductosServicio productoServicio;
        private readonly ServicioIndexacionProductos indexacionServicio;
        private readonly IConmutadorEntidadGenerica<Formatos> formatoServicio;
        private readonly IConmutadorEntidadGenerica<Versiones> versionesServicio;
        //conmutador marcas
        //conmutador ubicaciones_inventario
        //conmutador categorias
        //conmutador productos (base)
        public OrquestadorProductos(IProductosServicio _servicioProductos, IConmutadorEntidadGenerica<Formatos> _servicioFormatos,
            IConmutadorEntidadGenerica<Versiones> _servicioVersiones, ServicioIndexacionProductos _servicioIndexacion)
        {
            productoServicio = _servicioProductos;
            indexacionServicio = _servicioIndexacion;
            formatoServicio = _servicioFormatos;
            versionesServicio = _servicioVersiones;
        }

        public bool EditarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = productoServicio.RecuperarProductoPorID(productoModificado.ProductoSKU);
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaBlanca = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "UbicacionID",
                "MarcaID",
                "FormatoID",
                "ProductoVersionID"
            };

            foreach (var propiedad in propiedadesEntidad)
            {
                if (listaBlanca.Contains(propiedad.Name))
                {
                    var valorActual = propiedad.GetValue(productoActual);
                    var valorModificado = propiedad.GetValue(productoModificado);
                    if (!object.Equals(valorActual, valorModificado))
                    {

                    }
                }
            }

        }
    }
}
