using System.Data.SqlClient;
using System.IO;
using Microsoft.Data.Sqlite;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Enums;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class RepoVersionesSQLite : IRepoEntidadGenerica<Versiones>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public readonly Dictionary<string, string> MapeoDTO;
        public RepoVersionesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ProductoID", "producto_id" },
                {"EAN", "EAN" },
                {"MarcaID", "Marca_id" },
                {"FormatoID", "formato_id" },
                {"RutaRelativaImagen" , "RutaRelativaImagen" },
                {"ID", "ID" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
            MapeoDTO = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Acceso en lector
                {"ID", "v.id" },
                {"ProductoID", "v.producto_id" },
                {"EAN", "v.ean" },
                {"FormatoID", "v.formato_id" },
                {"RutaRelativaImagen", "v.RutaRelativaImagen" },
                {"MarcaID", "v.marca_id" },
                {"EsEliminado", "v.EsEliminado" },
                {"FechaModificacion", "v.FechaModificacion" },
                {"FechaCreacion","v.FechaCreacion" }
            };
        }
        public async IAsyncEnumerable<Versiones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Versiones> RecuperarList()
        {
            List<Versiones> Versiones = new List<Versiones>();
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            Versiones.Add(registro);
                        }

                        return Versiones;
                    }
                }
            }
        }
        public List<Versiones> RecuperarLotePorIDS(string propiedadNombre, List<string> IDs)
        {
            List<Versiones> versiones = new List<Versiones>();
            string parametros = string.Join(", ", Enumerable.Range(0, IDs.Count).Select(i => $"@id{i}"));
            string consulta = $@"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE {MapeoDTO[propiedadNombre]} IN ({parametros});";
            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    for (int i = 0; i < IDs.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@id{i}", IDs[i]);
                    }

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            versiones.Add(registro);
                        }

                        return versiones;
                    }
                }
            }
        }
        public Versiones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Versiones registro = new Versiones();

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID);
                            registro.MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            registro.ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen));
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            registro.EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan);
                        };
                        

                        return registro;
                    }
                }
            }
        }
        public List<Versiones> BuscarEan(string EanBuscado)
        {
            List<Versiones> Versiones = new List<Versiones>();
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.ean = @EANBuscado;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@EANBuscado", EanBuscado);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            Versiones.Add(registro);
                        }

                        return Versiones;
                    }
                }
            }
        }
        public bool Modificar(Versiones registroModificado)
        {
            Versiones registroActual = Recuperar(registroModificado.ID);
            bool flagModificaciones = false;
            var propiedadesEntidad = typeof(Versiones).GetProperties();
            var listaPropiedadesModificadas = new List<string>();
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ID",
                "FechaCreacion",
                "EsEliminado",
                "FechaModificacion"
            };

            try
            {
                using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
                {
                    using(SqliteCommand comando = new SqliteCommand())
                    {
                        comando.Connection = accesoDB.ObtenerConexionDB();

                        foreach(var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            var valorActual = propiedad.GetValue(registroActual);
                            var valorModificado = propiedad.GetValue(registroModificado);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(registroModificado) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_versiones SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", registroModificado.ID);
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
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_versiones SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_versiones WHERE ID = @id;";
            }

            using(SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    comando.Parameters.AddWithValue("@id",ID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
        public string Insertar(Versiones nuevoRegistro) 
        {
            string consulta = @"INSERT INTO Productos_versiones 
                (   ID,
                    producto_id,
                    EAN,
                    Marca_id,
                    FechaCreacion,
                    formato_id,
                    RutaRelativaImagen)
                VALUES (
                    @ID,
                    @Producto_id,
                    @EAN,
                    @Marca_id,
                    @FechaCreacion,
                    @formato_id,
                    @RutaRelativaImagen);";
            string consultaBusqueda = @"SELECT ID AS VersionID FROM Productos_versiones 
                WHERE Producto_id = @productoID
                AND EAN = @eanProducto
                AND Marca_id = @marcaID
                AND formato_id = @formatoID
                AND RutaRelativaImagen = @rutaImagen;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@productoID", nuevoRegistro.ProductoID);
                    comandoBusqueda.Parameters.AddWithValue("@eanProducto", nuevoRegistro.EAN);
                    comandoBusqueda.Parameters.AddWithValue("@marcaID",nuevoRegistro.MarcaID);
                    comandoBusqueda.Parameters.AddWithValue("@formatoID",nuevoRegistro.FormatoID);
                    comandoBusqueda.Parameters.AddWithValue("@rutaImagen",nuevoRegistro.RutaRelativaImagen);

                    using(SqliteDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("VersionID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    string rutaImagen = nuevoRegistro.RutaRelativaImagen;
                    object RutaValidadaDB = string.IsNullOrWhiteSpace(rutaImagen) ? (object)DBNull.Value : rutaImagen;

                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@Producto_id", nuevoRegistro.ProductoID);
                    comando.Parameters.AddWithValue("@EAN", nuevoRegistro.EAN);
                    comando.Parameters.AddWithValue("@Marca_id", nuevoRegistro.MarcaID);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.Parameters.AddWithValue("@formato_id", nuevoRegistro.FormatoID);
                    comando.Parameters.AddWithValue("@RutaRelativaImagen", RutaValidadaDB);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
    }
    public class RepoVersionesSQLServer : IRepoEntidadGenerica<Versiones>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public readonly Dictionary<string, string> MapeoDTO;
        public RepoVersionesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ProductoID", "producto_id" },
                {"EAN", "EAN" },
                {"MarcaID", "Marca_id" },
                {"FormatoID", "formato_id" },
                {"RutaRelativaImagen" , "RutaRelativaImagen" },
                {"ID", "ID" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
            MapeoDTO = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Acceso en lector
                {"ID", "v.id" },
                {"ProductoID", "v.producto_id" },
                {"EAN", "v.ean" },
                {"FormatoID", "v.formato_id" },
                {"RutaRelativaImagen", "v.RutaRelativaImagen" },
                {"MarcaID", "v.marca_id" },
                {"EsEliminado", "v.EsEliminado" },
                {"FechaModificacion", "v.FechaModificacion" },
                {"FechaCreacion","v.FechaCreacion" }
            };
        }
        public async IAsyncEnumerable<Versiones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.EsEliminado = False;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Versiones> RecuperarList()
        {
            List<Versiones> Versiones = new List<Versiones>();
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.EsEliminado = False;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            Versiones.Add(registro);
                        }

                        return Versiones;
                    }
                }
            }
        }
        public List<Versiones> RecuperarLotePorIDS(string propiedadNombre, List<string> IDs)
        {
            List<Versiones> versiones = new List<Versiones>();
            string parametros = string.Join(", ", Enumerable.Range(0, IDs.Count).Select(i => $"@id{i}"));
            string consulta = $@"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE {MapeoDTO[propiedadNombre]} IN ({parametros});";
            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    for (int i = 0; i < IDs.Count; i++)
                    {
                        comando.Parameters.AddWithValue($"@id{i}", IDs[i]);
                    }

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            versiones.Add(registro);
                        }

                        return versiones;
                    }
                }
            }
        }
        public Versiones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Versiones registro = new Versiones();

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID);
                            registro.MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            registro.ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen));
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            registro.EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan);
                        }
                        ;


                        return registro;
                    }
                }
            }
        }
        public List<Versiones> BuscarEan(string EanBuscado)
        {
            List<Versiones> Versiones = new List<Versiones>();
            string consulta = @"SELECT 
                    v.id AS VersionID,
                    v.producto_id AS ProductoID,
                    v.ean AS EAN,
                    v.marca_id AS MarcaID,
                    v.formato_id AS FormatoID,
                    v.RutaRelativaImagen AS RutaRelativaImagen,
                    v.FechaCreacion AS FechaCreacion,
                    v.FechaModificacion AS FechaModificacion,
                    v.EsEliminado AS EsEliminado
                FROM Productos_versiones AS v
                WHERE v.ean = @EANBuscado;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@EANBuscado", EanBuscado);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("VersionID");
                        int IDXProductoID = lector.GetOrdinal("ProductoID");
                        int IDXEan = lector.GetOrdinal("EAN");
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");
                        int IDXFormatoID = lector.GetOrdinal("FormatoID");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Versiones registro = new Versiones
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                FormatoID = lector.IsDBNull(IDXFormatoID) ? "" : lector.GetString(IDXFormatoID),
                                MarcaID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                ProductoID = lector.IsDBNull(IDXProductoID) ? "" : lector.GetString(IDXProductoID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                            Versiones.Add(registro);
                        }

                        return Versiones;
                    }
                }
            }
        }
        public bool Modificar(Versiones registroModificado)
        {
            Versiones registroActual = Recuperar(registroModificado.ID);
            var propiedadesEntidad = typeof(Versiones).GetProperties();
            var listaPropiedadesModificadas = new List<string>();
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ID",
                "FechaCreacion",
                "EsEliminado",
                "FechaModificacion"
            };

            try
            {
                using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand())
                    {
                        comando.Connection = accesoDB.ObtenerConexionDB();

                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            var valorActual = propiedad.GetValue(registroActual);
                            var valorModificado = propiedad.GetValue(registroModificado);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(registroModificado) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_versiones SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", registroModificado.ID);
                        comando.CommandText = Consulta;

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_versiones SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_versiones WHERE ID = @id;";
            }

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
        public string Insertar(Versiones nuevoRegistro)
        {
            string consulta = @"INSERT INTO Productos_versiones 
                (   ID,
                    producto_id,
                    EAN,
                    Marca_id,
                    FechaCreacion,
                    formato_id,
                    RutaRelativaImagen)
                VALUES (
                    @ID,
                    @Producto_id,
                    @EAN,
                    @Marca_id,
                    @FechaCreacion,
                    @formato_id,
                    @RutaRelativaImagen);";
            string consultaBusqueda = @"SELECT ID AS VersionID FROM Productos_versiones 
                WHERE Producto_id = @productoID
                AND EAN = @eanProducto
                AND Marca_id = @marcaID
                AND formato_id = @formatoID
                AND RutaRelativaImagen = @rutaImagen;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comandoBusqueda = new SqlCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@productoID", nuevoRegistro.ProductoID);
                    comandoBusqueda.Parameters.AddWithValue("@eanProducto", nuevoRegistro.EAN);
                    comandoBusqueda.Parameters.AddWithValue("@marcaID", nuevoRegistro.MarcaID);
                    comandoBusqueda.Parameters.AddWithValue("@formatoID", nuevoRegistro.FormatoID);
                    comandoBusqueda.Parameters.AddWithValue("@rutaImagen", nuevoRegistro.RutaRelativaImagen);

                    using (SqlDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("VersionID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    string rutaImagen = nuevoRegistro.RutaRelativaImagen;
                    object RutaValidadaDB = string.IsNullOrWhiteSpace(rutaImagen) ? (object)DBNull.Value : rutaImagen;

                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@Producto_id", nuevoRegistro.ProductoID);
                    comando.Parameters.AddWithValue("@EAN", nuevoRegistro.EAN);
                    comando.Parameters.AddWithValue("@Marca_id", nuevoRegistro.MarcaID);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.Parameters.AddWithValue("@formato_id", nuevoRegistro.FormatoID);
                    comando.Parameters.AddWithValue("@RutaRelativaImagen", RutaValidadaDB);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
    }
}
