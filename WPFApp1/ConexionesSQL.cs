﻿using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;

namespace WPFApp1
{
    public class ConfiguracionSQLServer()
    {
        public string CadenaConexion { get; set; }
        public bool ConexionValida { get; set; }
        public DateTime FechaHora { get; set; }
        public string Excepcion { get; set; }
    }
    public class ConexionDBSQLServer
    {
        public string CadenaConexion { get; set; }
        public bool ConexionValida { get; set; }
        private string rutaArchivoConfiguracion { get; set; }
        public string ExcepcionSQL { get; set; } 
        public ConexionDBSQLServer()
        {
            this.rutaArchivoConfiguracion = @".\datos\configuracionSQLServer.json";
        }
        public ConfiguracionSQLServer LeerArchivoConfiguracion()
        {
            try
            {
                if (File.Exists(rutaArchivoConfiguracion))
                {
                    string jsonString = File.ReadAllText(rutaArchivoConfiguracion);
                    var configuracion = JsonSerializer.Deserialize<ConfiguracionSQLServer>(jsonString);
                    return configuracion;
                }
                else
                {
                    File.Create(rutaArchivoConfiguracion);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return null;
            }
        }
        public void GuardarEstadoConexion()
        {
            var configuracion = new ConfiguracionSQLServer {CadenaConexion = this.CadenaConexion, ConexionValida = this.ConexionValida, Excepcion = this.ExcepcionSQL, FechaHora = DateTime.Now };
            string jsonString = JsonSerializer.Serialize(configuracion);
            try
            {
                File.WriteAllText(this.rutaArchivoConfiguracion, jsonString);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public bool ProbarConexion(string ruta)
        {
            using (SqlConnection conexion = new SqlConnection(ruta))
            {
                try
                {
                    conexion.Open();
                    this.ConexionValida = true;
                    this.ExcepcionSQL = null;
                    return true;
                }
                catch (SqlException ex)
                {
                    this.ConexionValida = false;
                    this.ExcepcionSQL = ex.Message;
                    return false;
                }
            }
        }
    }
    public class ConexionDBSQLite
    {
        public string CadenaConexion { get; private set; } = @"Data Source=.\datos\base.db;Version=3;";
        private string _rutaArchivo { get; set; } = @".\datos\base.db";
        public SQLiteConnection Conexion { get; private set; }

        public ConexionDBSQLite()
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
                    CrearTablaPersonas(Conexion);
                    CrearTablaProductos(Conexion);
                }
                else
                {
                    string _PruebaConexion = "SELECT * FROM Personas";
                    using (SQLiteCommand comando = new SQLiteCommand(_PruebaConexion, Conexion))
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
                                CrearTablaPersonas(Conexion);
                                Console.WriteLine("La tabla de personas no existia y fue creada nuevamente");
                            }
                        }

                    }
                    _PruebaConexion = "SELECT * FROM Productos";
                    using (SQLiteCommand comando = new SQLiteCommand(_PruebaConexion, Conexion))
                    {
                        try
                        {
                            using (SQLiteDataReader Lector = comando.ExecuteReader())
                            {
                                Console.WriteLine("La tabla de productos existe y es accesible");
                            }
                        }
                        catch (SQLiteException ex)
                        {
                            if (ex.Message.Contains("no such table"))
                            {
                                CrearTablaProductos(Conexion);
                                Console.WriteLine("La tabla de productos no existia y fue creada nuevamente");
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

        public static bool CrearTablaPersonas(SQLiteConnection Conexion)
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
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error al crear tabla personas {ex.Message}");
                return false;
            }
        }

        public static bool CrearTablaProductos(SQLiteConnection Conexion)
        {
            try
            {
                string consulta = "CREATE TABLE IF NOT EXISTS Productos (" +
                    "producto_id INTEGER PRIMARY KEY, " +
                    "Nombre TEXT NOT NULL," +
                    "Categoria TEXT NOT NULL," +
                    "Precio INTEGER NOT NULL, " +
                    "ruta_imagen VARCHAR);";

                using (SQLiteCommand comando = new SQLiteCommand(consulta, Conexion))
                {
                    comando.ExecuteNonQuery();
                    Console.WriteLine("Tabla 'Productos' creada");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error al crear tabla productos {ex.Message}");
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
