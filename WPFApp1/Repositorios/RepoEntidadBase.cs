using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;

namespace WPFApp1.Repositorios
{
    public class RepoEntidadBaseSQLite
    {
        private readonly ConexionDBSQLite accesoDB;
        public RepoEntidadBaseSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
        }
        public async IAsyncEnumerable<Formatos> RecuperarFormatos()
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
                    f.FechaCreacion AS FechaCreacion
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
                                Profundidad = lector.GetDecimal(IDXPeso),
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
    }
}
