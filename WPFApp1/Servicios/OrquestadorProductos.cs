using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Servicios
{
    public enum ServicioAsociado
    {
        Stock,
        Ubicaciones,
        Formatos,
        Versiones,
        Marcas,
        Categorias,
        Indexacion,
        Producto
    }
    public class OrquestadorProductos
    {
        public readonly Dictionary<string, ServicioAsociado> MapeoPropiedades;
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

            MapeoPropiedades = new Dictionary<string, ServicioAsociado>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Servicio Asociado
                {"RutaImagen", ServicioAsociado.Versiones },
                {"EAN", ServicioAsociado.Versiones },
                {"MarcaID", ServicioAsociado.Versiones },
                {"FormatoID", ServicioAsociado.Versiones },

                {"MarcaNombre", ServicioAsociado.Marcas },

                {"UbicacionID", ServicioAsociado.Stock},
                {"ID", ServicioAsociado.Producto},
                {"Haber", ServicioAsociado.Stock},
                {"Precio", ServicioAsociado.Stock},
                {"EsEliminado", ServicioAsociado.Stock },
                {"FechaModificacion", ServicioAsociado.Stock},
                {"FechaCreacion",ServicioAsociado.Stock }
            };
        }

        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = productoServicio.RecuperarProductoPorID(productoModificado.ProductoSKU);
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();

            foreach (var propiedad in propiedadesEntidad)
            {
                var valorActual = propiedad.GetValue(productoActual);
                var valorModificado = propiedad.GetValue(productoModificado);
                if (!object.Equals(valorActual, valorModificado))
                {

                }
            }
        }

        public bool ModificarVersion(ProductoCatalogo productoModificado)
        {
            Versiones versionActual = versionesServicio.Recuperar(productoModificado.ProductoVersionID);
            Versiones versionModificada = new Versiones
            {
                EAN = productoModificado.EAN,

            }
        }
    }
}
