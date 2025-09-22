using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class RepoFormatosSQLite : IRepoEntidadGenerica<Formatos>
    {
        public readonly ConexionDBSQLite accesoDB;
        public RepoFormatosSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
        }
        public async IAsyncEnumerable<Formatos> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    f.id AS ID,
                    f.descripcion AS Nombre,
                    f.alto AS Alto,
                    f.largo AS Largo,
                    f.profundidad AS Profundidad,
                    f.peso AS Peso,
                    f.EsEliminado AS EsEliminado,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion
                FROM Productos_formatos AS f
                WHERE f.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("ID");
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while(await lector.ReadAsync()) {
                            Formatos registro = new Formatos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Formatos> RecuperarList()
        {
            List<Formatos> formatos = new List<Formatos>();
            string consulta = @"SELECT 
                    f.id AS ID,
                    f.descripcion AS Nombre,
                    f.alto AS Alto,
                    f.largo AS Largo,
                    f.profundidad AS Profundidad,
                    f.peso AS Peso,
                    f.EsEliminado AS EsEliminado,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion
                FROM Productos_formatos AS f
                WHERE f.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector =  comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ID");
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Formatos registro = new Formatos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            formatos.Add(registro);
                        }

                        return formatos;
                    }
                }
            }
        }
        public bool Eliminar(string ID) => throw new NotImplementedException();
        public bool Modificar(Formatos registroModificado) => throw new NotImplementedException();
        public string Insertar(Formatos nuevoRegistro) => throw new NotImplementedException();
        public Formatos Recuperar(string ID) => throw new NotImplementedException();
    }

    public class RepoFormatosSQLServer : IRepoEntidadGenerica<Formatos>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public RepoFormatosSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
        }
        public async IAsyncEnumerable<Formatos> RecuperarStreamAsync()
        {
            List<Formatos> formatos = new List<Formatos>();
            string consulta = @"SELECT 
                    f.id AS ID,
                    f.descripcion AS Nombre,
                    f.alto AS Alto,
                    f.largo AS Largo,
                    f.profundidad AS Profundidad,
                    f.peso AS Peso,
                    f.EsEliminado AS EsEliminado,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion
                FROM Productos_formatos AS f
                WHERE f.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("ID");
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Formatos registro = new Formatos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Formatos> RecuperarList()
        {
            List<Formatos> formatos = new List<Formatos>();
            string consulta = @"SELECT 
                    f.id AS ID,
                    f.descripcion AS Nombre,
                    f.alto AS Alto,
                    f.largo AS Largo,
                    f.profundidad AS Profundidad,
                    f.peso AS Peso,
                    f.EsEliminado AS EsEliminado,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion
                FROM Productos_formatos AS f
                WHERE f.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ID");
                        int IDXAlto = lector.GetOrdinal("Alto");
                        int IDXLargo = lector.GetOrdinal("Largo");
                        int IDXProfundidad = lector.GetOrdinal("Profundidad");
                        int IDXPeso = lector.GetOrdinal("Peso");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Formatos registro = new Formatos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto),
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXAlto),
                                Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad),
                                Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            formatos.Add(registro);
                        }

                        return formatos;
                    }
                }
            }
        }
        public bool Eliminar(string ID) => throw new NotImplementedException();
        public bool Modificar(Formatos registroModificado) => throw new NotImplementedException();
        public string Insertar(Formatos nuevoRegistro) => throw new NotImplementedException();
        public Formatos Recuperar(string ID) => throw new NotImplementedException();
    }
}
