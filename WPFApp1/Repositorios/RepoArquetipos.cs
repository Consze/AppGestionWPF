using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;

namespace WPFApp1.Repositorios
{
    public class RepoArquetiposSQLite : IRepoEntidadGenerica<Arquetipos>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoArquetiposSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"CategoriaID", "categoria_id" },
                {"Nombre" , "Nombre" },
                {"ID", "ID" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public async IAsyncEnumerable<Arquetipos> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Arquetipos registro = new Arquetipos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Arquetipos> RecuperarList()
        {
            List<Arquetipos> ArquetiposLista = new List<Arquetipos>();
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.EsEliminado = False;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Arquetipos registro = new Arquetipos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            ArquetiposLista.Add(registro);
                        }

                        return ArquetiposLista;
                    }
                }
            }
        }
        public Arquetipos Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Arquetipos registro = new Arquetipos();

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                        };

                        return registro;
                    }
                }
            }
        }
        public bool Modificar(Arquetipos registroModificado)
        {
            Arquetipos registroActual = Recuperar(registroModificado.ID);
            var propiedadesEntidad = typeof(Arquetipos).GetProperties();
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
                        string Consulta = $"UPDATE Productos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", registroModificado.ID);
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
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos WHERE ID = @id;";
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
        public string Insertar(Arquetipos nuevoRegistro)
        {
            string consulta = @"INSERT INTO Productos
                (   ID,
                    Nombre,
                    categoria_id,
                    FechaCreacion)
                VALUES (
                    @ID,
                    @NombreProducto,
                    @categoria_id,
                    @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@NombreProducto", nuevoRegistro.Nombre);
                    comando.Parameters.AddWithValue("@categoria_id", nuevoRegistro.CategoriaID);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
    }
    public class RepoArquetiposSQLServer : IRepoEntidadGenerica<Arquetipos>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoArquetiposSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"CategoriaID", "categoria_id" },
                {"Nombre" , "Nombre" },
                {"ID", "ID" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public async IAsyncEnumerable<Arquetipos> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.EsEliminado = False;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Arquetipos registro = new Arquetipos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return registro;
                        }
                    }
                }
            }
        }
        public List<Arquetipos> RecuperarList()
        {
            List<Arquetipos> ArquetiposLista = new List<Arquetipos>();
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.EsEliminado = False;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Arquetipos registro = new Arquetipos
                            {
                                ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            ArquetiposLista.Add(registro);
                        }

                        return ArquetiposLista;
                    }
                }
            }
        }
        public Arquetipos Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    p.id AS ProductoID,
                    p.nombre AS Nombre,
                    p.categoria_id AS CategoriaID,
                    p.FechaCreacion AS FechaCreacion,
                    p.FechaModificacion AS FechaModificacion,
                    p.EsEliminado AS EsEliminado
                FROM Productos AS p
                WHERE p.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXID = lector.GetOrdinal("ProductoID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Arquetipos registro = new Arquetipos();

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.CategoriaID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                        };

                        return registro;
                    }
                }
            }
        }
        public bool Modificar(Arquetipos registroModificado)
        {
            Arquetipos registroActual = Recuperar(registroModificado.ID);
            var propiedadesEntidad = typeof(Arquetipos).GetProperties();
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
                        string Consulta = $"UPDATE Productos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", registroModificado.ID);
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
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos WHERE ID = @id;";
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
        public string Insertar(Arquetipos nuevoRegistro)
        {
            string consulta = @"INSERT INTO Productos
                (   ID,
                    Nombre,
                    categoria_id,
                    FechaCreacion)
                VALUES (
                    @ID,
                    @NombreProducto,
                    @categoria_id,
                    @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@NombreProducto", nuevoRegistro.Nombre);
                    comando.Parameters.AddWithValue("@categoria_id", nuevoRegistro.CategoriaID);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
    }
}
