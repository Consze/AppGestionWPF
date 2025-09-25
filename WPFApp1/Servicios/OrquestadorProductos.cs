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
        Producto,
        Null
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
                {"FechaModificacion", ServicioAsociado.Stock}
                //{"FechaCreacion",ServicioAsociado.Stock }
            };
        }

        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = productoServicio.RecuperarProductoPorID(productoModificado.ProductoSKU);
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();

            //Bandera
            bool ActualizarVersion = false;

            //1 - Bucle de relevamiento
            foreach (var propiedad in propiedadesEntidad)
            {
                var valorActual = propiedad.GetValue(productoActual);
                var valorModificado = propiedad.GetValue(productoModificado);
                if (!object.Equals(valorActual, valorModificado))
                {
                    switch (MapeoPropiedades[propiedad.Name])
                    {
                        case ServicioAsociado.Versiones:
                            ActualizarVersion = true;
                            break;
                    }
                }
            }

            //2- Llamada a servicios condicional
            if (ActualizarVersion)
            {
                string nuevaVersionID = ModificarVersion(productoModificado);
                productoModificado.ProductoVersionID = nuevaVersionID;
            }


            //3 - Llamada a conmutador para asentar cambios en Stock
            return productoServicio.ModificarProducto(productoModificado);
        }

        public string ModificarVersion(ProductoCatalogo productoModificado)
        {
            string IDNuevaVersion = string.Empty;
            Versiones versionActual = versionesServicio.Recuperar(productoModificado.ProductoVersionID);
            Versiones versionModificada = new Versiones
            {
                EAN = productoModificado.EAN,
                ProductoID = productoModificado.ID,
                RutaRelativaImagen = productoModificado.RutaImagen,
                FormatoID = productoModificado.FormatoProductoID,
                MarcaID = productoModificado.MarcaID,

                /**
                EsEliminado = Falso por defecto en DB
                FechaCreacion = DateTime.Now, Completado por conmutador
                FechaModificacion = Vacio
                ID = UUID Generada por conmutador
                */
            };

            if (!object.Equals(versionActual, versionModificada))
            {
                IDNuevaVersion = versionesServicio.Insertar(versionModificada);
            }
            return IDNuevaVersion;
        }
    }
}
