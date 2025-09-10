using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class ProductosDesarrollo
    {
        private readonly ConexionDBSQLite _accesoDB;
        public ProductosDesarrollo(ConexionDBSQLite accesoDB)
        {
            _accesoDB = accesoDB;
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
                AND s.EsEliminado = 0;";
            
            try
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IdBuscada",ProductoID);
                    using(SqliteDataReader lector = comando.ExecuteReader())
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
                            registro.Haber = lector.IsDBNull(IDXHaber) ? 0: lector.GetInt32(IDXHaber);
                            registro.Precio = lector.IsDBNull(IDXPrecio) ? 0: lector.GetDecimal(IDXPrecio);

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
                            registro.FechaCreacion = lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.GetDateTime(IDXFechaModificacion);

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
            catch(SqliteException ex)
            {
                throw;
            }
            return registro;
        }
    }
}
