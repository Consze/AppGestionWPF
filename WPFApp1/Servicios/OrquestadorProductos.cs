using WPFApp1.DTOS;
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
        Arquetipo,
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
                {"Alto", ServicioAsociado.Formatos},
                {"Largo", ServicioAsociado.Formatos},
                {"Profundidad", ServicioAsociado.Formatos},
                {"Peso", ServicioAsociado.Formatos},

                // Arquetipo
                {"Nombre", ServicioAsociado.Arquetipo },
                {"Categoria", ServicioAsociado.Arquetipo},
                
                // Propiedades de registro Stock
                {"UbicacionID", ServicioAsociado.Stock},
                {"Haber", ServicioAsociado.Stock},
                {"Precio", ServicioAsociado.Stock},
                {"EsEliminado", ServicioAsociado.Stock },
                {"VisibilidadWeb", ServicioAsociado.Stock},
                {"PrecioPublico", ServicioAsociado.Stock},

                // Ignoradas
                {"ID", ServicioAsociado.Null},
                {"ProductoVersionID", ServicioAsociado.Null},
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
            bool ActualizarMarca = false;
            bool ActualizarUbicacion = false;
            bool ActualizarFormato = false;
            bool ActualizarCategoria = false;

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

                        case ServicioAsociado.Arquetipo:
                            ActualizarArquetipo = true;
                            break;

                        case ServicioAsociado.Marcas:
                            ActualizarMarca = true;
                            break;

                        case ServicioAsociado.Ubicaciones:
                            ActualizarUbicacion = true;
                            break;

                        case ServicioAsociado.Formatos:
                            ActualizarFormato = true;
                            break;

                        case ServicioAsociado.Categorias:
                            ActualizarCategoria = true;
                            break;

                    }
                }
            }

            //2 - Llamada a servicios condicional
            if(ActualizarMarca)
            {
                Marcas _marcas = new Marcas
                {
                    Nombre = productoModificado.MarcaNombre
                };
                string nuevaMarcaID = marcasServicio.Insertar(_marcas);
                productoModificado.MarcaID = nuevaMarcaID;
            }

            if(ActualizarFormato)
            {
                Formatos _formato = new Formatos
                {
                    Alto = productoModificado.Alto,
                    Largo = productoModificado.Largo,
                    Profundidad = productoModificado.Profundidad,
                    Peso = productoModificado.Peso,
                    Nombre = productoModificado.FormatoNombre
                };
                string nuevoFormatoID = formatoServicio.Insertar(_formato);
                productoModificado.FormatoProductoID = nuevoFormatoID;
            }

            if(ActualizarUbicacion)
            {
                Ubicaciones _ubicacion = new Ubicaciones
                {
                    Nombre = productoModificado.UbicacionNombre
                };
                string nuevaUbicacionID = ubicacionesServicio.Insertar(_ubicacion);
                productoModificado.UbicacionID = nuevaUbicacionID;
            }

            if(ActualizarCategoria)
            {
                Categorias _categoria = new Categorias
                {
                    Nombre = productoModificado.CategoriaNombre
                };
                string nuevaCategoriaID = categoriasServicio.Insertar(_categoria);
                productoModificado.Categoria = nuevaCategoriaID;
                ActualizarArquetipo = true;
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

            if (ActualizarVersion)
            {
                string nuevaVersionID = ModificarVersion(productoModificado);
                productoModificado.ProductoVersionID = nuevaVersionID;
            }

            //3 - Llamada a conmutador para asentar cambios en Stock
            bool ActualizacionStock = productoServicio.ModificarProducto(productoModificado);


            return ActualizacionStock || ActualizarVersion || ActualizarArquetipo || ActualizarUbicacion || ActualizarMarca;
        }
        public string CrearProducto(ProductoCatalogo productoNuevo)
        {
            Formatos formato = new Formatos
            {
                Alto = productoNuevo.Alto,
                Largo = productoNuevo.Largo,
                Profundidad = productoNuevo.Profundidad,
                Peso = productoNuevo.Peso,
                Nombre = productoNuevo.FormatoNombre
            };
            productoNuevo.FormatoProductoID = formatoServicio.Insertar(formato);

            Categorias categoria = new Categorias
            {
                Nombre = productoNuevo.CategoriaNombre
            };
            productoNuevo.Categoria = categoriasServicio.Insertar(categoria);

            Ubicaciones ubicacion = new Ubicaciones
            {
                Nombre = productoNuevo.UbicacionNombre
            };
            productoNuevo.UbicacionID = ubicacionesServicio.Insertar(ubicacion);

            Marcas marca = new Marcas
            {
                Nombre = productoNuevo.MarcaNombre
            };
            productoNuevo.MarcaID = marcasServicio.Insertar(marca);

            Arquetipos arquetipo = new Arquetipos
            {
                CategoriaID = productoNuevo.Categoria,
                Nombre = productoNuevo.Nombre
            };
            ProductoBase nuevoArquetipo = new ProductoBase
            {
                Nombre = arquetipo.Nombre,
                Categoria = arquetipo.CategoriaID
            };
            productoNuevo.ID = arquetiposServicio.Insertar(arquetipo);
            nuevoArquetipo.ID = productoNuevo.ID;
            indexacionServicio.IndexarProducto(nuevoArquetipo);

            Versiones version = new Versiones
            {
                ProductoID = productoNuevo.ID,
                RutaRelativaImagen = productoNuevo.RutaImagen,
                FormatoID = productoNuevo.FormatoProductoID,
                MarcaID = productoNuevo.MarcaID,
                EAN = productoNuevo.EAN
            };
            productoNuevo.ProductoVersionID = versionesServicio.Insertar(version);

            // Insertar nuevo registro
            productoNuevo.ProductoSKU = productoServicio.CrearProducto(productoNuevo);
            return productoNuevo.ProductoSKU;
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
