using System.Data.SQLite;
using WPFApp1.Servicios;

namespace WPFApp1.Repositorios
{
    public class IndexadorProductosRepositorio
    {
        private readonly ConexionDBSQLite _dbConexionProvider;
        public IndexadorProductosRepositorio(ConexionDBSQLite dbConexionProvider)
        {
            _dbConexionProvider = dbConexionProvider;
        }
        public bool InsertarRegistro(string Palabra, int ID)
        {
            string consulta = "INSERT INTO Productos_titulos (producto_id, palabra) VALUES (@producto_id, @palabra);";
            try
            {
                using (SQLiteCommand comando = new SQLiteCommand(consulta, _dbConexionProvider.Conexion))
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

    }
}
