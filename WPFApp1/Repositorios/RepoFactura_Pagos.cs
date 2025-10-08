using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;
using System.Data.SqlClient;
using WPFApp1.DTOS;
using System.Windows.Media;
using System.Data.SQLite;

namespace WPFApp1.Repositorios
{
    public class RepoFacturaPagosSQLite : IRepoEntidadGenerica<Factura_pagos>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaPagosSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"FacturaID", "FacturaID" },
                {"MedioPagoID", "medio_pago" },
                {"Monto", "monto" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura_pagos Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    { 
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura_pagos FacturaPagos = new Factura_pagos();

                        if (lector.Read())
                        {
                            FacturaPagos.ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx);
                            FacturaPagos.FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            FacturaPagos.MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID);
                            FacturaPagos.Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto);
                            FacturaPagos.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            FacturaPagos.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            FacturaPagos.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        };

                        return FacturaPagos;
                    }
                }
            }
        }
        public string Insertar(Factura_pagos nuevaFactura)
        {
            string consulta = @"INSERT INTO Facturas_pagos (FacturaID, medio_pago, monto, FechaCreacion)
            VALUES (@FacturaID, @MedioPagoID, @monto, @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevaFactura.FacturaID);
                    comando.Parameters.AddWithValue("@MedioPagoID", nuevaFactura.MedioPagoID);
                    comando.Parameters.AddWithValue("@monto",nuevaFactura.Monto);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura_pagos> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Factura_pagos FacturaPagos = new Factura_pagos
                            {
                                ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                            };

                            yield return FacturaPagos;
                        }
                    }
                }
            }
        }
        public List<Factura_pagos> RecuperarList()
        {
            List<Factura_pagos> ListaFacturaPagos = new List<Factura_pagos>();
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura_pagos FacturaPagos = new Factura_pagos
                            {
                                ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                            };
                            ListaFacturaPagos.Add(FacturaPagos);
                        }

                        return ListaFacturaPagos;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas_pagos SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas_pagos WHERE ID = @id;";
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
        public bool Modificar(Factura_pagos facturaModificada)
        {
            Factura_pagos registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura_pagos).GetProperties();
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
                        string Consulta = $"UPDATE Facturas_pagos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
    public class RepoFacturaPagosSQLServer : IRepoEntidadGenerica<Factura_pagos>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaPagosSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"FacturaID", "FacturaID" },
                {"MedioPagoID", "medio_pago" },
                {"Monto", "monto" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura_pagos Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.id = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura_pagos FacturaPagos = new Factura_pagos();

                        if (lector.Read())
                        {
                            FacturaPagos.ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx);
                            FacturaPagos.FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            FacturaPagos.MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID);
                            FacturaPagos.Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto);
                            FacturaPagos.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            FacturaPagos.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            FacturaPagos.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return FacturaPagos;
                    }
                }
            }
        }
        public string Insertar(Factura_pagos nuevaFactura)
        {
            string consulta = @"INSERT INTO Facturas_pagos (FacturaID, medio_pago, monto, FechaCreacion)
            VALUES (@FacturaID, @MedioPagoID, @monto, @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevaFactura.FacturaID);
                    comando.Parameters.AddWithValue("@MedioPagoID", nuevaFactura.MedioPagoID);
                    comando.Parameters.AddWithValue("@monto", nuevaFactura.Monto);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevaFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura_pagos> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Factura_pagos FacturaPagos = new Factura_pagos
                            {
                                ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                            };

                            yield return FacturaPagos;
                        }
                    }
                }
            }
        }
        public List<Factura_pagos> RecuperarList()
        {
            List<Factura_pagos> ListaFacturaPagos = new List<Factura_pagos>();
            string consulta = @"SELECT 
                    fp.id AS RegistroID,
                    fp.FacturaID AS FacturaID,
                    fp.medio_pago AS MedioPagoID,
                    fp.monto AS Monto,
                    fp.FechaCreacion AS FechaCreacion,
                    fp.FechaModificacion AS FechaModificacion,
                    fp.EsEliminado AS EsEliminado
                FROM Facturas_pagos AS fp
                WHERE fp.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXidx = lector.GetOrdinal("RegistroID");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXMonto = lector.GetOrdinal("Monto");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura_pagos FacturaPagos = new Factura_pagos
                            {
                                ID = lector.IsDBNull(IDXidx) ? "" : lector.GetString(IDXidx),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                Monto = lector.IsDBNull(IDXMonto) ? 0 : lector.GetDecimal(IDXMonto),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                            };
                            ListaFacturaPagos.Add(FacturaPagos);
                        }

                        return ListaFacturaPagos;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas_pagos SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas_pagos WHERE ID = @id;";
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
        public bool Modificar(Factura_pagos facturaModificada)
        {
            Factura_pagos registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura_pagos).GetProperties();
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
                        string Consulta = $"UPDATE Facturas_pagos SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
