using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Enums;
using System.Windows.Media;

namespace WPFApp1.Repositorios
{
    public class RepoFacturaDetallesSQLite : IRepoEntidadGenerica<Factura_Detalles>
    {
        public readonly ConexionDBSQLite accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaDetallesSQLite(ConexionDBSQLite _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"FacturaID", "FacturaID" },
                {"ProductoSKU", "productoSKU" },
                {"PrecioVenta", "precio_venta" },
                {"Cantidad", "cantidad" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura_Detalles Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd. = @ID;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura_Detalles Factura = new Factura_Detalles();

                        if (lector.Read())
                        {
                            Factura.ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid);
                            Factura.FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            Factura.ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU);
                            Factura.PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta);
                            Factura.Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad);
                            Factura.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Factura.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Factura.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Factura;
                    }
                }
            }
        }
        /// <summary>
        /// Inserta un nuevo registro en detalles de facturas.
        /// </summary>
        /// <param name="nuevoDetalleFactura"></param>
        /// <returns></returns>
        public string Insertar(Factura_Detalles nuevoDetalleFactura)
        {
            string consulta = @"INSERT INTO Facturas_Detalles (
                FacturaID, productoSKU, precio_venta, cantidad, FechaCreacion)
            VALUES (
                @FacturaID, @productoSKU, @precioVenta ,@cantidad, @FechaCreacion);";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevoDetalleFactura.FacturaID);
                    comando.Parameters.AddWithValue("@productoSKU", nuevoDetalleFactura.ProductoSKU);
                    comando.Parameters.AddWithValue("@precioVenta",nuevoDetalleFactura.PrecioVenta);
                    comando.Parameters.AddWithValue("@cantidad",nuevoDetalleFactura.Cantidad);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoDetalleFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura_Detalles> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        
                        while (await lector.ReadAsync())
                        {
                            Factura_Detalles FacturaDetalle = new Factura_Detalles()
                            {
                                ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return FacturaDetalle;
                        }
                    }
                }
            }
        }
        public List<Factura_Detalles> RecuperarList()
        {
            List<Factura_Detalles> ListaFacturaDetalles = new List<Factura_Detalles>();
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd.EsEliminado = 0;";

            using (SqliteConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura_Detalles FacturaDetalle = new Factura_Detalles()
                            {
                                ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            ListaFacturaDetalles.Add(FacturaDetalle);
                        }

                        return ListaFacturaDetalles;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas_detalles SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas_detalles WHERE ID = @id;";
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
        public bool Modificar(Factura_Detalles facturaModificada)
        {
            Factura_Detalles registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura_Detalles).GetProperties();
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
                        string Consulta = $"UPDATE Facturas_detalles SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
    public class RepoFacturaDetallesSQLServer : IRepoEntidadGenerica<Factura_Detalles>
    {
        public readonly ConexionDBSQLServer accesoDB;
        public readonly Dictionary<string, string> MapeoColumnas;
        public RepoFacturaDetallesSQLServer(ConexionDBSQLServer _accesoDB)
        {
            accesoDB = _accesoDB;
            MapeoColumnas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                //Propiedad de Clase , Nombre de Columna
                {"ID", "ID" },
                {"FacturaID", "FacturaID" },
                {"ProductoSKU", "productoSKU" },
                {"PrecioVenta", "precio_venta" },
                {"Cantidad", "cantidad" },
                {"EsEliminado", "EsEliminado" },
                {"FechaModificacion", "FechaModificacion"},
                {"FechaCreacion","FechaCreacion" }
            };
        }
        public Factura_Detalles Recuperar(string ID)
        {
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd. = @ID;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@id", ID);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");
                        Factura_Detalles Factura = new Factura_Detalles();

                        if (lector.Read())
                        {
                            Factura.ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid);
                            Factura.FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID);
                            Factura.ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU);
                            Factura.PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta);
                            Factura.Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad);
                            Factura.EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado);
                            Factura.FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion);
                            Factura.FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion);
                        }
                        ;

                        return Factura;
                    }
                }
            }
        }
        /// <summary>
        /// Inserta un nuevo registro en detalles de facturas.
        /// </summary>
        /// <param name="nuevoDetalleFactura"></param>
        /// <returns></returns>
        public string Insertar(Factura_Detalles nuevoDetalleFactura)
        {
            string consulta = @"INSERT INTO Facturas_Detalles (
                FacturaID, productoSKU, precio_venta, cantidad, FechaCreacion)
            VALUES (
                @FacturaID, @productoSKU, @precioVenta ,@cantidad, @FechaCreacion);";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@FacturaID", nuevoDetalleFactura.FacturaID);
                    comando.Parameters.AddWithValue("@productoSKU", nuevoDetalleFactura.ProductoSKU);
                    comando.Parameters.AddWithValue("@precioVenta", nuevoDetalleFactura.PrecioVenta);
                    comando.Parameters.AddWithValue("@cantidad", nuevoDetalleFactura.Cantidad);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    comando.ExecuteNonQuery();
                    return nuevoDetalleFactura.ID;
                }
            }
        }
        public async IAsyncEnumerable<Factura_Detalles> RecuperarStreamAsync()
        {
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = await comando.ExecuteReaderAsync())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (await lector.ReadAsync())
                        {
                            Factura_Detalles FacturaDetalle = new Factura_Detalles()
                            {
                                ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };

                            yield return FacturaDetalle;
                        }
                    }
                }
            }
        }
        public List<Factura_Detalles> RecuperarList()
        {
            List<Factura_Detalles> ListaFacturaDetalles = new List<Factura_Detalles>();
            string consulta = @"SELECT 
                    fd.id AS idRegistro,
                    fd.FacturaID as FacturaID
                    fd.productoSKU AS ProductoSKU,
                    fd.precio_venta AS PrecioVenta,
                    fd.cantidad AS Cantidad,
                    fd.FechaCreacion AS FechaCreacion,
                    fd.FechaModificacion AS FechaModificacion,
                    fd.EsEliminado AS EsEliminado
                FROM Facturas_Detalles AS fd
                WHERE fd.EsEliminado = 0;";

            using (SqlConnection conexion = accesoDB.ObtenerConexionDB())
            {
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        int IDXid = lector.GetOrdinal("idRegistro");
                        int IDXFacturaID = lector.GetOrdinal("FacturaID");
                        int IDXProductoSKU = lector.GetOrdinal("ProductoSKU");
                        int IDXPrecioVenta = lector.GetOrdinal("PrecioVenta");
                        int IDXCantidad = lector.GetOrdinal("Cantidad");
                        int IDXFechaCreacion = lector.GetOrdinal("FechaCreacion");
                        int IDXFechaModificacion = lector.GetOrdinal("FechaModificacion");
                        int IDXEsEliminado = lector.GetOrdinal("EsEliminado");

                        while (lector.Read())
                        {
                            Factura_Detalles FacturaDetalle = new Factura_Detalles()
                            {
                                ID = lector.IsDBNull(IDXid) ? "" : lector.GetString(IDXid),
                                FacturaID = lector.IsDBNull(IDXFacturaID) ? "" : lector.GetString(IDXFacturaID),
                                ProductoSKU = lector.IsDBNull(IDXProductoSKU) ? "" : lector.GetString(IDXProductoSKU),
                                PrecioVenta = lector.IsDBNull(IDXPrecioVenta) ? 0 : lector.GetDecimal(IDXPrecioVenta),
                                Cantidad = lector.IsDBNull(IDXCantidad) ? 0 : lector.GetInt32(IDXCantidad),
                                FechaModificacion = lector.IsDBNull(IDXFechaModificacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaModificacion),
                                FechaCreacion = lector.IsDBNull(IDXFechaCreacion) ? DateTime.MinValue : lector.GetDateTime(IDXFechaCreacion),
                                EsEliminado = lector.IsDBNull(IDXEsEliminado) ? false : lector.GetBoolean(IDXEsEliminado)
                            };
                            ListaFacturaDetalles.Add(FacturaDetalle);
                        }

                        return ListaFacturaDetalles;
                    }
                }
            }
        }
        public bool Eliminar(string ID, TipoEliminacion Caso)
        {
            string consulta = "";
            if (Caso == TipoEliminacion.Logica)
            {
                consulta = "UPDATE Facturas_detalles SET EsEliminado = TRUE WHERE ID = @id;";
            }
            else
            {
                consulta = "DELETE FROM Facturas_detalles WHERE ID = @id;";
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
        public bool Modificar(Factura_Detalles facturaModificada)
        {
            Factura_Detalles registroActual = Recuperar(facturaModificada.ID);
            var propiedadesEntidad = typeof(Factura_Detalles).GetProperties();
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
                        string Consulta = $"UPDATE Facturas_detalles SET {string.Join(", ", listaPropiedadesModificadas)} WHERE ID = @IDModificar;";
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
