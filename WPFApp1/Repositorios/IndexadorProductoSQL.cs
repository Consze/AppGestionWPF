using System.Data.SqlClient;
using System.Data.SQLite;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;
using WPFApp1.Servicios;

namespace WPFApp1.Repositorios
{
    public class CoincidenciasBusqueda : PalabrasTitulosProductos
    {
        public int CantidadPalabrasCoincidentes { get; set; }
    }
    public class IndexadorProductoSQLite : IIndexadorProductosRepositorio
    {
        private readonly ConexionDBSQLite _dbConexionSQLite;
        public IndexadorProductoSQLite(ConexionDBSQLite dbConexionLocal)
        {
            _dbConexionSQLite = dbConexionLocal;
        }
        public bool InsertarRegistro(string Palabra, int ID)
        {
            string consulta = "INSERT INTO Productos_titulos (producto_id, palabra) VALUES (@producto_id, @palabra);";
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@producto_id", ID);
                    comando.Parameters.AddWithValue("@palabra", Palabra);
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
        public List<PalabrasTitulosProductos> BuscarPalabra(string Palabra)
        {
            List<PalabrasTitulosProductos> palabraColeccionTitulos = new List<PalabrasTitulosProductos>();
            string Consulta = "SELECT * FROM Productos_titulos WHERE palabra = @palabra COLLATE NOCASE;";
            try
            {
                using(SQLiteCommand comand = new SQLiteCommand(Consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comand.Parameters.AddWithValue("@palabra", Palabra);
                    using(SQLiteDataReader lector = comand.ExecuteReader())
                    {
                        while(lector.Read())
                        {
                            PalabrasTitulosProductos palabraEncontrada = new PalabrasTitulosProductos();
                            palabraEncontrada.palabra = lector["palabra"].ToString();
                            palabraEncontrada.producto_id = Convert.ToInt32(lector["producto_id"]);
                            
                            if(palabraEncontrada.palabra != null)
                            {
                                palabraColeccionTitulos.Add(palabraEncontrada);
                            }
                        }
                    }
                }
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            return palabraColeccionTitulos;
        }
    }
    public class IndexadorProductoSQLServer : IIndexadorProductosRepositorio
    {
        private readonly ConexionDBSQLServer _dbSQLServer;
        public IndexadorProductoSQLServer(ConexionDBSQLServer dbConexionServer)
        {
            _dbSQLServer = dbConexionServer;
        }
        public bool InsertarRegistro(string Palabra, int ID)
        {
            string consulta = "INSERT INTO Productos_titulos (producto_id, palabra) VALUES (@producto_id, @palabra);";
            try
            {
                using (SqlCommand comando = new SqlCommand(consulta, _dbSQLServer.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@producto_id", ID);
                    comando.Parameters.AddWithValue("@palabra", Palabra);
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
        public List<PalabrasTitulosProductos> BuscarPalabra(string Palabra)
        {
            List<PalabrasTitulosProductos> palabraColeccionTitulos = new List<PalabrasTitulosProductos>();
            string Consulta = "SELECT * FROM Productos_titulos WHERE palabra = @palabra;";
            try
            {
                using (SqlCommand comand = new SqlCommand(Consulta, _dbSQLServer.ObtenerConexionDB()))
                {
                    comand.Parameters.AddWithValue("@palabra", Palabra);
                    using (SqlDataReader lector = comand.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            PalabrasTitulosProductos palabraEncontrada = new PalabrasTitulosProductos();
                            palabraEncontrada.palabra = lector["palabra"].ToString();
                            palabraEncontrada.producto_id = Convert.ToInt32(lector["producto_id"]);

                            if (palabraEncontrada.palabra != null)
                            {
                                palabraColeccionTitulos.Add(palabraEncontrada);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return palabraColeccionTitulos;
        }
    }
}
