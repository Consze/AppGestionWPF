using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;

namespace WPFApp1.Repositorios
{
    public class RepoFormatosSQLite : IRepoEntidadGenerica<Formatos>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFormatosSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "descripcion" },
                {"Alto", "alto" },
                {"Largo", "largo" },
                {"Profundidad" , "profundidad" },
                {"Peso" , "peso" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
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
                WHERE f.EsEliminado = FALSE;";

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
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
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
                WHERE f.EsEliminado = FALSE;";

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
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
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
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            
            if(Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_formatos SET EsEliminado = True WHERE id = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_formatos WHERE id = @id;";
            }

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using(SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    comando.Parameters.AddWithValue("@id",ID);
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }
        public Formatos Recuperar(string ID)
        {
            Formatos registro = new Formatos();
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
                FROM Productos_formatos AS f;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
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

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto);
                            registro.Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo);
                            registro.Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad);
                            registro.Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                        }

                        return registro;
                    }
                }
            }
        }
        public bool Modificar(Formatos registroModificado)
        {
            Formatos registroActual = Recuperar(registroModificado.ID);

            //Campos a ignorar
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ID",
                "FechaCreacion",
                "FechaModificacion",
                "EsEliminado"
            };
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaPropiedadesModificadas = new List<string>();

            try
            {
                using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
                {
                    conexion.Open();
                    using (SqliteCommand comando = new SqliteCommand())
                    {
                        comando.Connection = conexion;

                        //bucle de construccion de consulta
                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            //Comprobar diferencia
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
                        string Consulta = $"UPDATE Productos_formatos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @ID;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@ID", registroModificado.ID);
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
        public string Insertar(Formatos nuevoRegistro)
        {
            string consulta = @"INSERT INTO Productos_formatos 
            (   ID,
                descripcion,
                alto,
                largo,
                profundidad,
                peso,
                FechaCreacion)
            VALUES (
                @ID,
                @descripcion,
                @alto,
                @largo,
                @profundidad,
                @peso,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS ID FROM Productos_formatos 
                WHERE Alto = @alto
                    AND Peso = @peso
                    AND Profundidad = @profundidad
                    AND Largo = @largo
                    AND Descripcion = @formatoNombre;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@peso", nuevoRegistro.Peso);
                    comandoBusqueda.Parameters.AddWithValue("@profundidad", nuevoRegistro.Profundidad);
                    comandoBusqueda.Parameters.AddWithValue("@largo", nuevoRegistro.Largo);
                    comandoBusqueda.Parameters.AddWithValue("@alto", nuevoRegistro.Alto);
                    comandoBusqueda.Parameters.AddWithValue("@formatoNombre", nuevoRegistro.Nombre);

                    using (SqliteDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("ID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@descripcion", nuevoRegistro.Nombre);
                    comando.Parameters.AddWithValue("@alto", nuevoRegistro.Alto);
                    comando.Parameters.AddWithValue("@profundidad", nuevoRegistro.Profundidad);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.Parameters.AddWithValue("@peso", nuevoRegistro.Peso);
                    comando.Parameters.AddWithValue("@largo", nuevoRegistro.Largo);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
        
    }

    public class RepoFormatosSQLServer : IRepoEntidadGenerica<Formatos>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFormatosSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "descripcion" },
                {"Alto", "alto" },
                {"Largo", "largo" },
                {"Profundidad" , "profundidad" },
                {"Peso" , "peso" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
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
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
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
                                Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo),
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
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";

            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Productos_formatos SET EsEliminado = True WHERE id = @id;";
            }
            else
            {
                consulta = "DELETE FROM Productos_formatos WHERE id = @id;";
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
        public Formatos Recuperar(string ID)
        {
            Formatos registro = new Formatos();
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
                FROM Productos_formatos AS f;";

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

                        if (lector.Read())
                        {
                            registro.ID = lector.IsDBNull(IDXID) ? "" : lector.GetString(IDXID);
                            registro.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            registro.Alto = lector.IsDBNull(IDXAlto) ? 0 : lector.GetDecimal(IDXAlto);
                            registro.Largo = lector.IsDBNull(IDXLargo) ? 0 : lector.GetDecimal(IDXLargo);
                            registro.Profundidad = lector.IsDBNull(IDXProfundidad) ? 0 : lector.GetDecimal(IDXProfundidad);
                            registro.Peso = lector.IsDBNull(IDXPeso) ? 0 : lector.GetDecimal(IDXPeso);
                            registro.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                            registro.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            registro.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                        }

                        return registro;
                    }
                }
            }
        }
        public bool Modificar(Formatos registroModificado)
        {
            Formatos registroActual = Recuperar(registroModificado.ID);

            //Campos a ignorar
            var listaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ID",
                "FechaCreacion",
                "FechaModificacion",
                "EsEliminado"
            };
            var propiedadesEntidad = typeof(ProductoCatalogo).GetProperties();
            var listaPropiedadesModificadas = new List<string>();

            try
            {
                using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand())
                    {
                        comando.Connection = conexion;

                        //bucle de construccion de consulta
                        foreach (var propiedad in propiedadesEntidad)
                        {
                            if (listaExclusion.Contains(propiedad.Name))
                                continue;

                            //Comprobar diferencia
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
                        string Consulta = $"UPDATE Productos_formatos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @ID;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@ID", registroModificado.ID);
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
        public string Insertar(Formatos nuevoRegistro)
        {
            string consulta = @"INSERT INTO Productos_formatos 
            (   ID,
                descripcion,
                alto,
                largo,
                profundidad,
                peso,
                FechaCreacion)
            VALUES (
                @ID,
                @descripcion,
                @alto,
                @largo,
                @profundidad,
                @peso,
                @FechaCreacion);";
            string consultaBusqueda = @"SELECT ID AS ID FROM Productos_formatos 
                WHERE Alto = @alto
                    AND Peso = @peso
                    AND Profundidad = @profundidad
                    AND Largo = @largo
                    AND Nombre = @formatoNombre;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comandoBusqueda = new SqlCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@peso", nuevoRegistro.Peso);
                    comandoBusqueda.Parameters.AddWithValue("@profundidad", nuevoRegistro.Profundidad);
                    comandoBusqueda.Parameters.AddWithValue("@largo", nuevoRegistro.Largo);
                    comandoBusqueda.Parameters.AddWithValue("@alto", nuevoRegistro.Alto);
                    comandoBusqueda.Parameters.AddWithValue("@formatoNombre", nuevoRegistro.Nombre);

                    using (SqlDataReader lector = comandoBusqueda.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            int IDXid = lector.GetOrdinal("ID");
                            return lector.GetString(IDXid);
                        }
                    }
                }

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@ID", nuevoRegistro.ID);
                    comando.Parameters.AddWithValue("@descripcion", nuevoRegistro.Nombre);
                    comando.Parameters.AddWithValue("@alto", nuevoRegistro.Alto);
                    comando.Parameters.AddWithValue("@profundidad", nuevoRegistro.Profundidad);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.Parameters.AddWithValue("@peso", nuevoRegistro.Peso);
                    comando.Parameters.AddWithValue("@largo", nuevoRegistro.Largo);
                    comando.ExecuteNonQuery();
                    return nuevoRegistro.ID;
                }
            }
        }
    }
}
