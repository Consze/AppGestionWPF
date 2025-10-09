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
                            sucursal.Localidad = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXLocalidad);
                            sucursal.Calle = lector.IsDBNull(IDXCalle) ? "" : lector.GetString(IDXCalle);
                            sucursal.alturaCalle = lector.IsDBNull(IDXAltura) ? 0 : lector.GetInt32(IDXAltura);
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
                using (SqliteCommand comandoBusqueda = new SqliteCommand(consultaBusqueda, conexion))
                {
                    comandoBusqueda.Parameters.AddWithValue("@NombreMarca", nuevaMarca.Nombre);
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
}
