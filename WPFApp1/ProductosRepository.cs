using System.Data.SQLite;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WPFApp1
{
    public class ProductosRepository
    {
        public static bool AniadirNuevoProducto(Productos NuevoProducto)
        {
            ConexionDB Instancia = new ConexionDB();
            string Consulta = "INSERT INTO Productos (Nombre, Categoria, Precio, Ruta_imagen) VALUES (@nombre, @categoria, @precio, @ruta_imagen)";
            try
            {
                using (SQLiteCommand Comando = new SQLiteCommand(Consulta,Instancia.Conexion))
                {
                    Comando.Parameters.AddWithValue("@nombre", NuevoProducto.Nombre);
                    Comando.Parameters.AddWithValue("@categoria", NuevoProducto.Categoria);
                    Comando.Parameters.AddWithValue("@precio", NuevoProducto.Precio);
                    Comando.Parameters.AddWithValue("@ruta_imagen", NuevoProducto.RutaImagen);
                    Comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return false;
            }
            finally
            {
                Instancia.CerrarConexionDB();
            }
        }
        public static List<Productos> LeerProductos()
        {
            List<Productos> ListaProductos = new List<Productos>();
            ConexionDB Instancia = new ConexionDB();
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
                        if (!string.IsNullOrWhiteSpace(RutaImagen)) { RutaImagen = System.IO.Path.GetFullPath(RutaImagen); }
                        Productos _registroActual = new Productos(ProductoID,Nombre, Categoria, Precio, RutaImagen);
                        ListaProductos.Add(_registroActual);
                    }
                }
            }
            Instancia.CerrarConexionDB();
            return ListaProductos;
        }
        public static bool ModificarProducto(Productos RegistroModificado)
        {
            ConexionDB Instancia = new ConexionDB();
            Productos ProductoVigente = RecuperarRegistro(RegistroModificado.ID);

            if(ProductoVigente.ID > 0 ) // Validar registro
            {
                FlagsCambiosProductos Propiedades = new FlagsCambiosProductos();

                // Comprobar cambios
                string Consulta = "UPDATE Productos SET ";
                if (RegistroModificado.Nombre != ProductoVigente.Nombre)
                {
                    Consulta += "Nombre = @Nombre";
                    Propiedades.NombreCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (RegistroModificado.Categoria != ProductoVigente.Categoria)
                {
                    Consulta += "Categoria = @Categoria";
                    Propiedades.CategoriaCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                if(RegistroModificado.Precio != ProductoVigente.Precio)
                {
                    Consulta += "Precio = @Precio";
                    Propiedades.PrecioCambiado = true;
                    Propiedades.ContadorCambios += 1;
                }
                if (RegistroModificado.RutaImagen != ProductoVigente.RutaImagen)
                {
                    Consulta += "ruta_imagen = @ruta_imagen";
                    Propiedades.RutaImagenCambiada = true;
                    Propiedades.ContadorCambios += 1;
                }
                Consulta += " WHERE producto_id = @id";

                if(Propiedades.ContadorCambios > 0)
                {
                    using(SQLiteCommand Comando = new SQLiteCommand(Consulta,Instancia.Conexion))
                    {
                        Comando.Parameters.AddWithValue("@id", RegistroModificado.ID);
                        if(Propiedades.NombreCambiado) { Comando.Parameters.AddWithValue("@Nombre", RegistroModificado.Nombre); }
                        if(Propiedades.CategoriaCambiada) { Comando.Parameters.AddWithValue("@Categoria", RegistroModificado.Categoria); }
                        if (Propiedades.PrecioCambiado) { Comando.Parameters.AddWithValue("@Precio", RegistroModificado.Precio); }
                        if (Propiedades.RutaImagenCambiada) { Comando.Parameters.AddWithValue("@ruta_imagen", RegistroModificado.RutaImagen); }

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
        public static bool EliminarProducto(int RegistroID)
        {
            Productos Registro = new Productos(0, "", "", 0, "");
            ConexionDB Instancia = new ConexionDB();
            Registro = RecuperarRegistro(RegistroID);
            string Consulta = "DELETE FROM Productos WHERE producto_id = @id";
            if (Registro.ID > 0)
            {
                try
                {
                    using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
                    {
                        Comando.Parameters.AddWithValue("@id", RegistroID);
                        int FilasAfectadas = Comando.ExecuteNonQuery();
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
                catch(SQLiteException ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                    return false;
                }
                finally
                {
                    Instancia.CerrarConexionDB();
                }
            }
            else // No existe el registro
            {
                return false;
            }
        }
        public static Productos RecuperarRegistro(int RegistroID)
        {
            Productos Registro = new Productos(0,"", "", 0, "");
            ConexionDB Instancia = new ConexionDB();
            string Consulta = "SELECT * FROM Productos WHERE producto_id = @id;";
            using (SQLiteCommand Comando = new SQLiteCommand(Consulta, Instancia.Conexion))
            {
                Comando.Parameters.AddWithValue("@id", RegistroID);
                using (SQLiteDataReader Lector = Comando.ExecuteReader())
                {
                    while(Lector.Read())
                    {
                        Registro.ID = Convert.ToInt32(Lector["producto_id"]);
                        Registro.Nombre = Lector["Nombre"].ToString();
                        Registro.Categoria = Lector["Categoria"].ToString();
                        Registro.Precio = Convert.ToInt32(Lector["Precio"]);
                        Registro.RutaImagen = Lector["ruta_imagen"].ToString();
                    }
                }
            }
            Instancia.CerrarConexionDB();
            return Registro;
        }

        /// <summary>
        /// Crea un archivo XLSX conteniendo Nombre, Categoría, Precio e ID de cada registro de la tabla 'Productos'
        /// </summary>
        /// <param name="Productos"></param>
        /// <returns></returns>
        public static bool CrearLibro(List<Productos> Productos)
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

            while(File.Exists(DestinoArchivo))
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
                System.Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
    }
}
