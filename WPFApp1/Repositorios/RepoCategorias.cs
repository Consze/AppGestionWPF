using System.Data.SqlClient;
using WPFApp1.Enums;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class RepoCategoriasSQLite : IRepoEntidadGenerica<Categorias>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCategoriasSQLite(ConexionDBSQLite _accesoDB)
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
        public Categorias Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Categorias Categoria = new Categorias();

                        if (lector.Read())
                        {
                            Categoria.ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);
                            Categoria.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Categoria.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Categoria.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Categoria.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        };

                        return Categoria;
                    }
                }
            }
        }
        public string Insertar(Categorias nuevaCategoria)
        {
            string consulta = @"INSERT INTO Productos_categorias (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @CategoriaID,
                @Nombre,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS CategoriaID FROM Productos_categorias
                WHERE Nombre = @NombreCategoria;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreCategoria", nuevaCategoria.Nombre);
                    using (SqliteDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("CategoriaID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CategoriaID", nuevaCategoria.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaCategoria.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaCategoria.ID;
                }
            }
        }
        public async IAsyncEnumerable<Categorias> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Categorias Categoria = new Categorias
                            {
                                ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Categoria;
                        }
                    }
                }
            }
        }
        public List<Categorias> RecuperarList()
        {
            List<Categorias> Categorias = new List<Categorias>();
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Categorias Categoria = new Categorias
                            {
                                ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Categorias.Add(Categoria);
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
                consulta = "UPDATE Productos_categorias SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_categorias WHERE ID = @id;";
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
        public bool Modificar(Categorias categoriaModificada)
        {
            Categorias registroActual = Recuperar(categoriaModificada.ID);
            var propiedadesEntidad = typeof(Categorias).GetProperties();
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
                        string Consulta = $"UPDATE Productos_categorias SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
    public class RepoCategoriasSQLServer : IRepoEntidadGenerica<Categorias>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCategoriasSQLServer(ConexionDBSQLServer _accesoDB)
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
        public Categorias Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Categorias Categoria = new Categorias();

                        if (lector.Read())
                        {
                            Categoria.ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID);
                            Categoria.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            Categoria.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Categoria.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Categoria.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Categoria;
                    }
                }
            }
        }
        public string Insertar(Categorias nuevaCategoria)
        {
            string consulta = @"INSERT INTO Productos_categorias (
                ID,
                Nombre,
                FechaCreacion)
            VALUES (
                @CategoriaID,
                @Nombre,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS CategoriaID FROM Productos_categorias
                WHERE Nombre = @NombreCategoria;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comandoBusqueda = new SqlCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreCategoria", nuevaCategoria.Nombre);
                    using (SqlDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("CategoriaID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CategoriaID", nuevaCategoria.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaCategoria.Nombre);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaCategoria.ID;
                }
            }
        }
        public async IAsyncEnumerable<Categorias> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Categorias Categoria = new Categorias
                            {
                                ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Categoria;
                        }
                    }
                }
            }
        }
        public List<Categorias> RecuperarList()
        {
            List<Categorias> Categorias = new List<Categorias>();
            string consulta = @"SELECT 
                    c.id AS CategoriaID,
                    c.nombre AS Nombre,
                    c.FechaCreacion AS FechaCreacion,
                    c.FechaModificacion AS FechaModificacion,
                    c.EsEliminado AS EsEliminado
                FROM Productos_categorias AS c
                WHERE c.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCategoriaID = lector.GetOrdinal("CategoriaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Categorias Categoria = new Categorias
                            {
                                ID = lector.IsDBNull(IDXCategoriaID) ? "" : lector.GetString(IDXCategoriaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Categorias.Add(Categoria);
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
                consulta = "UPDATE Productos_categorias SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_categorias WHERE ID = @id;";
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
        public bool Modificar(Categorias categoriaModificada)
        {
            Categorias registroActual = Recuperar(categoriaModificada.ID);
            var propiedadesEntidad = typeof(Categorias).GetProperties();
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
                        string Consulta = $"UPDATE Productos_categorias SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
