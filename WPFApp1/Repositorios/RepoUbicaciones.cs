using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class RepoUbicacionesSQLite : IRepoEntidadGenerica<Ubicaciones>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoUbicacionesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Descripcion" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Ubicaciones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Ubicaciones Ubicacion = new Ubicaciones();

                        if (lector.Read())
                        {
                            Ubicacion.ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID);
                            Ubicacion.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Ubicacion.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Ubicacion.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Ubicacion.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Ubicacion;
                    }
                }
            }
        }
        public string Insertar(Ubicaciones nuevaUbicacion)
        {
            string consulta = @"INSERT INTO Ubicaciones_inventario (
                ID,
                Descripcion,
                FechaCreacion)
            VALUES (
                @UbicacionID,
                @Descripcion,
                @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@UbicacionID", nuevaUbicacion.ID);
                    comando.Parameters.AddWithValue("@Descripcion", nuevaUbicacion.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaUbicacion.ID;
                }
            }
        }
        public async IAsyncEnumerable<Ubicaciones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Ubicaciones Ubicacion = new Ubicaciones
                            {
                                ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Ubicacion;
                        }
                    }
                }
            }
        }
        public List<Ubicaciones> RecuperarList()
        {
            List<Ubicaciones> Ubicaciones = new List<Ubicaciones>();
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Ubicaciones Ubicacion = new Ubicaciones
                            {
                                ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Ubicaciones.Add(Ubicacion);
                        }

                        return Ubicaciones;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Ubicaciones_inventario SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Ubicaciones_inventario WHERE ID = @id;";
            }

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
        public bool Modificar(Ubicaciones ubicacionModificada)
        {
            Ubicaciones registroActual = Recuperar(ubicacionModificada.ID);
            var propiedadesEntidad = typeof(Ubicaciones).GetProperties();
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
                    using (SqliteCommand comando = new SqliteCommand())
                    {
                        comando.Connection = accesoDB.ObtenerConexionDB();

                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            var valorActual = propiedad.GetValue(registroActual);
                            var valorModificado = propiedad.GetValue(ubicacionModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(ubicacionModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Ubicaciones_inventario SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", ubicacionModificada.ID);
                        comando.CommandText = Consulta;

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (SqliteException ex)
            {
                throw;
            }
        }
    }
    public class RepoUbicacionesSQLServer : IRepoEntidadGenerica<Ubicaciones>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoUbicacionesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Descripcion" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Ubicaciones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Ubicaciones Ubicacion = new Ubicaciones();

                        if (lector.Read())
                        {
                            Ubicacion.ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID);
                            Ubicacion.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Ubicacion.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Ubicacion.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Ubicacion.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Ubicacion;
                    }
                }
            }
        }
        public string Insertar(Ubicaciones nuevaUbicacion)
        {
            string consulta = @"INSERT INTO Ubicaciones_inventario (
                ID,
                Descripcion,
                FechaCreacion)
            VALUES (
                @UbicacionID,
                @Descripcion,
                @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@UbicacionID", nuevaUbicacion.ID);
                    comando.Parameters.AddWithValue("@Descripcion", nuevaUbicacion.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaUbicacion.ID;
                }
            }
        }
        public async IAsyncEnumerable<Ubicaciones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Ubicaciones Marca = new Ubicaciones
                            {
                                ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Marca;
                        }
                    }
                }
            }
        }
        public List<Ubicaciones> RecuperarList()
        {
            List<Ubicaciones> Ubicaciones = new List<Ubicaciones>();
            string consulta = @"SELECT 
                    u.id AS UbicacionID,
                    u.descripcion AS UbicacionNombre,
                    u.FechaCreacion AS FechaCreacion,
                    u.FechaModificacion AS FechaModificacion,
                    u.EsEliminado AS EsEliminado
                FROM Ubicaciones_inventario AS u
                WHERE u.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXUbicacionID = lector.GetOrdinal("UbicacionID");
                        int IDXNombre = lector.GetOrdinal("UbicacionNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Ubicaciones Ubicacion = new Ubicaciones
                            {
                                ID = lector.IsDBNull(IDXUbicacionID) ? "" : lector.GetString(IDXUbicacionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Ubicaciones.Add(Ubicacion);
                        }

                        return Ubicaciones;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Ubicaciones_inventario SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Ubicaciones_inventario WHERE ID = @id;";
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
        public bool Modificar(Ubicaciones ubicacionModificada)
        {
            Ubicaciones registroActual = Recuperar(ubicacionModificada.ID);
            var propiedadesEntidad = typeof(Ubicaciones).GetProperties();
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
                            var valorModificado = propiedad.GetValue(ubicacionModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(ubicacionModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Ubicaciones_inventario SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", ubicacionModificada.ID);
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
    }
}
