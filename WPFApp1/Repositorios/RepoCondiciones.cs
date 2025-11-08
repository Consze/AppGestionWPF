using System.Data.SqlClient;
using WPFApp1.Enums;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using System.Data.SQLite;

namespace WPFApp1.Repositorios
{
    public class RepoCondicionesSQLite : IRepoEntidadGenerica<Condiciones>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCondicionesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Condiciones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Condiciones Condicion = new Condiciones();

                        if (lector.Read())
                        {
                            Condicion.ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID);
                            Condicion.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Condicion.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Condicion.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Condicion.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Condicion;
                    }
                }
            }
        }
        public string Insertar(Condiciones nuevaCondicion)
        {
            string consulta = @"INSERT INTO Productos_condiciones (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @CondicionID,
                @Nombre,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS CondicionID FROM Productos_condiciones
                WHERE Nombre = @NombreCondicion;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreCondicion", nuevaCondicion.Nombre);
                    using (SqliteDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("CondicionID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CondicionID", nuevaCondicion.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaCondicion.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaCondicion.ID;
                }
            }
        }
        public async IAsyncEnumerable<Condiciones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Condiciones Condicion = new Condiciones
                            {
                                ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Condicion;
                        }
                    }
                }
            }
        }
        public List<Condiciones> RecuperarList()
        {
            List<Condiciones> Categorias = new List<Condiciones>();
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Condiciones Condicion = new Condiciones
                            {
                                ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Categorias.Add(Condicion);
                        }

                        return Categorias;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_condiciones SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_condiciones WHERE ID = @id;";
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
        public bool Modificar(Condiciones categoriaModificada)
        {
            Condiciones registroActual = Recuperar(categoriaModificada.ID);
            var propiedadesEntidad = typeof(Condiciones).GetProperties();
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
                            var valorModificado = propiedad.GetValue(categoriaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(categoriaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_condiciones SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", categoriaModificada.ID);
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
    public class RepoCondicionesSQLServer : IRepoEntidadGenerica<Condiciones>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCondicionesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Condiciones Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Condiciones Condicion = new Condiciones();

                        if (lector.Read())
                        {
                            Condicion.ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID);
                            Condicion.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Condicion.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Condicion.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Condicion.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Condicion;
                    }
                }
            }
        }
        public string Insertar(Condiciones nuevaCondicion)
        {
            string consulta = @"INSERT INTO Productos_condiciones (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @CondicionID,
                @Nombre,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS CondicionID FROM Productos_condiciones
                WHERE Nombre = @NombreCondicion;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comandoBusqueda = new SqlCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreCondicion", nuevaCondicion.Nombre);
                    using (SqlDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("CondicionID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CondicionID", nuevaCondicion.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaCondicion.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaCondicion.ID;
                }
            }
        }
        public async IAsyncEnumerable<Condiciones> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Condiciones Condicion = new Condiciones
                            {
                                ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Condicion;
                        }
                    }
                }
            }
        }
        public List<Condiciones> RecuperarList()
        {
            List<Condiciones> Categorias = new List<Condiciones>();
            string consulta = @"SELECT 
                    cd.id AS CondicionID,
                    cd.nombre AS Nombre,
                    cd.FechaCreacion AS FechaCreacion,
                    cd.FechaModificacion AS FechaModificacion,
                    cd.EsEliminado AS EsEliminado
                FROM Productos_condiciones AS cd
                WHERE cd.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCondicionID = lector.GetOrdinal("CondicionID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Condiciones Condicion = new Condiciones
                            {
                                ID = lector.IsDBNull(IDXCondicionID) ? "" : lector.GetString(IDXCondicionID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Categorias.Add(Condicion);
                        }

                        return Categorias;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_condiciones SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_condiciones WHERE ID = @id;";
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
        public bool Modificar(Condiciones categoriaModificada)
        {
            Condiciones registroActual = Recuperar(categoriaModificada.ID);
            var propiedadesEntidad = typeof(Condiciones).GetProperties();
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
                            var valorModificado = propiedad.GetValue(categoriaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(categoriaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Productos_condiciones SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", categoriaModificada.ID);
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
