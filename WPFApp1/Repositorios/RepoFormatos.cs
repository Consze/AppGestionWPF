using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
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

                        while(await lector.ReadAsync()) {
                            Formatos registro = new Formatos
                            {
                                ID = lector.GetString(IDXID),
                                Nombre = lector.GetString(IDXNombre),
                                Alto = lector.GetDecimal(IDXAlto),
                                Largo = lector.GetDecimal(IDXLargo),
                                Profundidad = lector.GetDecimal(IDXProfundidad),
                                Peso = lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion)
                            };

                            yield return registro;
                        }
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

                        while (await lector.ReadAsync())
                        {
                            Formatos registro = new Formatos
                            {
                                ID = lector.GetString(IDXID),
                                Nombre = lector.GetString(IDXNombre),
                                Alto = lector.GetDecimal(IDXAlto),
                                Largo = lector.GetDecimal(IDXLargo),
                                Profundidad = lector.GetDecimal(IDXProfundidad),
                                Peso = lector.GetDecimal(IDXPeso),
                                FechaCreacion = lector.GetDateTime(IDXFechaCreacion),
                                FechaModificacion = lector.GetDateTime(IDXFechaModificacion)
                            };

                            yield return registro;
                        }
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
