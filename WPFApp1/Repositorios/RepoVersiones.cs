using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data.SqlClient;

namespace WPFApp1.Repositorios
{
    public class RepoVersionesSQLite : IRepoEntidadGenerica<Versiones>
    {
        public readonly ConexionDBSQLite accesoDB;
        public RepoVersionesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
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
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
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
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                        }

                        return Versiones;
                    }
                }
            }
        }
        public bool Eliminar(string ID) => throw new NotImplementedException();
        public bool Modificar(Versiones registroModificado) => throw new NotImplementedException();
        public string Insertar(Versiones nuevoRegistro) => throw new NotImplementedException();
        public Versiones Recuperar(string ID) => throw new NotImplementedException();
    }
    public class RepoVersionesSQLServer : IRepoEntidadGenerica<Versiones>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public RepoVersionesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
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
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
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
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
                                RutaRelativaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                EAN = lector.IsDBNull(IDXEan) ? "" : lector.GetString(IDXEan)
                            };
                        }

                        return Versiones;
                    }
                }
            }
        }
        public bool Eliminar(string ID) => throw new NotImplementedException();
        public bool Modificar(Versiones registroModificado) => throw new NotImplementedException();
        public string Insertar(Versiones nuevoRegistro) => throw new NotImplementedException();
        public Versiones Recuperar(string ID) => throw new NotImplementedException();
    }
}
