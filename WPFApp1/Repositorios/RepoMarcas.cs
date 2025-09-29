using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class RepoMarcasSQLite : IRepoEntidadGenerica<Marcas>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoMarcasSQLite(ConexionDBSQLite _accesoDB)
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
        public Marcas Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Marcas Marca = new Marcas();

                        if(lector.Read())
                        {
                            Marca.ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            Marca.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Marca.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Marca.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Marca.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        };

                        return Marca;
                    }
                }
            }
        }
        /// <summary>
        /// Si no existe crea un nuevo registro para la entidad 'Marcas'
        /// </summary>
        /// <param name="nuevaMarca"></param>
        /// <returns>ID de registro nuevo o Pre-existente</returns>
        public string Insertar(Marcas nuevaMarca)
        {
            string consulta = @"INSERT INTO Marcas (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @MarcaID,
                @NombreMarca,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS MarcaID FROM Marcas
                WHERE Nombre = @NombreMarca";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda,conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreMarca",nuevaMarca.ID);
                    using (SqliteDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("MarcaID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@MarcaID", nuevaMarca.ID);
                    comando.Parameters.AddWithValue("@NombreMarca", nuevaMarca.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaMarca.ID;
                }
            }
        }
        public async IAsyncEnumerable<Marcas> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Marcas Marca = new Marcas
                            {
                                ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
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
        public List<Marcas> RecuperarList()
        {
            List<Marcas> Marcas = new List<Marcas>();
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Marcas Marca = new Marcas
                            {
                                ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Marcas.Add(Marca);
                        }

                        return Marcas;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Marcas SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Marcas WHERE ID = @id;";
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
        public bool Modificar(Marcas marcaModificada)
        {
            Marcas registroActual = Recuperar(marcaModificada.ID);
            var propiedadesEntidad = typeof(Marcas).GetProperties();
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
                            var valorModificado = propiedad.GetValue(marcaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(marcaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Marcas SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", marcaModificada.ID);
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
    public class RepoMarcasSQLServer : IRepoEntidadGenerica<Marcas>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoMarcasSQLServer(ConexionDBSQLServer _accesoDB)
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
        public Marcas Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Marcas Marca = new Marcas();

                        if (lector.Read())
                        {
                            Marca.ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID);
                            Marca.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Marca.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Marca.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Marca.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Marca;
                    }
                }
            }
        }
        public string Insertar(Marcas nuevaMarca)
        {
            string consulta = @"INSERT INTO Marcas (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @MarcaID,
                @NombreMarca,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS MarcaID FROM Marcas
                WHERE Nombre = @NombreMarca";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comandoBusqueda = new SqlCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreMarca", nuevaMarca.ID);
                    using (SqlDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("MarcaID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@MarcaID", nuevaMarca.ID);
                    comando.Parameters.AddWithValue("@NombreMarca", nuevaMarca.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaMarca.ID;
                }
            }
        }
        public async IAsyncEnumerable<Marcas> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Marcas Marca = new Marcas
                            {
                                ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
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
        public List<Marcas> RecuperarList()
        {
            List<Marcas> Marcas = new List<Marcas>();
            string consulta = @"SELECT 
                    m.id AS MarcaID,
                    m.nombre AS MarcaNombre,
                    m.FechaCreacion AS FechaCreacion,
                    m.FechaModificacion AS FechaModificacion,
                    m.EsEliminado AS EsEliminado
                FROM Marcas AS m
                WHERE m.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXMarcaID = lector.GetOrdinal("MarcaID");
                        int IDXNombre = lector.GetOrdinal("MarcaNombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Marcas Marca = new Marcas
                            {
                                ID = lector.IsDBNull(IDXMarcaID) ? "" : lector.GetString(IDXMarcaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Marcas.Add(Marca);
                        }

                        return Marcas;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Marcas SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Marcas WHERE ID = @id;";
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
        public bool Modificar(Marcas marcaModificada)
        {
            Marcas registroActual = Recuperar(marcaModificada.ID);
            var propiedadesEntidad = typeof(Marcas).GetProperties();
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
                            var valorModificado = propiedad.GetValue(marcaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(marcaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Marcas SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", marcaModificada.ID);
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
