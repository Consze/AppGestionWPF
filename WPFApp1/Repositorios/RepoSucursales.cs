using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;

namespace WPFApp1.Repositorios
{
    public class RepoSucursalesSQLite : IRepoEntidadGenerica<Sucursal>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoSucursalesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"Localidad", "Localidad" },
                {"Calle", "Calle" },
                {"alturaCalle", "Altura" },
                {"Telefono", "Telefono" },
                {"Longitud", "Longitud" },
                {"Latitud", "Latitud" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Sucursal Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Sucursal sucursal = new Sucursal();

                        if (lector.Read())
                        {
                            sucursal.ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID);
                            sucursal.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            sucursal.Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad);
                            sucursal.Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle);
                            sucursal.alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura);
                            sucursal.Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono);
                            sucursal.Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud);
                            sucursal.Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud);
                            sucursal.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            sucursal.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            sucursal.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return sucursal;
                    }
                }
            }
        }
        public string Insertar(Sucursal nuevaSucursal)
        {
            string consulta = @"INSERT INTO Sucursales (
                ID,
                Nombre,
                Localidad,
                Calle,
                Altura,
                Telefono,
                Latitud,
                Longitud,
                FechaCreacion)
            VALUES (
                @SucursalID,
                @NombreSucursal,
                @Localidad,
                @Calle,
                @Altura,
                @Telefono,
                @Latitud,
                @Longitud,
                @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@SucursalID", nuevaSucursal.ID);
                    comando.Parameters.AddWithValue("@NombreSucursal", nuevaSucursal.Nombre);
                    comando.Parameters.AddWithValue("@Localidad", nuevaSucursal.Localidad);
                    comando.Parameters.AddWithValue("@Calle", nuevaSucursal.Calle);
                    comando.Parameters.AddWithValue("@Altura", nuevaSucursal.alturaCalle);
                    comando.Parameters.AddWithValue("@Telefono", nuevaSucursal.Telefono);
                    comando.Parameters.AddWithValue("@Latitud", nuevaSucursal.Latitud);
                    comando.Parameters.AddWithValue("@Longitud", nuevaSucursal.Longitud);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaSucursal.ID;
                }
            }
        }
        public async IAsyncEnumerable<Sucursal> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Sucursal sucursal = new Sucursal
                            {
                                ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad),
                                Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle),
                                alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura),
                                Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono),
                                Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud),
                                Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return sucursal;
                        }
                    }
                }
            }
        }
        public List<Sucursal> RecuperarList()
        {
            List<Sucursal> Sucursales = new List<Sucursal>();
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Sucursal sucursal = new Sucursal
                            {
                                ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad),
                                Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle),
                                alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura),
                                Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono),
                                Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud),
                                Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };
                            Sucursales.Add(sucursal);
                        }

                        return Sucursales;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Sucursales SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Sucursales WHERE ID = @id;";
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
        public bool Modificar(Sucursal marcaModificada)
        {
            Sucursal registroActual = Recuperar(marcaModificada.ID);
            var propiedadesEntidad = typeof(Sucursal).GetProperties();
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
                        string Consulta = $"UPDATE Sucursales SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
    public class RepoSucursalesSQLServer : IRepoEntidadGenerica<Sucursal>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoSucursalesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"Localidad", "Localidad" },
                {"Calle", "Calle" },
                {"alturaCalle", "Altura" },
                {"Telefono", "Telefono" },
                {"Longitud", "Longitud" },
                {"Latitud", "Latitud" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Sucursal Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Sucursal sucursal = new Sucursal();

                        if (lector.Read())
                        {
                            sucursal.ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID);
                            sucursal.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            sucursal.Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad);
                            sucursal.Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle);
                            sucursal.alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura);
                            sucursal.Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono);
                            sucursal.Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud);
                            sucursal.Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud);
                            sucursal.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            sucursal.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            sucursal.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return sucursal;
                    }
                }
            }
        }
        public string Insertar(Sucursal nuevaSucursal)
        {
            string consulta = @"INSERT INTO Sucursales (
                ID,
                Nombre,
                Localidad,
                Calle,
                Altura,
                Telefono,
                Latitud,
                Longitud,
                FechaCreacion)
            VALUES (
                @SucursalID,
                @NombreSucursal,
                @Localidad,
                @Calle,
                @Altura,
                @Telefono,
                @Latitud,
                @Longitud,
                @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@SucursalID", nuevaSucursal.ID);
                    comando.Parameters.AddWithValue("@NombreSucursal", nuevaSucursal.Nombre);
                    comando.Parameters.AddWithValue("@Localidad", nuevaSucursal.Localidad);
                    comando.Parameters.AddWithValue("@Calle", nuevaSucursal.Calle);
                    comando.Parameters.AddWithValue("@Altura", nuevaSucursal.alturaCalle);
                    comando.Parameters.AddWithValue("@Telefono", nuevaSucursal.Telefono);
                    comando.Parameters.AddWithValue("@Latitud", nuevaSucursal.Latitud);
                    comando.Parameters.AddWithValue("@Longitud", nuevaSucursal.Longitud);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaSucursal.ID;
                }
            }
        }
        public async IAsyncEnumerable<Sucursal> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Sucursal sucursal = new Sucursal
                            {
                                ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad),
                                Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle),
                                alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura),
                                Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono),
                                Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud),
                                Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return sucursal;
                        }
                    }
                }
            }
        }
        public List<Sucursal> RecuperarList()
        {
            List<Sucursal> Sucursales = new List<Sucursal>();
            string consulta = @"SELECT 
                    s.id AS SucursalID,
                    s.Nombre AS NombreSucursal,
                    s.Localidad AS Localidad,
                    s.Calle AS Calle,
                    s.Altura AS Altura,
                    s.Telefono AS Telefono,
                    s.Latitud AS Latitud,
                    s.Longitud AS Longitud,
                    s.FechaCreacion AS FechaCreacion,
                    s.FechaModificacion AS FechaModificacion,
                    s.EsEliminado AS EsEliminado
                FROM Sucursales AS s
                WHERE s.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXNombre = lector.GetOrdinal("NombreSucursal");
                        int IDXLocalidad = lector.GetOrdinal("Localidad");
                        int IDXCalle = lector.GetOrdinal("Calle");
                        int IDXAltura = lector.GetOrdinal("Altura");
                        int IDXTelefono = lector.GetOrdinal("Telefono");
                        int IDXLatitud = lector.GetOrdinal("Latitud");
                        int IDXLongitud = lector.GetOrdinal("Longitud");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Sucursal sucursal = new Sucursal
                            {
                                ID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                Localidad = lector.IsDBNull(IDXLocalidad) ? "" : lector.GetString(IDXLocalidad),
                                Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle),
                                alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura),
                                Telefono = lector.IsDBNull(IDXTelefono) ? "" : lector.GetString(IDXTelefono),
                                Latitud = lector.IsDBNull(IDXLatitud) ? 0 : lector.GetDecimal(IDXLatitud),
                                Longitud = lector.IsDBNull(IDXLongitud) ? 0 : lector.GetDecimal(IDXLongitud),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };
                            Sucursales.Add(sucursal);
                        }

                        return Sucursales;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Sucursales SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Sucursales WHERE ID = @id;";
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
        public bool Modificar(Sucursal marcaModificada)
        {
            Sucursal registroActual = Recuperar(marcaModificada.ID);
            var propiedadesEntidad = typeof(Sucursal).GetProperties();
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
                        string Consulta = $"UPDATE Sucursales SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
