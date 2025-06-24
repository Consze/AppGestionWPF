using System.Data.SQLite;

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
                        int Precio = Convert.ToInt32(Lector["Precio"]);
                        string Nombre = Lector["Nombre"].ToString();
                        string Categoria = Lector["Categoria"].ToString();
                        string RutaImagen = Lector["ruta_imagen"].ToString();
                        Productos _registroActual = new Productos(Nombre, Categoria, Precio, RutaImagen);
                        ListaProductos.Add(_registroActual);
                    }
                }
            }
            Instancia.CerrarConexionDB();
            return ListaProductos;
        }
    }
}
