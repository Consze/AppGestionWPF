using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;
using System.Data.SqlClient;

namespace WPFApp1.Repositorios
{
    public class RepoVentasSQLite : IRepoEntidadGenerica<Ventas>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoVentasSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"ProductoSKU", "producto_vendido" },
                {"MedioPagoID", "medio_pago"},
                {"precioVenta", "precio_venta"},
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Ventas Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.Cantidad AS Cantidad,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.id = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Ventas Venta = new Ventas();

                        if (lector.Read())
                        {
                            Venta.ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID);
                            Venta.MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID);
                            Venta.ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU);
                            Venta.precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta);
                            Venta.Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad);
                            Venta.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Venta.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Venta.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        };

                        return Venta;
                    }
                }
            }
        }
        public string Insertar(Ventas nuevaVenta)
        {
            string consulta = @"INSERT INTO Ventas_stock (
                ID, producto_vendido, medio_pago, precio_venta, cantidad, FechaCreacion) VALUES (
                @id, @productoSKU, @medioPagoID, @precioVenta, @cantidad, @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using(SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    comando.Parameters.AddWithValue("@id",nuevaVenta.ID);
                    comando.Parameters.AddWithValue("@productoSKU", nuevaVenta.ProductoSKU);
                    comando.Parameters.AddWithValue("@medioPagoID", nuevaVenta.MedioPagoID);
                    comando.Parameters.AddWithValue("@precioVenta", nuevaVenta.precioVenta);
                    comando.Parameters.AddWithValue("@cantidad", nuevaVenta.Cantidad);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();

                    return nuevaVenta.ID;
                }
            }
        }
        public async IAsyncEnumerable<Ventas> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.EsEliminado = FALSE";
            using(SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using(SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while(await lector.ReadAsync())
                        {
                            Ventas Venta = new Ventas
                            {
                                ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return Venta;
                        }
                    }
                }
            }
        }
        public List<Ventas> RecuperarList()
        {
            List<Ventas> ventas = new List<Ventas>();
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.EsEliminado = FALSE";

            using(SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using(SqliteCommand comando = new SqliteCommand(consulta,conexion))
                {
                    using(SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Ventas Venta = new Ventas
                            {
                                ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            ventas.Add(Venta);
                        }

                        return ventas;
                    }
                }
            }
        }
        public bool Modificar(Ventas ventaModificada)
        {
            Ventas registroActual = Recuperar(ventaModificada.ID);
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
                            var valorModificado = propiedad.GetValue(ventaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(ventaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Ventas_stock SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", ventaModificada.ID);
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
                consulta = "UPDATE Ventas_stock SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Ventas_stock WHERE ID = @id;";
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
    }
    public class RepoVentasSQLServer : IRepoEntidadGenerica<Ventas>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoVentasSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"ProductoSKU", "producto_vendido" },
                {"MedioPagoID", "medio_pago"},
                {"precioVenta", "precio_venta"},
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Ventas Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.id = @ID;";
            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Ventas Venta = new Ventas();

                        if (lector.Read())
                        {
                            Venta.ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID);
                            Venta.MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID);
                            Venta.ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU);
                            Venta.precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta);
                            Venta.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Venta.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Venta.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        };

                        return Venta;
                    }
                }
            }
        }
        public string Insertar(Ventas nuevaVenta)
        {
            string consulta = @"INSERT INTO Ventas_stock (
                ID, producto_vendido, medio_pago, precio_venta, FechaCreacion) VALUES (
                @id, @productoSKU, @medioPagoID, @precioVenta, @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", nuevaVenta.ID);
                    comando.Parameters.AddWithValue("@productoSKU", nuevaVenta.ProductoSKU);
                    comando.Parameters.AddWithValue("@medioPagoID", nuevaVenta.MedioPagoID);
                    comando.Parameters.AddWithValue("@precioVenta", nuevaVenta.precioVenta);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();

                    return nuevaVenta.ID;
                }
            }
        }
        public async IAsyncEnumerable<Ventas> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.EsEliminado = FALSE";
            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Ventas Venta = new Ventas
                            {
                                ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            yield return Venta;
                        }
                    }
                }
            }
        }
        public List<Ventas> RecuperarList()
        {
            List<Ventas> ventas = new List<Ventas>();
            string consulta = @"SELECT 
                    vt.id AS VentaID,
                    vt.producto_vendido AS ProductoSKU,
                    vt.medio_pago AS MedioPagoID,
                    vt.precio_venta AS PrecioVenta,
                    vt.FechaCreacion AS FechaCreacion,
                    vt.FechaModificacion AS FechaModificacion,
                    vt.EsEliminado AS EsEliminado
                FROM Ventas_stock AS vt
                WHERE vt.EsEliminado = FALSE";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXVentaID = lector.GetOrdinal("VentaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXMedioPagoID = lector.GetOrdinal("MedioPagoID");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Ventas Venta = new Ventas
                            {
                                ID = lector.IsDBNull(IDXVentaID) ? "" : lector.GetString(IDXVentaID),
                                MedioPagoID = lector.IsDBNull(IDXMedioPagoID) ? "" : lector.GetString(IDXMedioPagoID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                precioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion)
                            };

                            ventas.Add(Venta);
                        }

                        return ventas;
                    }
                }
            }
        }
        public bool Modificar(Ventas ventaModificada)
        {
            Ventas registroActual = Recuperar(ventaModificada.ID);
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
                            var valorModificado = propiedad.GetValue(ventaModificada);
                            if (!object.Equals(valorActual, valorModificado))
                            {
                                string nombreColumna = MapeoColumnas[propiedad.Name];
                                string nombreParametro = propiedad.Name;
                                listaPropiedadesModificadas.Add($"{nombreColumna} = @{nombreParametro}");
                                comando.Parameters.AddWithValue($"@{nombreParametro}", propiedad.GetValue(ventaModificada) ?? DBNull.Value);
                            }
                        }

                        if (listaPropiedadesModificadas.Count == 0)
                            return false;

                        listaPropiedadesModificadas.Add("FechaModificacion = @FechaActual");
                        string Consulta = $"UPDATE Ventas_stock SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
                        comando.Parameters.AddWithValue("@FechaActual", DateTime.Now);
                        comando.Parameters.AddWithValue("@IDModificar", ventaModificada.ID);
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
                consulta = "UPDATE Ventas_stock SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Ventas_stock WHERE ID = @id;";
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
    }
}
