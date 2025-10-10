using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;

namespace WPFApp1.Repositorios
{
    public class RepoCuentasContablesSQLite : IRepoEntidadGenerica<Cuentas_Contables>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCuentasContablesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"TipoCuenta", "tipo_cuenta" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Cuentas_Contables Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Cuentas_Contables cuentaContable = new Cuentas_Contables();

                        if (lector.Read())
                        {
                            cuentaContable.ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID);
                            cuentaContable.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            cuentaContable.TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta);
                            cuentaContable.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            cuentaContable.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            cuentaContable.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return cuentaContable;
                    }
                }
            }
        }
        public string Insertar(Cuentas_Contables nuevaSucursal)
        {
            string consulta = @"INSERT INTO Sucursales (
                ID,
                Nombre,
                tipo_cuenta,
                FechaCreacion)
            VALUES (
                @CuentaID,
                @Nombre,
                @tipoCuenta,
                @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CuentaID", nuevaSucursal.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaSucursal.Nombre);
                    comando.Parameters.AddWithValue("@tipoCuenta", nuevaSucursal.TipoCuenta);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaSucursal.ID;
                }
            }
        }
        public async IAsyncEnumerable<Cuentas_Contables> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Cuentas_Contables cuentaContable = new Cuentas_Contables
                            {
                                ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return cuentaContable;
                        }
                    }
                }
            }
        }
        public List<Cuentas_Contables> RecuperarList()
        {
            List<Cuentas_Contables> ListaCuentasContables = new List<Cuentas_Contables>();
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Cuentas_Contables cuentaContable = new Cuentas_Contables
                            {
                                ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };
                            ListaCuentasContables.Add(cuentaContable);
                        }

                        return ListaCuentasContables;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Cuentas_contables SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Cuentas_contables WHERE ID = @id;";
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
        public bool Modificar(Cuentas_Contables cuentaContableModificada)
        {
            Cuentas_Contables registroActual = Recuperar(cuentaContableModificada.ID);
            var propiedadesEntidad = typeof(Cuentas_Contables).GetProperties();
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
                            var valorModificado = propiedad.GetValue(cuentaContableModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(cuentaContableModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Cuentas_contables SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", cuentaContableModificada.ID);
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
    public class RepoCuentasContablesSQLServer : IRepoEntidadGenerica<Cuentas_Contables>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoCuentasContablesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"Nombre", "Nombre" },
                {"TipoCuenta", "tipo_cuenta" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Cuentas_Contables Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Cuentas_Contables cuentaContable = new Cuentas_Contables();

                        if (lector.Read())
                        {
                            cuentaContable.ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID);
                            cuentaContable.Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre);
                            cuentaContable.TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta);
                            cuentaContable.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            cuentaContable.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            cuentaContable.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return cuentaContable;
                    }
                }
            }
        }
        public string Insertar(Cuentas_Contables nuevaSucursal)
        {
            string consulta = @"INSERT INTO Sucursales (
                ID,
                Nombre,
                tipo_cuenta,
                FechaCreacion)
            VALUES (
                @CuentaID,
                @Nombre,
                @tipoCuenta,
                @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@CuentaID", nuevaSucursal.ID);
                    comando.Parameters.AddWithValue("@Nombre", nuevaSucursal.Nombre);
                    comando.Parameters.AddWithValue("@tipoCuenta", nuevaSucursal.TipoCuenta);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaSucursal.ID;
                }
            }
        }
        public async IAsyncEnumerable<Cuentas_Contables> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Cuentas_Contables cuentaContable = new Cuentas_Contables
                            {
                                ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return cuentaContable;
                        }
                    }
                }
            }
        }
        public List<Cuentas_Contables> RecuperarList()
        {
            List<Cuentas_Contables> ListaCuentasContables = new List<Cuentas_Contables>();
            string consulta = @"SELECT 
                    cc.id AS CuentaID,
                    cc.Nombre AS Nombre,
                    cc.tipo_cuenta AS TipoCuenta,
                    cc.FechaCreacion AS FechaCreacion,
                    cc.FechaModificacion AS FechaModificacion,
                    cc.EsEliminado AS EsEliminado
                FROM Cuentas_contables AS cc
                WHERE cc.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXCuentaID = lector.GetOrdinal("CuentaID");
                        int IDXNombre = lector.GetOrdinal("Nombre");
                        int IDXTipoCuenta = lector.GetOrdinal("TipoCuenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Cuentas_Contables cuentaContable = new Cuentas_Contables
                            {
                                ID = lector.IsDBNull(IDXCuentaID) ? "" : lector.GetString(IDXCuentaID),
                                Nombre = lector.IsDBNull(IDXNombre) ? "" : lector.GetString(IDXNombre),
                                TipoCuenta = lector.IsDBNull(IDXTipoCuenta) ? "" : lector.GetString(IDXTipoCuenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };
                            ListaCuentasContables.Add(cuentaContable);
                        }

                        return ListaCuentasContables;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Cuentas_contables SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Cuentas_contables WHERE ID = @id;";
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
        public bool Modificar(Cuentas_Contables cuentaContableModificada)
        {
            Cuentas_Contables registroActual = Recuperar(cuentaContableModificada.ID);
            var propiedadesEntidad = typeof(Cuentas_Contables).GetProperties();
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
                            var valorModificado = propiedad.GetValue(cuentaContableModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(cuentaContableModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Cuentas_contables SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", cuentaContableModificada.ID);
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
