using System.Data.SQLite;
using System.IO;
using NPOI.SS.Formula.Functions;

namespace WPFApp1
{
    class ConexionDB
    {
        public string CadenaConexion { get; private set; } = "Data Source=.\\datos\\base.db;Version=3;";
        private string _rutaArchivo { get; set; } = ".\\datos\\base.db";
        public SQLiteConnection Conexion { get; private set; }

        public ConexionDB()
        {
            bool _BanderaCrearTablas = false;
            if (File.Exists(_rutaArchivo) == false)
            {
                _BanderaCrearTablas = true;
                Console.WriteLine($"Archivo de base de datos local no encontrado en : {_rutaArchivo}");
            }

            Conexion = new SQLiteConnection(CadenaConexion);
            try
            {
                Conexion.Open();
                if (_BanderaCrearTablas)
                {
                    CrearTablas(Conexion);
                }
                else
                { 
                    string _PruebaConexion = "SELECT * FROM Personas";
                    using(SQLiteCommand comando = new SQLiteCommand(_PruebaConexion,Conexion))
                    {
                        try
                        {
                            using (SQLiteDataReader Lector = comando.ExecuteReader())
                            {
                                Console.WriteLine("La tabla de personas existe y es accesible");
                            }
                        }
                        catch (SQLiteException ex)
                        {
                            if (ex.Message.Contains("no such table"))
                            {
                                CrearTablas(Conexion);
                                Console.WriteLine("La tabla de personas no existia y fue creada nuevamente");
                            }
                        }
                    
                    }
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error en la conexion a la base de datos {ex.Message}");
            }
        }

        public static bool CrearTablas(SQLiteConnection Conexion)
        {
            try
            {
                string consulta = "CREATE TABLE IF NOT EXISTS Personas (" +
                    "persona_id INTEGER PRIMARY KEY, " +
                    "Nombre TEXT NOT NULL," +
                    "Altura INTEGER NOT NULL," +
                    "Peso INTEGER NOT NULL)";

                using (SQLiteCommand comando = new SQLiteCommand(consulta, Conexion))
                {
                    comando.ExecuteNonQuery();
                    Console.WriteLine("Tabla 'Personas' creada");
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine($"Error al crear tabla personas {ex.Message}");
                return false;
            }
        }
        public void CerrarConexionDB()
        {
            if (Conexion != null && Conexion.State == System.Data.ConnectionState.Open)
            {
                Conexion.Close();
                Console.WriteLine("Conexión a la base de datos cerrada.");
            }
        }
    }
}
