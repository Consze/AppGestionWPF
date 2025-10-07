using System.Data.SqlClient;
using System.IO;
using Microsoft.Data.Sqlite;
using WPFApp1.DTOS;
using WPFApp1.Enums;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    /// <summary>
    /// Implementación para DBMS local SQLite
    /// </summary>
    public class ProductosAcessoDatosSQLite : IProductosAccesoDatos
    {
        private readonly ConexionDBSQLite _accesoDB;
        public readonly Dictionary<string,string> MapeoColumnas;
        public ProductosAcessoDatosSQLite(ConexionDBSQLite accesoDB)
        {
            _accesoDB = accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ProductoSKU", "SKU_Producto" },
                {"UbicacionID", "ubicacion_id" },
                {"ProductoVersionID", "producto_version_id" },
                {"Haber" , "Haber" },
                {"Precio", "Precio" },
                {"EsEliminado", "EsEliminado" },
                {"VisibilidadWeb","VisibilidadWeb" },
                {"PrecioPublico","PrecioPublico" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public ProductoCatalogo RecuperarProductoPorID(string ProductoID)
        {
            ProductoCatalogo registro = new ProductoCatalogo();
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.SKU_Producto = @IdBuscada
                AND s.haber > 0;";

            try
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IdBuscada", ProductoID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            //indices
                            int IDXAlto = lector.GetOrdinal("Alto");
                            int IDXProfundidad = lector.GetOrdinal("Profundidad");
                            int IDXLargo = lector.GetOrdinal("Largo");
                            int IDXPeso = lector.GetOrdinal("Peso");
                            int IDXHaber = lector.GetOrdinal("ProductoHaber");
                            int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                            int IDXNombre = lector.GetOrdinal("ProductoNombre");
                            int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                            int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                            int IDXEan = lector.GetOrdinal("EAN");
                            int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                            int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                            int IDXMarcaID = lector.GetOrdinal("MarcaID");
                            int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                            int IDXFormatoID = lector.GetOrdinal("FormatoID");
                            int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                            int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                            int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                            int IDXProductoID = lector.GetOrdinal("ProductoID");
                            int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                            int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                            int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                            int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                            int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                            int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                            //__


                            //Numericos
                            registro.Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto);
                            registro.Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad);
                            registro.Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo);
                            registro.Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber);
                            registro.Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio);
                            registro.Peso= lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso);

                            //Cadena
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria);
                            registro.EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan);
                            registro.RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen));
                            registro.MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre);
                            registro.UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre);
                            registro.FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre);

                            //Claves
                            registro.UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID);
                            registro.MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            registro.FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID);
                            registro.ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID);
                            registro.ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID);
                            registro.Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);

                            //Datetime
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);

                            //Booleanos
                            registro.EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado);
                            registro.PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico);
                            registro.VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb);

                            //--PK--
                            registro.ProductoSKU = lector.GetString(IDXProductoSKU);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw;
            }
            return registro;
        }
        public List<ProductoCatalogo> RecuperarLotePorID(List<string> SKUs)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, SKUs.Count).Select(i => $"@sku{i}"));
            string Consulta = $@"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.SKU_Producto IN ({parametros});";

            List<ProductoCatalogo> listaRegistros = new List<ProductoCatalogo>();
            using (SqliteConnection conexion = _accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, conexion))
                {
                    for (int i = 0; i < SKUs.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@sku{i}", SKUs[i]);
                    }

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        // Indices
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                        int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                        int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                        int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                        int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                        while (lector.Read())
                        {
                            ProductoCatalogo registroActual = new ProductoCatalogo
                            {
                                ProductoSKU = lector.GetString(IDXProductoSKU),

                                //Numericos
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                //Datetime
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                //Booleanos
                                EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                            };

                            listaRegistros.Add(registroActual);
                        }
                    }
                }
            }
            return listaRegistros;
        }
        public string CrearProducto(ProductoCatalogo nuevoProducto)
        {
            string Consulta = @"INSERT INTO Productos_Stock (
                SKU_Producto,
                ubicacion_id,
                producto_version_id,
                Haber,
                Precio,
                VisibilidadWeb,
                PrecioPublico,
                FechaCreacion
            ) VALUES (
                @SKU, @UbicacionID, @producto_version_id, @Haber, @Precio, @VisibilidadWeb, @PrecioPublico, @FechaCreacion
            )";

            try
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@SKU", nuevoProducto.ProductoSKU);
                    comando.Parameters.AddWithValue("@UbicacionID", nuevoProducto.UbicacionID);
                    comando.Parameters.AddWithValue("@producto_version_id", nuevoProducto.ProductoVersionID);
                    comando.Parameters.AddWithValue("@Haber", nuevoProducto.Haber);
                    comando.Parameters.AddWithValue("@Precio", nuevoProducto.Precio);
                    comando.Parameters.AddWithValue("@VisibilidadWeb", nuevoProducto.VisibilidadWeb);
                    comando.Parameters.AddWithValue("@PrecioPublico", nuevoProducto.PrecioPublico);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoProducto.ProductoSKU;
                }
            }
            catch (SqliteException ex)
            {
                throw;
            }
        }
        public async IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync()
        {
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.EsEliminado = False
                AND s.haber > 0;";

            using (var conexion = _accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (var comando = new SqliteCommand(Consulta, conexion))
                {
                    using (var lector = await comando.ExecuteReaderAsync())
                        {
                            // Indices
                            int IDXAlto = lector.GetOrdinal("Alto");
                            int IDXProfundidad = lector.GetOrdinal("Profundidad");
                            int IDXLargo = lector.GetOrdinal("Largo");
                            int IDXPeso = lector.GetOrdinal("Peso");
                            int IDXHaber = lector.GetOrdinal("ProductoHaber");
                            int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                            int IDXNombre = lector.GetOrdinal("ProductoNombre");
                            int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                            int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                            int IDXEan = lector.GetOrdinal("EAN");
                            int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                            int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                            int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                            int IDXMarcaID = lector.GetOrdinal("MarcaID");
                            int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                            int IDXFormatoID = lector.GetOrdinal("FormatoID");
                            int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                            int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                            int IDXProductoID = lector.GetOrdinal("ProductoID");
                            int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                            int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                            int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                            int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                            int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                            int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                            while (await lector.ReadAsync())
                            {
                                var registroActual = new ProductoCatalogo
                                {
                                    ProductoSKU = lector.GetString(IDXProductoSKU),

                                    //Numericos
                                    Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                    Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                    Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                    Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                    Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                    Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                    //Cadena
                                    Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                    CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                    EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                    RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                    MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                    UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                    FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                    //Claves
                                    UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                    MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                    FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                    ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                    ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                    Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                    //Datetime
                                    FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                    FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                    //Booleanos
                                    EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                    PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                    VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                                };

                                yield return registroActual;
                            }
                        }
                }
            }
        }
        public List<ProductoCatalogo> LeerProductos()
        {
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.EsEliminado = False
                AND s.haber > 0;";
            List<ProductoCatalogo> listaRegistros = new List<ProductoCatalogo>();
            using (SqliteConnection conexion = _accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        // Indices
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                        int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                        int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                        int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                        int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                        while (lector.Read())
                        {
                            ProductoCatalogo registroActual = new ProductoCatalogo
                            {
                                ProductoSKU = lector.GetString(IDXProductoSKU),

                                //Numericos
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                //Datetime
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                //Booleanos
                                EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                            };

                            listaRegistros.Add(registroActual);
                        }
                    }
                }
            }
            return listaRegistros;
        }
        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = RecuperarProductoPorID(productoModificado.ProductoSKU);
            bool registroModificado = false;

            //Campos a ignorar
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "SKU_Producto",
                "FechaCreacion",
                "FechaModificacion",
                "CategoriaNombre",
                "UbicacionNombre",
                "MarcaNombre",
                "EAN",
                "RutaImagen",
                "Nombre",
                "ID",
                "FormatoProductoID",
                "FormatoNombre",
                "Categoria",
                "MarcaID",
                "Alto",
                "Profundidad",
                "Largo",
                "Peso"
            };
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaPropiedadesModificadas = new List<string>();

            try
            {
                using (var conexion = _accesoDB.ObtenerConexionDB())
                {
                    conexion.Open();
                    using (var comando = new SqliteCommand())
                    {
                        comando.Connection = conexion;

                        //bucle de construccion de consulta
                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            //Comprobar diferencia
                            var valorActual = propiedad.GetValue(productoActual);
                            var valorModificado = propiedad.GetValue(productoModificado);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(productoModificado) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_stock SET {string.Join(", ", listaPropiedadesModificadas)} WHERE SKU_Producto = @IDProductoModificado";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDProductoModificado", productoModificado.ProductoSKU);
                        comando.CommandText = Consulta;

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch(SqliteException ex)
            {
                throw;
            }
        }
        public bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion)
        {
            try
            {
                string consulta = "";
                if(TipoEliminacion == TipoEliminacion.Logica)
                {
                    consulta = "UPDATE Productos_stock SET Haber = 0 WHERE SKU_Producto = @IDProducto";
                }
                else
                {
                    consulta = "DELETE FROM Productos_stock WHERE SKU_Producto = @IDProducto";
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IDProducto", ProductoID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }

            }
            catch(SqliteException ex)
            {
                throw;
            }
        }
        public bool ModificacionMasiva(List<ProductoSKU_Propiedad_Valor> lista)
        {
            if (lista.Count == 0)
                return false;

            string PropiedadNombre = lista[0].PropiedadNombre;
            Dictionary<string, (string Nombre, string Definicion, SqliteType TipoDriver)> MapeoPropiedades;
            MapeoPropiedades = new Dictionary<string, (string, string, SqliteType)>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , nombre de Columna, definicion
                {"UbicacionID", ("ubicacion_id", "VARCHAR(36)", SqliteType.Text)},
                {"ProductoVersionID", ("producto_version_id", "VARCHAR(36)", SqliteType.Text)},
                {"Haber" , ("Haber", "INT", SqliteType.Integer)},
                {"Precio", ("Precio", "NUMERIC (18,2)", SqliteType.Real)},
                {"EsEliminado", ("EsEliminado", "BOOLEAN", SqliteType.Integer)},
                {"VisibilidadWeb", ("VisibilidadWeb", "BOOLEAN", SqliteType.Integer)},
                {"PrecioPublico", ("PrecioPublico", "BOOLEAN", SqliteType.Integer)},
                {"ProductoSKU", ("SKU_Producto", "VARCHAR(36)", SqliteType.Text)}
            };

            string consultaCreacion = $@"CREATE TABLE TempActualizacion (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductoSKU VARCHAR(36),
                    NuevoValor {MapeoPropiedades[PropiedadNombre].Definicion},
                    FOREIGN KEY(ProductoSKU) REFERENCES Productos_stock(SKU_Producto));";

            
            using(SqliteConnection conexion = _accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoCreacion = new SqliteCommand(consultaCreacion, conexion))
                {
                    comandoCreacion.ExecuteNonQuery();
                }

                using (var transacccion = conexion.BeginTransaction())
                {
                    string consulta = @"INSERT INTO TempActualizacion (ProductoSKU, NuevoValor) VALUES (@productoSKU, @propiedadValor);";

                    using (SqliteCommand comando = new SqliteCommand(consulta, conexion, transacccion))
                    {
                        comando.Parameters.Add("@productoSKU", MapeoPropiedades["ProductoSKU"].TipoDriver);
                        comando.Parameters.Add("@propiedadValor", MapeoPropiedades[PropiedadNombre].TipoDriver);

                        foreach (var item in lista)
                        {
                            comando.Parameters["@productoSKU"].Value = item.ProductoSKU;
                            comando.Parameters["@propiedadValor"].Value = item.Valor ?? DBNull.Value;
                            comando.ExecuteNonQuery();
                        }
                        transacccion.Commit();
                    }
                }

                string consultaActualizacion = $@"
                    UPDATE Productos_stock
                    SET {MapeoPropiedades[PropiedadNombre].Nombre} = T.NuevoValor,
                        FechaModificacion = datetime('now')
                    FROM TempActualizacion AS T
                    WHERE Productos_stock.SKU_Producto = T.ProductoSKU;";
                string consultaEliminacion = @"DROP TABLE IF EXISTS TempActualizacion;";
                int filasAfectadas = 0;


                using (SqliteCommand comando = new SqliteCommand(consultaActualizacion, conexion))
                {
                    filasAfectadas = comando.ExecuteNonQuery();
                }

                using(SqliteCommand comando = new SqliteCommand(consultaEliminacion, conexion))
                {
                    comando.ExecuteNonQuery();
                }

                return filasAfectadas > 0;
            }
        }
    }

    /// <summary>
    /// Implementación para DBMS SQL Server
    /// </summary>
    public class ProductosAccesoDatosSQLServer : IProductosAccesoDatos
    {
        public readonly ConexionDBSQLServer _accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public ProductosAccesoDatosSQLServer(ConexionDBSQLServer accesoDB)
        {
            _accesoDB = accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ProductoSKU", "SKU_Producto" },
                {"UbicacionID", "ubicacion_id" },
                {"Haber" , "Haber" },
                {"Precio", "Precio" },
                {"EsEliminado", "EsEliminado" },
                {"VisibilidadWeb","VisibilidadWeb" },
                {"PrecioPublico","PrecioPublico" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public ProductoCatalogo RecuperarProductoPorID(string ProductoID)
        {
            ProductoCatalogo registro = new ProductoCatalogo();
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.SKU_Producto = @IdBuscada
                AND s.haber > 0;";

            try
            {
                using (SqlCommand comando = new SqlCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IdBuscada", ProductoID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            //indices
                            int IDXAlto = lector.GetOrdinal("Alto");
                            int IDXProfundidad = lector.GetOrdinal("Profundidad");
                            int IDXLargo = lector.GetOrdinal("Largo");
                            int IDXPeso = lector.GetOrdinal("Peso");
                            int IDXHaber = lector.GetOrdinal("ProductoHaber");
                            int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                            int IDXNombre = lector.GetOrdinal("ProductoNombre");
                            int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                            int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                            int IDXEan = lector.GetOrdinal("EAN");
                            int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                            int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                            int IDXMarcaID = lector.GetOrdinal("MarcaID");
                            int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                            int IDXFormatoID = lector.GetOrdinal("FormatoID");
                            int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                            int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                            int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                            int IDXProductoID = lector.GetOrdinal("ProductoID");
                            int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                            int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                            int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                            int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                            int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                            int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                            //__


                            //Numericos
                            registro.Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto);
                            registro.Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad);
                            registro.Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo);
                            registro.Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber);
                            registro.Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio);
                            registro.Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso);

                            //Cadena
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria);
                            registro.EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan);
                            registro.RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen));
                            registro.MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre);
                            registro.UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre);
                            registro.FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre);

                            //Claves
                            registro.UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID);
                            registro.MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            registro.FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID);
                            registro.ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID);
                            registro.ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID);
                            registro.Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);

                            //Datetime
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);

                            //Booleanos
                            registro.EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado);
                            registro.PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico);
                            registro.VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb);

                            //--PK--
                            registro.ProductoSKU = lector.GetString(IDXProductoSKU);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
            return registro;
        }
        public List<ProductoCatalogo> RecuperarLotePorID(List<string> SKUs)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, SKUs.Count).Select(i => $"@sku{i}"));
            string Consulta = $@"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.SKU_Producto IN ({parametros});";

            List<ProductoCatalogo> listaRegistros = new List<ProductoCatalogo>();
            using (SqlConnection conexion = _accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(Consulta, conexion))
                {
                    for (int i = 0; i < SKUs.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@sku{i}", SKUs[i]);
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        // Indices
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                        int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                        int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                        int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                        int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                        while (lector.Read())
                        {
                            ProductoCatalogo registroActual = new ProductoCatalogo
                            {
                                ProductoSKU = lector.GetString(IDXProductoSKU),

                                //Numericos
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                //Datetime
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                //Booleanos
                                EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                            };

                            listaRegistros.Add(registroActual);
                        }
                    }
                }
            }
            return listaRegistros;
        }
        public string CrearProducto(ProductoCatalogo nuevoProducto)
        {
            string Consulta = @"INSERT INTO Productos_Stock (
                SKU_Producto,
                ubicacion_id,
                producto_version_id,
                Haber,
                Precio,
                VisibilidadWeb,
                PrecioPublico,
                FechaCreacion
            ) VALUES (
                @SKU, @UbicacionID, @producto_version_id, @Haber, @Precio, @VisibilidadWeb, @PrecioPublico, @FechaCreacion
            )";

            try
            {
                using (SqlCommand comando = new SqlCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@SKU", nuevoProducto.ProductoSKU);
                    comando.Parameters.AddWithValue("@UbicacionID", nuevoProducto.UbicacionID);
                    comando.Parameters.AddWithValue("@producto_version_id", nuevoProducto.ProductoVersionID);
                    comando.Parameters.AddWithValue("@Haber", nuevoProducto.Haber);
                    comando.Parameters.AddWithValue("@Precio", nuevoProducto.Precio);
                    comando.Parameters.AddWithValue("@VisibilidadWeb", nuevoProducto.VisibilidadWeb);
                    comando.Parameters.AddWithValue("@PrecioPublico", nuevoProducto.PrecioPublico);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoProducto.ProductoSKU;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        public async IAsyncEnumerable<ProductoCatalogo> LeerProductosAsync()
        {
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.EsEliminado = False
                AND s.haber > 0;";

            using (var conexion = _accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (var comando = new SqlCommand(Consulta, conexion))
                {
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        // Indices
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                        int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                        int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                        int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                        int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                        while (await lector.ReadAsync())
                        {
                            var registroActual = new ProductoCatalogo
                            {
                                ProductoSKU = lector.GetString(IDXProductoSKU),

                                //Numericos
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                //Datetime
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                //Booleanos
                                EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                            };

                            yield return registroActual;
                        }
                    }
                }
            }
        }
        public List<ProductoCatalogo> LeerProductos()
        {
            string Consulta = @"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.ubicacion_id AS ProductoHaberUbicacionID,
                s.producto_version_id AS ProductoVersionID,
                s.haber AS ProductoHaber,
                s.precio AS ProductoPrecio,
                s.VisibilidadWeb AS ProductoVisibilidadWeb,
                s.PrecioPublico AS ProductoPrecioPublico,
                s.FechaModificacion AS ProductoFechaModificacion,
                s.FechaCreacion AS ProductoFechaCreacion,
                s.EsEliminado AS ProductoEsEliminado,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                v.EAN AS EAN,
                v.producto_id AS ProductoID,
                v.Marca_id AS MarcaID,
                v.formato_id AS FormatoID,
                f.alto AS Alto,
                f.profundidad AS Profundidad,
                f.largo AS Largo,
                f.peso AS Peso,
                f.descripcion AS FormatoNombre,
                p.nombre AS ProductoNombre,
                p.categoria_id AS CategoriaID,
                c.nombre AS ProductoCategoria,
                m.nombre AS MarcaNombre,
                u.descripcion AS UbicacionNombre
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Marcas AS m ON v.Marca_id = m.id
            INNER JOIN Ubicaciones_inventario AS u ON s.ubicacion_id = u.id
            WHERE s.EsEliminado = False
                AND s.haber > 0;";
            List<ProductoCatalogo> listaRegistros = new List<ProductoCatalogo>();
            using (SqlConnection conexion = _accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(Consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        // Indices
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXMarcaNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXFormatoNombre = lector.GetOrdinal("FormatoNombre");
                        int IDXUbicacionNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXProductoVersionID = lector.GetOrdinal("ProductoVersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXFechaCreacion = lector.GetOrdinal("ProductoFechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("ProductoFechaModificacion");
                        int IDXProductoEsEliminado = lector.GetOrdinal("ProductoEsEliminado");
                        int IDXProductoPrecioPublico = lector.GetOrdinal("ProductoPrecioPublico");
                        int IDXProductoVisibilidadWeb = lector.GetOrdinal("ProductoVisibilidadWeb");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");

                        while (lector.Read())
                        {
                            ProductoCatalogo registroActual = new ProductoCatalogo
                            {
                                ProductoSKU = lector.GetString(IDXProductoSKU),

                                //Numericos
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                                Peso = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPeso),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaNombre = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                MarcaNombre = lector.IsDBNull(IDXMarcaNombre) ? "" : lector.GetString(IDXMarcaNombre),
                                UbicacionNombre = lector.IsDBNull(IDXUbicacionNombre) ? "" : lector.GetString(IDXUbicacionNombre),
                                FormatoNombre = lector.IsDBNull(IDXFormatoNombre) ? "" : lector.GetString(IDXFormatoNombre),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                Categoria = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),

                                //Datetime
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),

                                //Booleanos
                                EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado),
                                PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico),
                                VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb)
                            };

                            listaRegistros.Add(registroActual);
                        }
                    }
                }
            }
            return listaRegistros;
        }
        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = RecuperarProductoPorID(productoModificado.ProductoSKU);
            bool registroModificado = false;

            //Campos a ignorar
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "SKU_Producto",
                "FechaCreacion",
                "FechaModificacion",
                "CategoriaNombre",
                "UbicacionNombre",
                "MarcaNombre",
                "EAN",
                "RutaImagen",
                "Nombre",
                "ID",
                "FormatoProductoID",
                "FormatoNombre",
                "Categoria",
                "MarcaID",
                "Alto",
                "Profundidad",
                "Largo",
                "Peso"
            };
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaPropiedadesModificadas = new List<string>();

            try
            {
                using (var conexion = _accesoDB.ObtenerConexionDB())
                {
                    conexion.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexion;

                        //bucle de construccion de consulta
                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            //Comprobar diferencia
                            var valorActual = propiedad.GetValue(productoActual);
                            var valorModificado = propiedad.GetValue(productoModificado);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(productoModificado) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_stock SET {string.Join(", ", listaPropiedadesModificadas)} WHERE SKU_Producto = @IDProductoModificado";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDProductoModificado", productoModificado.ProductoSKU);
                        comando.CommandText = Consulta;

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
        }
        public bool EliminarProducto(string ProductoID, TipoEliminacion TipoEliminacion)
        {
            try
            {
                string consulta = "";
                if (TipoEliminacion == TipoEliminacion.Logica)
                {
                    consulta = "UPDATE Productos_stock SET Haber = 0 WHERE SKU_Producto = @IDProducto";
                }
                else
                {
                    consulta = "DELETE FROM Productos_stock WHERE SKU_Producto = @IDProducto";
                }

                using (SqlCommand comando = new SqlCommand(consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IDProducto", ProductoID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }

            }
            catch(SqlException ex)
            {
                throw;
            }
        }
        public bool ModificacionMasiva(List<ProductoSKU_Propiedad_Valor> lista)
        {
            throw new NotImplementedException("Metodo temporalmente sin implementación para SQL Server");
        }
    }
}
