using Microsoft.Data.Sqlite;
using NPOI.SS.Formula.Functions;
using WPFApp1.Entidades;

namespace WPFApp1.Repositorios
{
    public class ProductosDesarrollo
    {
        private readonly ConexionDBSQLite _accesoDB;
        public readonly Dictionary<string,string> MapeoColumnas;
        public ProductosDesarrollo(ConexionDBSQLite accesoDB)
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
            string Consulta = @"
            SELECT 
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
                f.alto AS Altura,
                f.ancho AS Ancho,
                f.largo AS Largo,
                f.peso AS Peso,
                p.nombre AS ProductoNombre,
                p.categoria AS ProductoCategoria
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            WHERE s.SKU_Producto = @IdBuscada
                AND s.haber > 0
                AND s.EsEliminado = FALSE;";

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
                            int IDXAltura = lector.GetOrdinal("Altura");
                            int IDXAncho = lector.GetOrdinal("Ancho");
                            int IDXLargo = lector.GetOrdinal("Largo");
                            int IDXHaber = lector.GetOrdinal("ProductoHaber");
                            int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                            int IDXNombre = lector.GetOrdinal("ProductoNombre");
                            int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                            int IDXEan = lector.GetOrdinal("EAN");
                            int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                            int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                            int IDXMarcaID = lector.GetOrdinal("MarcaID");
                            int IDXFormatoID = lector.GetOrdinal("FormatoID");
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
                            registro.Altura = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura);
                            registro.Ancho = lector.IsDBNull(IDXAncho) ? 0 : lector.GetInt32(IDXAncho);
                            registro.Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetInt32(IDXLargo);
                            registro.Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber);
                            registro.Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio);

                            //Cadena
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.Categoria = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria);
                            registro.EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan);
                            registro.RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : lector.GetString(IDXRutaImagen);

                            //Claves
                            registro.UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID);
                            registro.MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            registro.FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID);
                            registro.ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID);
                            registro.ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID);

                            //Datetime
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);

                            //Booleanos
                            registro.EsEliminado = lector.IsDBNull(IDXProductoEsEliminado) ? false : lector.GetBoolean(IDXProductoEsEliminado);
                            registro.PrecioPublico = lector.IsDBNull(IDXProductoPrecioPublico) ? false : lector.GetBoolean(IDXProductoPrecioPublico);
                            registro.VisibilidadWeb = lector.IsDBNull(IDXProductoVisibilidadWeb) ? false : lector.GetBoolean(IDXProductoVisibilidadWeb);

                            //--PK--
                            registro.ProductoSKU = lector.GetString(IDXProductoSKU);

                            //TODO registro.Categoria se asigna en el servicio orquestador
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
        public bool CrearProducto(ProductoCatalogo nuevoProducto)
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

                    return comando.ExecuteNonQuery() > 0;
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
                f.alto AS Altura,
                f.ancho AS Ancho,
                f.largo AS Largo,
                f.peso AS Peso,
                p.nombre AS ProductoNombre,
                p.categoria AS ProductoCategoria
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
                WHERE s.EsEliminado = False;";

            using (var conexion = _accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (var comando = new SqliteCommand(Consulta, conexion))
                {
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        // Indices
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXAncho = lector.GetOrdinal("Ancho");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXHaber = lector.GetOrdinal("ProductoHaber");
                        int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                        int IDXNombre = lector.GetOrdinal("ProductoNombre");
                        int IDXCategoria = lector.GetOrdinal("ProductoCategoria");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXUbicacionID = lector.GetOrdinal("ProductoHaberUbicacionID");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
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
                                Altura = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura),
                                Ancho = lector.IsDBNull(IDXAncho) ? 0 : lector.GetInt32(IDXAncho),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetInt32(IDXLargo),
                                Haber = lector.IsDBNull(IDXHaber) ? 0 : lector.GetInt32(IDXHaber),
                                Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),

                                //Cadena
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Categoria = lector.IsDBNull(IDXCategoria) ? "" : lector.GetString(IDXCategoria),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan),
                                RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : lector.GetString(IDXRutaImagen),

                                //Claves
                                UbicacionID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                FormatoProductoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                ProductoVersionID = lector.IsDBNull(IDXProductoVersionID) ? "" : lector.GetString(IDXProductoVersionID),
                                ID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),

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
        public bool ModificarProducto(ProductoCatalogo productoModificado)
        {
            ProductoCatalogo productoActual = RecuperarProductoPorID(productoModificado.ProductoSKU);
            bool registroModificado = false;

            //Campos a ignorar
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "SKU_Producto",
                "FechaCreacion"
            };
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaPropiedadesModificadas = new List<string>();

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

                    string Consulta = $"UPDATE Productos_stock SET {string.Join(", ", listaPropiedadesModificadas)} WHERE SKU_Producto = @IDProductoModificado";
                    comando.Parameters.AddWithValue("@IDProductoModificado", productoModificado.ProductoSKU);
                    comando.CommandText = Consulta;

                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
        public bool EliminarProducto(string ProductoID)
        {
            //TODO
            try
            {
                string consulta = "UPDATE Producto_Stock";
                using (SqliteCommand comando = new SqliteCommand())
                {

                }
            }
            catch
            {
                throw;
            }

            return false;
        }
    }
}
