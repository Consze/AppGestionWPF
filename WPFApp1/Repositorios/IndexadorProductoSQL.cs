using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.Data.SQLite;
using WPFApp1.DTOS;
using WPFApp1.Interfaces;

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
        public bool InsertarRegistro(string Palabra, string ID)
        {
            string consulta = "INSERT INTO Productos_titulos (producto_id, palabra) VALUES (@producto_id, @palabra);";
            try
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@producto_id", ID);
                    comando.Parameters.AddWithValue("@palabra", Palabra);
                    comando.ExecuteNonQuery();
                    return true;
                }
            }
            catch(SqliteException ex)
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
                            palabraEncontrada.producto_id = lector["producto_id"].ToString();
                            
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
        public List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(string producto_id)
        {
            List<IDX_Prod_Titulos> registrosIDX = new List<IDX_Prod_Titulos>();

            string consulta = "SELECT * FROM Productos_titulos WHERE producto_id = @id";
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@id", producto_id);
                    using (SQLiteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            int ID = Convert.ToInt32(lector["id"]);
                            string prod_id = lector["producto_id"].ToString();
                            string Palabra = lector["palabra"].ToString();
                            IDX_Prod_Titulos registro = new IDX_Prod_Titulos
                            {
                                ID = ID,
                                producto_id = prod_id,
                                palabra = Palabra
                            };
                            registrosIDX.Add(registro);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return registrosIDX;
        }
        public bool EliminarIndicesPorID(List<int> indicesID)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, indicesID.Count).Select(contador => $"@id{contador}"));
            if (string.IsNullOrEmpty(parametros))
            {
                return false;
            }

            int filasAfectadas = 0;

            string consulta = $"DELETE FROM Productos_titulos WHERE ID IN ({parametros})";
            using (SQLiteCommand comando = new SQLiteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
            {
                int contador = 0;
                foreach (int ID in indicesID)
                {
                    comando.Parameters.AddWithValue($"@id{contador}", ID);
                    contador++;
                }
                filasAfectadas = comando.ExecuteNonQuery();
            }

            return filasAfectadas > 0;
        }
    }
    public class IndexadorProductoSQLServer : IIndexadorProductosRepositorio
    {
        private readonly ConexionDBSQLServer _dbSQLServer;
        public IndexadorProductoSQLServer(ConexionDBSQLServer dbConexionServer)
        {
            _dbSQLServer = dbConexionServer;
        }
        public bool InsertarRegistro(string Palabra, string ID)
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
                            palabraEncontrada.producto_id = lector["producto_id"].ToString();

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
        public List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(string producto_id)
        {
            List<IDX_Prod_Titulos> registrosIDX = new List<IDX_Prod_Titulos>();

            string consulta = "SELECT * FROM dbo.Productos_titulos WHERE producto_id = @id";
            try
            {
                using (SqlCommand comando = new SqlCommand(consulta, _dbSQLServer.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@id", producto_id);
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while(lector.Read())
                        {
                            int ID = Convert.ToInt32(lector["id"]);
                            string prod_id = lector["producto_id"].ToString();
                            string Palabra = lector["palabra"].ToString();
                            IDX_Prod_Titulos registro = new IDX_Prod_Titulos
                            {
                                ID = ID,
                                producto_id = prod_id,
                                palabra = Palabra
                            };
                            registrosIDX.Add(registro);
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return registrosIDX;
        }
        public bool EliminarIndicesPorID(List<int> indicesID)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, indicesID.Count).Select(contador => $"@id{contador}"));
            if (string.IsNullOrEmpty(parametros))
            {
                return false;
            }

            int filasAfectadas = 0;

            string consulta = $"DELETE FROM dbo.Productos_titulos WHERE ID IN ({parametros})";
            using (SqlCommand comando = new SqlCommand(consulta, _dbSQLServer.ObtenerConexionDB()))
            {
                int contador = 0;
                foreach(int ID in indicesID)
                {
                    comando.Parameters.AddWithValue($"@id{contador}",ID);
                    contador++;
                }
                filasAfectadas = comando.ExecuteNonQuery();
            }

            return filasAfectadas > 0;
        }
    }
}
