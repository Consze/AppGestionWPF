using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;

namespace WPFApp1.Servicios
{
    public class SQLiteAccesoProductos : IProductosAccesoDatos
    {
        private readonly string _conexionCadena;
        public SQLiteAccesoProductos(string rutaConexion)
        {
            _conexionCadena = rutaConexion;
        }
        public Productos RecuperarProductoPorID(int producto_id)
        {
            Productos Registro = new Productos(0, "", "", 0, "");
            ConexionDBSQLite Instancia = new ConexionDBSQLite();
            string Consulta = "SELECT * FROM Productos WHERE producto_id = @id;";
            try
            {
                using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                {
                    Comando.Parameters.AddWithValue("@id", producto_id);
                    using (SQLiteDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            Registro.ID = Convert.ToInt32(Lector["producto_id"]);
                            Registro.Nombre = Lector["Nombre"].ToString();
                            Registro.Categoria = Lector["Categoria"].ToString();
                            Registro.Precio = Convert.ToInt32(Lector["Precio"]);
                            Registro.RutaImagen = Lector["ruta_imagen"].ToString();
                        }
                    }
                }
                return Registro; 
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                Instancia.CerrarConexionDB();
            }
        }
        public bool ActualizarProducto(Productos productoModificado)
        {
            ConexionDBSQLite Instancia = new ConexionDBSQLite();
            Productos ProductoVigente = RecuperarProductoPorID(productoModificado.ID);

            if (ProductoVigente.ID > 0 && ProductoVigente != null) // Validar registro
            {
                FlagsCambiosProductos Propiedades = new FlagsCambiosProductos();

                // Comprobar cambios
                string Consulta = "UPDATE Productos SET ";
                if (productoModificado.Nombre != ProductoVigente.Nombre)
                {
                    Consulta += "Nombre = @Nombre";
                    Propiedades.NombreCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (productoModificado.Categoria != ProductoVigente.Categoria)
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", Categoria = @Categoria";
                    }
                    else
                    {
                        Consulta += "Categoria = @Categoria";
                    }
                    Propiedades.CategoriaCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (productoModificado.Precio != ProductoVigente.Precio)
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", Precio = @Precio";
                    }
                    else
                    {
                        Consulta += "Precio = @Precio";
                    }
                    Propiedades.PrecioCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (Path.GetFullPath(productoModificado.RutaImagen) != Path.GetFullPath(ProductoVigente.RutaImagen))
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", ruta_imagen = @ruta_imagen";
                    }
                    else
                    {
                        Consulta += "ruta_imagen = @ruta_imagen";
                    }
                    Propiedades.RutaImagenCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                Consulta += " WHERE producto_id = @id;";

                if (Propiedades.ContadorCambios > 0)
                {
                    using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                    {
                        Comando.Parameters.AddWithValue("@id", productoModificado.ID);
                        if (Propiedades.NombreCambiado) { Comando.Parameters.AddWithValue("@Nombre", productoModificado.Nombre); }
                        if (Propiedades.CategoriaCambiada) { Comando.Parameters.AddWithValue("@Categoria", productoModificado.Categoria); }
                        if (Propiedades.PrecioCambiado) { Comando.Parameters.AddWithValue("@Precio", productoModificado.Precio); }
                        if (Propiedades.RutaImagenCambiada) { Comando.Parameters.AddWithValue("@ruta_imagen", productoModificado.RutaImagen); }

                        int FilasAfectadas = Comando.ExecuteNonQuery();
                        Instancia.CerrarConexionDB();
                        return FilasAfectadas > 0;
                    }
                }
                else // No se modifico ninguna propiedad
                {
                    Instancia.CerrarConexionDB();
                    return false;
                }
            }
            else 
            {
                Instancia.CerrarConexionDB();
                return false;
            }
        }
        public int CrearProducto(Productos producto)
        {
            ConexionDBSQLite Instancia = new ConexionDBSQLite();
            string Consulta = "INSERT INTO Productos (Nombre, Categoria, Precio, Ruta_imagen) VALUES (@nombre, @categoria, @precio, @ruta_imagen);";
            int nuevoProductoId = 0;
            try
            {
                using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                {
                    Comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    Comando.Parameters.AddWithValue("@categoria", producto.Categoria);
                    Comando.Parameters.AddWithValue("@precio", producto.Precio);
                    Comando.Parameters.AddWithValue("@ruta_imagen", producto.RutaImagen);
                    Comando.ExecuteNonQuery();

                    Comando.CommandText = "SELECT last_insert_rowid()";
                    nuevoProductoId = Convert.ToInt32(Comando.ExecuteScalar());

                    return nuevoProductoId;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return nuevoProductoId;
            }
            finally
            {
                Instancia.CerrarConexionDB();
            }
        }
        public bool EliminarProducto(int producto_id)
        {
            ConexionDBSQLite Instancia = new ConexionDBSQLite();
            Productos Registro = RecuperarProductoPorID(producto_id);
            string Consulta = "DELETE FROM Productos WHERE producto_id = @id";
            if (Registro != null)
            {
                try
                {
                    using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                    {
                        Comando.Parameters.AddWithValue("@id", Registro.ID);
                        int FilasAfectadas = Comando.ExecuteNonQuery();
                        return FilasAfectadas > 0;
                    }
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                    return false;
                }
                finally
                {
                    Instancia.CerrarConexionDB();
                }
            }
            else
            {
                return false;
            }
        }
        public static List<Productos> LeerProductos()
        {
            List<Productos> ListaProductos = new List<Productos>();
            ConexionDBSQLite Instancia = new ConexionDBSQLite();
            string consulta = "SELECT * FROM Productos";

            using (SQLiteCommand Comando = new SQLiteCommand(consulta, Instancia.Conexion))
            {
                using (SQLiteDataReader Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        int ProductoID = Convert.ToInt32(Lector["producto_id"]);
                        int Precio = Convert.ToInt32(Lector["Precio"]);
                        string Nombre = Lector["Nombre"].ToString();
                        string Categoria = Lector["Categoria"].ToString();
                        string RutaImagen = Lector["ruta_imagen"].ToString();
                        if (!string.IsNullOrWhiteSpace(RutaImagen)) { RutaImagen = Path.GetFullPath(RutaImagen); }
                        Productos _registroActual = new Productos(ProductoID, Nombre, Categoria, Precio, RutaImagen);
                        ListaProductos.Add(_registroActual);
                    }
                }
            }
            Instancia.CerrarConexionDB();
            return ListaProductos;
        }
    }

    public class SQLServerAccesoProductos : IProductosAccesoDatos
    {
        private readonly string _conexionCadena;
        public SQLServerAccesoProductos(string rutaConexion)
        {
            _conexionCadena = rutaConexion;
        }
        public Productos RecuperarProductoPorID(int producto_id)
        {
            string consulta = "SELECT * FROM Productos WHERE producto_id = @id;";
            Productos registro = new Productos(0, "", "", 0, "");
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexionCadena))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", producto_id);
                        SqlDataReader Lector = comando.ExecuteReader();
                        while (Lector.Read())
                        {
                            registro.ID = Convert.ToInt32(Lector["producto_id"]);
                            registro.Nombre = Lector["Nombre"].ToString();
                            registro.Categoria = Lector["Categoria"].ToString();
                            registro.Precio = Convert.ToInt32(Lector["Precio"]);
                            registro.RutaImagen = Lector["ruta_imagen"].ToString();
                        }
                        Lector.Close();
                    }
                }
                return registro;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return null;
            }
        }
        public bool ActualizarProducto(Productos productoModificado)
        {
            string Consulta = "UPDATE Productos SET";
            Productos ProductoVigente = RecuperarProductoPorID(productoModificado.ID);
            if(ProductoVigente != null && ProductoVigente.ID > 0)
            {
                FlagsCambiosProductos Propiedades = new FlagsCambiosProductos();

                if (productoModificado.Nombre != ProductoVigente.Nombre)
                {
                    Consulta += "Nombre = @Nombre";
                    Propiedades.NombreCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (productoModificado.Categoria != ProductoVigente.Categoria)
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", Categoria = @Categoria";
                    }
                    else
                    {
                        Consulta += "Categoria = @Categoria";
                    }
                    Propiedades.CategoriaCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (productoModificado.Precio != ProductoVigente.Precio)
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", Precio = @Precio";
                    }
                    else
                    {
                        Consulta += "Precio = @Precio";
                    }
                    Propiedades.PrecioCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (Path.GetFullPath(productoModificado.RutaImagen) != Path.GetFullPath(ProductoVigente.RutaImagen))
                {
                    if (Consulta.Contains(","))
                    {
                        Consulta += ", ruta_imagen = @ruta_imagen";
                    }
                    else
                    {
                        Consulta += "ruta_imagen = @ruta_imagen";
                    }
                    Propiedades.RutaImagenCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                Consulta += " WHERE producto_id = @id;";

                if (Propiedades.ContadorCambios > 0)
                {
                    try
                    {
                        using (SqlConnection conexion = new SqlConnection(_conexionCadena))
                        {
                            conexion.Open();
                            using (SqlCommand Comando = new SqlCommand(Consulta, conexion))
                            {
                                Comando.Parameters.AddWithValue("@id", productoModificado.ID);
                                if (Propiedades.NombreCambiado) { Comando.Parameters.AddWithValue("@Nombre", productoModificado.Nombre); }
                                if (Propiedades.CategoriaCambiada) { Comando.Parameters.AddWithValue("@Categoria", productoModificado.Categoria); }
                                if (Propiedades.PrecioCambiado) { Comando.Parameters.AddWithValue("@Precio", productoModificado.Precio); }
                                if (Propiedades.RutaImagenCambiada) { Comando.Parameters.AddWithValue("@ruta_imagen", productoModificado.RutaImagen); }
                                int filasAfectadas = Comando.ExecuteNonQuery();
                                return filasAfectadas > 0;
                            }
                        }
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public int CrearProducto(Productos producto)
        {
            string Consulta = "INSERT INTO Productos (Nombre, Categoria, Precio, Ruta_imagen) VALUES (@Nombre, @Categoria, @Precio, @Ruta_imagen); SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexionCadena))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(Consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
                        comando.Parameters.AddWithValue("@Categoria", producto.Categoria);
                        comando.Parameters.AddWithValue("@Precio", producto.Precio);
                        comando.Parameters.AddWithValue("@Ruta_imagen", producto.RutaImagen);

                        object resultado = comando.ExecuteScalar();

                        if (resultado != null && resultado != DBNull.Value)
                        {
                            return Convert.ToInt32(resultado);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
        public bool EliminarProducto(int producto_id)
        {
            string consulta = "DELETE FROM Productos WHERE producto_id = @id;";
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexionCadena))
                {
                    conexion.Open();
                    using(SqlCommand comando = new SqlCommand(consulta,conexion))
                    {
                        comando.Parameters.AddWithValue("@id", producto_id);
                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}

