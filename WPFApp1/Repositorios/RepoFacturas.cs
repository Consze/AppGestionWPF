using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;
using System.Data.SqlClient;
using WPFApp1.DTOS;

namespace WPFApp1.Repositorios
{
    public class RepoFacturaSQLite : IRepoEntidadGenerica<Factura>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"SucursalID", "sucursalID" },
                {"FechaVenta", "FechaVenta" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura Factura = new Factura();

                        if (lector.Read())
                        {
                            Factura.ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            Factura.SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID);
                            Factura.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Factura.FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta);
                            Factura.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Factura.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Factura;
                    }
                }
            }
        }
        public string Insertar(Factura nuevaFactura)
        {
            string consulta = @"INSERT INTO Facturas (
                ID,
                sucursalID,
                FechaVenta,
                FechaCreacion)
            VALUES (
                @FacturaID,
                @SucursalID,
                @FechaVenta,
                @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevaFactura.ID);
                    comando.Parameters.AddWithValue("@SucursalID", nuevaFactura.SucursalID);
                    comando.Parameters.AddWithValue("@FechaVenta", nuevaFactura.FechaVenta);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Factura Factura = new Factura
                            {
                                ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Factura;
                        }
                    }
                }
            }
        }
        public List<Factura> RecuperarList()
        {
            List<Factura> Facturas = new List<Factura>();
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.EsEliminado = FALSE;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura Factura = new Factura
                            {
                                ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Facturas.Add(Factura);
                        }

                        return Facturas;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas WHERE ID = @id;";
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
        public bool Modificar(Factura facturaModificada)
        {
            Factura registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura).GetProperties();
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
                            var valorModificado = propiedad.GetValue(facturaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(facturaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Facturas SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", facturaModificada.ID);
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
    public class RepoFacturaSQLServer : IRepoEntidadGenerica<Factura>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"SucursalID", "sucursalID" },
                {"FechaVenta", "FechaVenta" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura Factura = new Factura();

                        if (lector.Read())
                        {
                            Factura.ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            Factura.SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID);
                            Factura.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Factura.FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta);
                            Factura.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Factura.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Factura;
                    }
                }
            }
        }
        public string Insertar(Factura nuevaFactura)
        {
            string consulta = @"INSERT INTO Facturas (
                ID,
                sucursalID,
                FechaVenta,
                FechaCreacion)
            VALUES (
                @FacturaID,
                @SucursalID,
                @FechaVenta,
                @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevaFactura.ID);
                    comando.Parameters.AddWithValue("@SucursalID", nuevaFactura.SucursalID);
                    comando.Parameters.AddWithValue("@FechaVenta", nuevaFactura.FechaVenta);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Factura Factura = new Factura
                            {
                                ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return Factura;
                        }
                    }
                }
            }
        }
        public List<Factura> RecuperarList()
        {
            List<Factura> Facturas = new List<Factura>();
            string consulta = @"SELECT 
                    f.id AS FacturaID,
                    f.sucursalID AS SucursalID,
                    f.FechaVenta AS FechaVenta,
                    f.FechaCreacion AS FechaCreacion,
                    f.FechaModificacion AS FechaModificacion,
                    f.EsEliminado AS EsEliminado
                FROM Facturas AS f
                WHERE f.EsEliminado = FALSE;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXSucursalID = lector.GetOrdinal("SucursalID");
                        int IDXFechaVenta = lector.GetOrdinal("FechaVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura Factura = new Factura
                            {
                                ID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                SucursalID = lector.IsDBNull(IDXSucursalID) ? "" : lector.GetString(IDXSucursalID),
                                FechaVenta = lector.IsDBNull(IDXFechaVenta) ? DateTime.MinValue : lector.GetDateTime(IDXFechaVenta),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            Facturas.Add(Factura);
                        }

                        return Facturas;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas WHERE ID = @id;";
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
        public bool Modificar(Factura facturaModificada)
        {
            Factura registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura).GetProperties();
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
                            var valorModificado = propiedad.GetValue(facturaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(facturaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Facturas SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", facturaModificada.ID);
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
