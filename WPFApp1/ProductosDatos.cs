﻿using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using WPFApp1.DTOS;

namespace WPFApp1
{
    public class SQLiteAccesoProductos : IProductosAccesoDatos
    {
        private readonly string _conexionCadena;
        public SQLiteAccesoProductos(string rutaConexion)
        {
            this._conexionCadena = rutaConexion;
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

            if (ProductoVigente.ID > 0) // Validar registro
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
                        if (FilasAfectadas > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else // No se modifico ninguna propiedad
                {
                    Instancia.CerrarConexionDB();
                    return false;
                }
            }
            else // No existe el registro
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
            return true;
        }
    }

    public class SQLServerAccesoProductos : IProductosAccesoDatos
    {
        private readonly string _conexionCadena;
        public SQLServerAccesoProductos(string rutaConexion)
        {
            this._conexionCadena = rutaConexion;
        }
        public Productos RecuperarProductoPorID(int producto_id)
        {
            string consulta = "SELECT * FROM Productos WHERE productos_id = @id;";
            Productos registro = new Productos(0, "", "", 0, "");
            try
            {
                using (SqlConnection conexion = new SqlConnection(this._conexionCadena))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {    
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
        public bool ActualizarProducto(Productos producto)
        {
            return false;
        }
        public int CrearProducto(Productos producto)
        {
            string Consulta = "INSERT INTO Productos (Nombre, Categoria, Precio, Ruta_imagen) VALUES (@Nombre, @Categoria, @Precio, @Ruta_imagen); SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection conexion = new SqlConnection(this._conexionCadena))
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
            return true;
        }
    }
}
