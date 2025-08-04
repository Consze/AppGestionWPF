using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text.Json;

namespace WPFApp1.Servicios
{
    public class ConfiguracionSQLServer()
    {
        public string CadenaConexion { get; set; }
        public bool ConexionValida { get; set; }
        public DateTime FechaHora { get; set; }
        public string Excepcion { get; set; }
    }
    public class EstadoSQLServer()
    {
        public bool ServidorActivo { get; set; }
    }
    public class ConexionDBSQLServer
    {
        public string CadenaConexion { get; set; }
        public bool ConexionValida { get; set; }
        private string rutaArchivoConfiguracion;
        private string rutaConfiguracionManual; 
        public string ExcepcionSQL { get; set; } 
        public ConexionDBSQLServer()
        {
            this.rutaArchivoConfiguracion = @".\datos\configuracionSQLServer.json";
            this.rutaConfiguracionManual = @".\datos\estadoServidor.json";
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
            var configuracion = new ConfiguracionSQLServer {CadenaConexion = CadenaConexion, ConexionValida = ConexionValida, Excepcion = ExcepcionSQL, FechaHora = DateTime.Now };
            string jsonString = JsonSerializer.Serialize(configuracion);
            try
            {
                File.WriteAllText(rutaArchivoConfiguracion, jsonString);
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
                    ConexionValida = true;
                    ExcepcionSQL = null;
                    return true;
                }
                catch (SqlException ex)
                {
                    ConexionValida = false;
                    ExcepcionSQL = ex.Message;
                    return false;
                }
            }
        }
        public bool GuardarConfiguracionManual(bool entrada)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(rutaConfiguracionManual));
            var configuracion = new EstadoSQLServer { ServidorActivo = entrada };
            string jsonString = JsonSerializer.Serialize(configuracion);
            try
            {
                File.WriteAllText(rutaConfiguracionManual, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la configuración: {ex.Message}");
                return false;
            }
        }
        public bool LeerConfiguracionManual()
        {
            try
            {
                if (File.Exists(this.rutaConfiguracionManual))
                {
                    string jsonString = File.ReadAllText(rutaConfiguracionManual);
                    var configuracion = JsonSerializer.Deserialize<EstadoSQLServer>(jsonString);
                    if (string.IsNullOrEmpty(jsonString) || configuracion == null)
                    {
                        return false;
                    }
                    return configuracion.ServidorActivo;
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(rutaConfiguracionManual));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return false;
            }
        }
    }
    public class TablaEsquema
    {
        public string NombreTabla { get; set; }
        public string DefinicionSQL { get; set; }
    }
    public class ConexionDBSQLite
    {
        public string CadenaConexion { get; private set; } = @"Data Source=.\datos\base.db;Version=3;";
        private string _rutaArchivo { get; set; } = @".\datos\base.db";
        private string _rutaArchivoEsquemaDB { get; set; } = @".\datos\esquemaDB.json";
        private string esquemaDB { get; set; } = @"CREATE TABLE IF NOT EXISTS Libros (
            Nombre TEXT NOT NULL,
            Autor TEXT NOT NULL,
            Categoria_id TEXT NOT NULL,
            Sinopsis TEXT,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME,
            ID TEXT PRIMARY KEY,
            FOREIGN KEY(Categoria_id) REFERENCES Categorias(ID)
        );

        CREATE TABLE IF NOT EXISTS Productos (
            producto_id INTEGER PRIMARY KEY,
            Nombre TEXT NOT NULL,
            Categoria TEXT NOT NULL, 
            Precio INTEGER NOT NULL,  
            ruta_imagen VARCHAR
        );

        CREATE TABLE IF NOT EXISTS Productos_titulos (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            producto_id INTEGER NOT NULL,
            palabra TEXT NOT NULL COLLATE NOCASE,
            UNIQUE (producto_id, palabra) ON CONFLICT IGNORE,
            FOREIGN KEY(producto_id) REFERENCES Productos(producto_id)
        );
    
        CREATE TABLE IF NOT EXISTS Libros_titulos (
            Palabra TEXT NOT NULL COLLATE NOCASE,
            Libro_ID TEXT NOT NULL,
            FOREIGN KEY(Libro_ID) REFERENCES Libros(ID),
            PRIMARY KEY(Palabra, Libro_ID)
        );

        CREATE TABLE IF NOT EXISTS Libros_sinopsis (
            Palabra TEXT NOT NULL COLLATE NOCASE,
            Libro_ID TEXT NOT NULL,
            FOREIGN KEY(Libro_ID) REFERENCES Libros(ID),
            PRIMARY KEY(Palabra, Libro_ID)
        );

        CREATE TABLE IF NOT EXISTS Editoriales (
            Nombre TEXT NOT NULL,
            ID TEXT PRIMARY KEY,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );

        CREATE TABLE IF NOT EXISTS Categorias (
            Nombre TEXT NOT NULL,
            ID TEXT PRIMARY KEY,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );

        CREATE TABLE IF NOT EXISTS Libros_Ediciones (
            libro_id TEXT NOT NULL,
            editorial_id TEXT NOT NULL,
            ISBN TEXT,
            anioPublicacion INT,
            cantidadPaginas INT,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME,
            ID TEXT PRIMARY KEY,
            FOREIGN KEY(libro_id) REFERENCES Libros(ID),
            FOREIGN KEY(editorial_id) REFERENCES Editoriales(ID)
        );

        CREATE TABLE IF NOT EXISTS Libros_Stock (
            Edicion_id TEXT NOT NULL,
            Haber INT NOT NULL,
            ColorDorsoID TEXT NOT NULL,
            RutaImagen TEXT,
            Ubicacion_Inventario TEXT,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            EsEliminado BOOLEAN DEFAULT False,
            SeMuestraOnline BOOLEAN DEFAULT FALSE,
            PrecioPublico BOOLEAN DEFAULT FALSE,
            Condicion TEXT NOT NULL,
            Formato TEXT,
            SKU_Producto TEXT PRIMARY KEY,
            FOREIGN KEY(Ubicacion_Inventario) REFERENCES Ubicaciones_inventario(ID),
            FOREIGN KEY(Edicion_id) REFERENCES Libros_Ediciones(ID),
            FOREIGN KEY (ColorDorsoID) REFERENCES Colores(ID),
            FOREIGN KEY (Condicion) REFERENCES Condiciones(ID),
            FOREIGN KEY (Formato) REFERENCES Libros_Formatos(ID)
        );

        CREATE TABLE IF NOT EXISTS Condiciones (
            ID TEXT PRIMARY KEY,
            Condicion TEXT NOT NULL,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );

        CREATE TABLE IF NOT EXISTS Ubicaciones_inventario (
            ID TEXT PRIMARY KEY,
            Descripcion TEXT NOT NULL,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );

        CREATE TABLE IF NOT EXISTS Libros_Formatos (
            ID TEXT PRIMARY KEY,
            Descripcion TEXT,
            Alto FLOAT NOT NULL,
            Largo FLOAT NOT NULL,
            Ancho FLOAT NOT NULL,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );

        CREATE TABLE IF NOT EXISTS Colores (
            ID TEXT PRIMARY KEY,
            Codigo_Hexadecimal TEXT NOT NULL,
            Nombre TEXT,
            EsEliminado BOOLEAN NOT NULL DEFAULT FALSE,
            FechaModificacion DATETIME
        );";
        public SQLiteConnection Conexion { get; private set; }
        public ConexionDBSQLite()
        {
            bool _BanderaCrearTablas = false;
            if (File.Exists(_rutaArchivo) == false)
            {
                _BanderaCrearTablas = true;
                Console.WriteLine($"Archivo de base de datos local no encontrado en : {_rutaArchivo}");
            }

            using (SQLiteConnection conexion = ObtenerConexionDB())
            {
                try
                {
                    using (SQLiteCommand command = new SQLiteCommand("PRAGMA foreign_keys = ON;", conexion))
                    {
                        command.ExecuteNonQuery();
                    }
                    InicializarEsquema();
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine($"Error en la conexion a la base de datos {ex.Message}");
                    throw;
                }
            }
        }
        public void InicializarEsquema()
        {
            this.CrearTabla(esquemaDB);
        }
        public List<TablaEsquema> LeerArchivoEsquemaTablas()
        {
            if (!File.Exists(_rutaArchivoEsquemaDB))
            {
                return null; 
            }
            string jsonString = File.ReadAllText(_rutaArchivoEsquemaDB);
            var Esquema = JsonSerializer.Deserialize<List<TablaEsquema>>(jsonString);
            return Esquema;
        }
        public void GuardarArchivoEsquemaTablas(List<TablaEsquema> Esquema)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(Esquema, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_rutaArchivoEsquemaDB, jsonString);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public bool ComprobarExistenciaTabla(string NombreTabla)
        {
            using (SQLiteConnection conexion = ObtenerConexionDB())
            {
                string _PruebaConexion = $"SELECT * FROM {NombreTabla};";
                using (SQLiteCommand comando = new SQLiteCommand(_PruebaConexion, conexion))
                {
                    try
                    {
                        using (SQLiteDataReader Lector = comando.ExecuteReader())
                        {
                            return true;
                        }
                    }
                    catch (SQLiteException ex)
                    {
                        if (ex.Message.Contains("no such table"))
                        {
                            return false;
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
            }
        }
        public bool CrearTabla(string consulta)
        {
            using (SQLiteConnection conexion = ObtenerConexionDB())
            {
                try
                {
                    using (SQLiteCommand comando = new SQLiteCommand(consulta, conexion))
                    {
                        comando.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (SQLiteException ex)
                {
                    return false;
                }
            }
            
        }
        public SQLiteConnection ObtenerConexionDB()
        {
            SQLiteConnection conexion = new SQLiteConnection(this.CadenaConexion);
            conexion.Open();
            return conexion;
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
