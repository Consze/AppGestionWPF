using System.Security.AccessControl;
using WPFApp1.Conmutadores;
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
        private readonly IConmutadorEntidadGenerica<Arquetipos> arquetiposServicio;
        private readonly IConmutadorEntidadGenerica<Ubicaciones> ubicacionesServicio;
        private readonly IConmutadorEntidadGenerica<Categorias> categoriasServicio;
        private readonly IConmutadorEntidadGenerica<Marcas> marcasServicio;

        public OrquestadorProductos(IProductosServicio _servicioProductos, IConmutadorEntidadGenerica<Formatos> _servicioFormatos,
            IConmutadorEntidadGenerica<Versiones> _servicioVersiones, ServicioIndexacionProductos _servicioIndexacion, 
            IConmutadorEntidadGenerica<Arquetipos> _servicioArquetipo, IConmutadorEntidadGenerica<Ubicaciones> _servicioUbicaciones,
            IConmutadorEntidadGenerica<Categorias> _servicioCategorias, IConmutadorEntidadGenerica<Marcas> _servicioMarcas)
        {
            productoServicio = _servicioProductos;
            indexacionServicio = _servicioIndexacion;
            formatoServicio = _servicioFormatos;
            versionesServicio = _servicioVersiones;
            arquetiposServicio = _servicioArquetipo;
            ubicacionesServicio = _servicioUbicaciones;
            categoriasServicio = _servicioCategorias;
            marcasServicio = _servicioMarcas;

            MapeoPropiedades = new Dictionary<string, ServicioAsociado>(StringComparer.OrdinalIgnoreCase)
            {
                // Propiedad de Clase , Servicio Asociado
                {"RutaImagen", ServicioAsociado.Versiones },
                {"EAN", ServicioAsociado.Versiones },
                {"MarcaID", ServicioAsociado.Versiones },
                {"FormatoProductoID", ServicioAsociado.Versiones },

                // Inserción de nueva entidad auxiliar
                {"MarcaNombre", ServicioAsociado.Marcas },
                {"CategoriaNombre", ServicioAsociado.Categorias },
                {"UbicacionNombre", ServicioAsociado.Ubicaciones },
                {"FormatoNombre", ServicioAsociado.Formatos },
                {"ID", ServicioAsociado.Producto},
                {"Nombre", ServicioAsociado.Producto },
                {"Alto", ServicioAsociado.Formatos},
                {"Largo", ServicioAsociado.Formatos},
                {"Profundidad", ServicioAsociado.Formatos},
                {"Peso", ServicioAsociado.Formatos},

                // Propiedades de registro Stock
                {"Categoria", ServicioAsociado.Stock},
                {"UbicacionID", ServicioAsociado.Stock},
                {"Haber", ServicioAsociado.Stock},
                {"Precio", ServicioAsociado.Stock},
                {"EsEliminado", ServicioAsociado.Stock },
                {"VisibilidadWeb", ServicioAsociado.Stock},
                {"PrecioPublico", ServicioAsociado.Stock},

                // Ignoradas
                {"ProductoSKU", ServicioAsociado.Null},
                {"FechaModificacion", ServicioAsociado.Null},
                {"FechaCreacion",ServicioAsociado.Null}
            };
        }

        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = productoServicio.RecuperarProductoPorID(productoModificado.ProductoSKU);
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();

            //Banderas
            bool ActualizarVersion = false;
            bool ActualizarArquetipo = false;

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

                        case ServicioAsociado.Producto:
                            ActualizarArquetipo = true;
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
            if(ActualizarArquetipo)
            {
                Arquetipos _registro = new Arquetipos
                {
                    ID = productoModificado.ID,
                    Nombre = productoModificado.Nombre,
                    CategoriaID = productoModificado.Categoria
                };
                arquetiposServicio.Modificar(_registro);

                ProductoBase _producto = new ProductoBase
                {
                    ID = productoModificado.ID,
                    Nombre = productoModificado.Nombre
                };
                indexacionServicio.IndexarProducto(_producto);
            }

            //3 - Llamada a conmutador para asentar cambios en Stock
            bool ActualizacionStock = productoServicio.ModificarProducto(productoModificado);


            return ActualizacionStock || ActualizarVersion || ActualizarArquetipo;
        }
        public string CrearProducto(ProductoCatalogo productoNuevo)
        {
            Arquetipos nuevoProducto = new Arquetipos
            {
                CategoriaID = productoNuevo.Categoria,
                Nombre = productoNuevo.Nombre
            };
            string arquetipoID = arquetiposServicio.Insertar(nuevoProducto);
            productoNuevo.ID = arquetipoID;

            Versiones version = new Versiones
            {
                ProductoID = arquetipoID,
                RutaRelativaImagen = productoNuevo.RutaImagen,
                FormatoID = productoNuevo.FormatoProductoID,
                MarcaID = productoNuevo.MarcaID,
                EAN = productoNuevo.EAN
            };
            productoNuevo.ProductoVersionID = versionesServicio.Insertar(version);

            // ubicacion, marca, formato
            if (string.IsNullOrEmpty(productoNuevo.MarcaID) && !string.IsNullOrEmpty(productoNuevo.MarcaNombre))
            {
                Marcas nuevaMarca = new Marcas { Nombre = productoNuevo.MarcaNombre };
                productoNuevo.MarcaID = marcasServicio.Insertar(nuevaMarca);
            }

            if(string.IsNullOrEmpty(productoNuevo.FormatoProductoID) && !string.IsNullOrEmpty(productoNuevo.FormatoNombre))
            {
                Formatos nuevoFormato = new Formatos
                {
                    Alto = productoNuevo.Alto,
                    Largo = productoNuevo.Largo,
                    Profundidad = productoNuevo.Profundidad,
                    Peso = productoNuevo.Peso,
                    Nombre = productoNuevo.FormatoNombre
                };
                productoNuevo.FormatoProductoID = formatoServicio.Insertar(nuevoFormato);
            }

            if (string.IsNullOrEmpty(productoNuevo.UbicacionID) && !string.IsNullOrEmpty(productoNuevo.UbicacionNombre))
            {
                Ubicaciones nuevaUbicacion = new Ubicaciones { Nombre = productoNuevo.UbicacionNombre };
                productoNuevo.UbicacionID = ubicacionesServicio.Insertar(nuevaUbicacion);
            }

            // Insertar nuevo registro
            string nuevaId = productoServicio.CrearProducto(productoNuevo);
            productoNuevo.ProductoSKU = nuevaId;
            indexacionServicio.IndexarProducto(productoNuevo);
            return nuevaId;
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
