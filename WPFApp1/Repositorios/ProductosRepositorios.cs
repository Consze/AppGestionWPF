using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.IO;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class SQLiteAccesoProductos : IProductosAccesoDatos
    {
        private readonly ConexionDBSQLite _accesoDB;
        public SQLiteAccesoProductos(ConexionDBSQLite accesoDB)
        {
            _accesoDB = accesoDB;
        }
        public Productos RecuperarProductoPorID(string producto_id)
        {
            Productos Registro = new Productos(null, "", "", 0, "");
            string Consulta = "SELECT * FROM Productos WHERE id = @id;";
            try
            {
                using (SqliteCommand Comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    Comando.Parameters.AddWithValue("@id", producto_id);
                    using (SqliteDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            Registro.ID = Lector["id"].ToString();
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
                _accesoDB.CerrarConexionDB();
            }
        }
        public bool ActualizarProducto(Productos productoModificado)
        {
            Productos ProductoVigente = RecuperarProductoPorID(productoModificado.ID);

            if (ProductoVigente != null) // Validar registro
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
                    if (Propiedades.ContadorCambios > 0)
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
                    if (Propiedades.ContadorCambios > 0)
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

                string rutaAbsolutaVigente = string.IsNullOrWhiteSpace(ProductoVigente.RutaImagen) ? string.Empty : Path.GetFullPath(ProductoVigente.RutaImagen);
                string rutaModificada = string.IsNullOrWhiteSpace(productoModificado.RutaImagen) ? string.Empty : Path.GetFullPath(productoModificado.RutaImagen);
                if (rutaModificada != rutaAbsolutaVigente)
                {
                    if (Propiedades.ContadorCambios > 0)
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
                Consulta += " WHERE id = @id;";

                if (Propiedades.ContadorCambios > 0)
                {
                    using (SqliteCommand Comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                    {
                        Comando.Parameters.AddWithValue("@id", productoModificado.ID);
                        if (Propiedades.NombreCambiado) { Comando.Parameters.AddWithValue("@Nombre", productoModificado.Nombre); }
                        if (Propiedades.CategoriaCambiada) { Comando.Parameters.AddWithValue("@Categoria", productoModificado.Categoria); }
                        if (Propiedades.PrecioCambiado) { Comando.Parameters.AddWithValue("@Precio", productoModificado.Precio); }
                        if (Propiedades.RutaImagenCambiada) { Comando.Parameters.AddWithValue("@ruta_imagen", productoModificado.RutaImagen); }

                        int FilasAfectadas = Comando.ExecuteNonQuery();
                        _accesoDB.CerrarConexionDB();
                        return FilasAfectadas > 0;
                    }
                }
                else // No se modifico ninguna propiedad
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
        public string CrearProducto(Productos producto)
        {
            string Consulta = "INSERT INTO Productos (ID, Nombre, Categoria, Precio, Ruta_imagen) VALUES (@id, @nombre, @categoria, @precio, @ruta_imagen);";
            string nuevoProductoId = null;
            try
            {
                using (SqliteCommand Comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    Comando.Parameters.AddWithValue("@id", producto.ID);
                    Comando.Parameters.AddWithValue("@nombre", producto.Nombre);
                    Comando.Parameters.AddWithValue("@categoria", producto.Categoria);
                    Comando.Parameters.AddWithValue("@precio", producto.Precio);
                    Comando.Parameters.AddWithValue("@ruta_imagen", producto.RutaImagen);
                    Comando.ExecuteNonQuery();

                    return producto.ID;
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return producto.ID;
            }
            finally
            {
                _accesoDB.CerrarConexionDB();
            }
        }
        public bool EliminarProducto(string producto_id)
        {
            Productos Registro = RecuperarProductoPorID(producto_id);
            string Consulta = "DELETE FROM Productos WHERE id = @id";
            if (Registro != null)
            {
                try
                {
                    using (SqliteCommand Comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                    {
                        Comando.Parameters.AddWithValue("@id", Registro.ID);
                        int FilasAfectadas = Comando.ExecuteNonQuery();
                        return FilasAfectadas > 0;
                    }
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<Productos> LeerProductos()
        {
            List<Productos> ListaProductos = new List<Productos>();
            string consulta = "SELECT * FROM Productos";

            using (SqliteCommand Comando = new SqliteCommand(consulta, _accesoDB.ObtenerConexionDB()))
            {
                using (SqliteDataReader Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        string ProductoID = Lector["id"].ToString();
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
            return ListaProductos;
        }
        public async IAsyncEnumerable<Productos> LeerProductosAsync()
        {
            string consulta = "SELECT id, Nombre, Precio, Categoria, ruta_imagen FROM Productos";
            using (var conexion = _accesoDB.ObtenerConexionDB())
            {
                await conexion.OpenAsync();
                using (var comando = new SqliteCommand(consulta, conexion))
                {
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            string productoID = lector["id"].ToString();
                            int precio = Convert.ToInt32(lector["Precio"]);
                            string nombre = lector["Nombre"].ToString();
                            string categoria = lector["Categoria"].ToString();
                            string rutaImagen = lector["ruta_imagen"].ToString();

                            if (!string.IsNullOrWhiteSpace(rutaImagen))
                            {
                                rutaImagen = Path.GetFullPath(rutaImagen);
                            }

                            var registroActual = new Productos(productoID, nombre, categoria, precio, rutaImagen);
                            yield return registroActual;
                        }
                    }
                }
            }
        }
        public bool CrearLibro(List<Productos> Productos)
        {
            XSSFWorkbook WorkBook = new XSSFWorkbook();
            ISheet Hoja;
            Hoja = WorkBook.CreateSheet("Productos");

            // Crear Filas
            IRow EncabezadoFila = Hoja.CreateRow(0);
            EncabezadoFila.CreateCell(0).SetCellValue("Nombre de Producto");
            EncabezadoFila.CreateCell(1).SetCellValue("Categoría");
            EncabezadoFila.CreateCell(2).SetCellValue("Precio");
            EncabezadoFila.CreateCell(3).SetCellValue("ID");

            // Escribir datos
            int NumeroDeFila = 1;
            for (int i = 0; i < Productos.Count; i++)
            {
                IRow Fila = Hoja.CreateRow(NumeroDeFila++);
                Fila.CreateCell(0).SetCellValue(Productos[i].Nombre);
                Fila.CreateCell(1).SetCellValue(Productos[i].Categoria);
                Fila.CreateCell(2).SetCellValue(Productos[i].Precio.ToString());
                Fila.CreateCell(3).SetCellValue(Productos[i].ID.ToString());
            }

            // Crear Archivo XLSX
            string DestinoArchivo = ".\\Exportaciones\\Productos_Exportados.xlsx";
            int NumeroIntento = 0;

            while (File.Exists(DestinoArchivo))
            {
                DestinoArchivo = $".\\Exportaciones\\Productos_Exportados{NumeroIntento++}.xlsx";
            }

            try
            {
                using (FileStream file = new FileStream(DestinoArchivo, FileMode.Create, FileAccess.Write))
                {
                    WorkBook.Write(file);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
    }

    public class SQLServerAccesoProductos : IProductosAccesoDatos
    {
        private readonly string _conexionCadena;
        public SQLServerAccesoProductos(string rutaConexion)
        {
            _conexionCadena = rutaConexion;
        }
        public Productos RecuperarProductoPorID(string producto_id)
        {
            string consulta = "SELECT * FROM Productos WHERE id = @id;";
            Productos registro = new Productos(null, "", "", 0, "");
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
                            registro.ID = Lector["id"].ToString();
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
            string Consulta = "UPDATE Productos SET ";
            Productos ProductoVigente = RecuperarProductoPorID(productoModificado.ID);
            if(ProductoVigente != null)
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
                    if (Propiedades.ContadorCambios > 0)
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
                    if (Propiedades.ContadorCambios > 0)
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

                string rutaAbsolutaVigente = string.IsNullOrWhiteSpace(ProductoVigente.RutaImagen) ? string.Empty : Path.GetFullPath(ProductoVigente.RutaImagen);
                string rutaModificada = string.IsNullOrWhiteSpace(productoModificado.RutaImagen) ? string.Empty : Path.GetFullPath(productoModificado.RutaImagen);
                if (rutaModificada != rutaAbsolutaVigente)
                {
                    if (Propiedades.ContadorCambios > 0)
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
                Consulta += " WHERE id = @id;";

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
        public string CrearProducto(Productos producto)
        {
            string Consulta = "INSERT INTO Productos (ID, Nombre, Categoria, Precio, Ruta_imagen) VALUES (@id, @Nombre, @Categoria, @Precio, @Ruta_imagen); SELECT SCOPE_IDENTITY();";
            try
            {
                using (SqlConnection conexion = new SqlConnection(_conexionCadena))
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand(Consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@id", producto.ID);
                        comando.Parameters.AddWithValue("@Nombre", producto.Nombre);
                        comando.Parameters.AddWithValue("@Categoria", producto.Categoria);
                        comando.Parameters.AddWithValue("@Precio", producto.Precio);
                        comando.Parameters.AddWithValue("@Ruta_imagen", producto.RutaImagen);
                        comando.ExecuteNonQuery();
                        return producto.ID;
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public bool EliminarProducto(string producto_id)
        {
            string consulta = "DELETE FROM Productos WHERE id = @id;";
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
        public List<Productos> LeerProductos()
        {
            List<Productos> RegistrosProductos = new List<Productos>();
            string Consulta = "SELECT * FROM Productos";
            try
            {
                using(SqlConnection conexion = new SqlConnection(_conexionCadena))
                {
                    conexion.Open();
                    using(SqlCommand comando = new SqlCommand(Consulta, conexion))
                    {
                        using(SqlDataReader lector = comando.ExecuteReader())
                        {
                            while(lector.Read())
                            {
                                Productos _registro = new Productos(null, "", "", 0 , "");
                                _registro.Nombre = lector["Nombre"].ToString();
                                _registro.ID = lector["id"].ToString();
                                _registro.Precio = Convert.ToInt32(lector["Precio"]);
                                _registro.Categoria = lector["categoria"].ToString();
                                _registro.RutaImagen = string.IsNullOrWhiteSpace(lector["ruta_imagen"].ToString()) ? string.Empty : Path.GetFullPath(lector["ruta_imagen"].ToString());

                                RegistrosProductos.Add(_registro);
                            }
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return RegistrosProductos;
        }
        public async IAsyncEnumerable<Productos> LeerProductosAsync()
        {
            string consulta = "SELECT id, Nombre, Precio, Categoria, ruta_imagen FROM Productos";

            using (var conexion = new SqlConnection(_conexionCadena))
            {
                await conexion.OpenAsync();
                using (var comando = new SqlCommand(consulta, conexion))
                {
                    using (var lector = await comando.ExecuteReaderAsync())
                    {
                        while (await lector.ReadAsync())
                        {
                            string productoID = lector["id"].ToString();
                            int precio = Convert.ToInt32(lector["Precio"]);
                            string nombre = lector["Nombre"].ToString();
                            string categoria = lector["Categoria"].ToString();
                            string rutaImagen = lector["ruta_imagen"].ToString();

                            if (!string.IsNullOrWhiteSpace(rutaImagen))
                            {
                                rutaImagen = Path.GetFullPath(rutaImagen);
                            }

                            var registroActual = new Productos(productoID, nombre, categoria, precio, rutaImagen);
                            yield return registroActual;
                        }
                    }
                }
            }
        }
        public bool CrearLibro(List<Productos> Productos)
        {
            XSSFWorkbook WorkBook = new XSSFWorkbook();
            ISheet Hoja;
            Hoja = WorkBook.CreateSheet("Productos");

            // Crear Filas
            IRow EncabezadoFila = Hoja.CreateRow(0);
            EncabezadoFila.CreateCell(0).SetCellValue("Nombre de Producto");
            EncabezadoFila.CreateCell(1).SetCellValue("Categoría");
            EncabezadoFila.CreateCell(2).SetCellValue("Precio");
            EncabezadoFila.CreateCell(3).SetCellValue("ID");

            // Escribir datos
            int NumeroDeFila = 1;
            for (int i = 0; i < Productos.Count; i++)
            {
                IRow Fila = Hoja.CreateRow(NumeroDeFila++);
                Fila.CreateCell(0).SetCellValue(Productos[i].Nombre);
                Fila.CreateCell(1).SetCellValue(Productos[i].Categoria);
                Fila.CreateCell(2).SetCellValue(Productos[i].Precio.ToString());
                Fila.CreateCell(3).SetCellValue(Productos[i].ID.ToString());
            }

            // Crear Archivo XLSX
            string DestinoArchivo = ".\\Exportaciones\\Productos_Exportados.xlsx";
            int NumeroIntento = 0;

            while (File.Exists(DestinoArchivo))
            {
                DestinoArchivo = $".\\Exportaciones\\Productos_Exportados{NumeroIntento++}.xlsx";
            }

            try
            {
                using (FileStream file = new FileStream(DestinoArchivo, FileMode.Create, FileAccess.Write))
                {
                    WorkBook.Write(file);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
    }
}

