using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;
using WPFApp1.DTOS;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

namespace WPFApp1.Repositorios
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
        public string esquemaDB { get; set; } = @"
            IF OBJECT_ID('dbo.Productos_titulos', 'U') IS NULL 
            BEGIN
                CREATE TABLE dbo.Productos_titulos (
                    ID INT PRIMARY KEY IDENTITY(1,1),
                    producto_id VARCHAR(36) NOT NULL,
                    palabra VARCHAR(255) NOT NULL
                ); 
            END
            
            IF NOT EXISTS (
                SELECT
                    1
                FROM
                    sys.indexes
                WHERE
                    name = 'IX_Productos_Titulos_ProductoId_Palabra'
                    AND object_id = OBJECT_ID('dbo.Productos_titulos')
            )
            BEGIN
                CREATE UNIQUE NONCLUSTERED INDEX IX_Productos_Titulos_ProductoId_Palabra
                ON dbo.Productos_titulos (producto_id, palabra);
            END";
        public ConexionDBSQLServer()
        {
            rutaArchivoConfiguracion = @".\datos\configuracionSQLServer.json";
            rutaConfiguracionManual = @".\datos\estadoServidor.json";
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
        public bool EjecutarConsultaNoQuery(string Consulta)
        {
            try
            {
                using(SqlConnection conexion = ObtenerConexionDB())
                {
                    using (SqlCommand comando = new SqlCommand(Consulta,conexion))
                    {
                        comando.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
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
                if (File.Exists(rutaConfiguracionManual))
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
        public bool ConexionInicial()
        {
            try
            {
                InicializarEsquemaDB();
                return true;
            }
            catch (Exception ex)
            {
                Notificacion _notificacion = new Notificacion { Mensaje = $"Error: {ex.Message}", Titulo = "Operación Fallida", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                return false;
            }
        }
        public void InicializarEsquemaDB()
        {
            EjecutarConsultaNoQuery(esquemaDB);
        }
        public SqlConnection ObtenerConexionDB()
        {
            ConfiguracionSQLServer configuracionServer = LeerArchivoConfiguracion();
            if (!ProbarConexion(configuracionServer.CadenaConexion))
            {
                throw new InvalidOperationException("No se pudo establecer una conexión válida para SQL Server.");
            }
            SqlConnection Conexion = new SqlConnection(configuracionServer.CadenaConexion);
            Conexion.Open();
            return Conexion;
        }
    }
    public class TablaEsquema
    {
        public string NombreTabla { get; set; }
        public string DefinicionSQL { get; set; }
    }
    public class ConexionDBSQLite
    {
        public string CadenaConexion { get; private set; } = @"Data Source=.\datos\base.db;";
        private string _rutaArchivo { get; set; } = @".\datos\base.db";
        private string _rutaArchivoEsquemaDB { get; set; } = @".\datos\esquemaDB.json";
        private string esquemaDB { get; set; } = @"CREATE TABLE IF NOT EXISTS Productos (
            id VARCHAR(36) PRIMARY KEY,
            Nombre VARCHAR(255) NOT NULL,
            categoria_id VARCHAR(36),
            EsEliminado BOOLEAN NOT NULL DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            FOREIGN KEY(categoria_id) REFERENCES Productos_categorias(ID)
        );

        CREATE TABLE IF NOT EXISTS Facturas (
            ID VARCHAR(36) PRIMARY KEY,
            sucursalID VARCHAR(36),
            FechaVenta DATETIME,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            EsEliminado BOOLEAN DEFAULT 0,
            FOREIGN KEY(sucursalID) REFERENCES Sucursales(ID)
        );

        CREATE TABLE IF NOT EXISTS Facturas_Detalles (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FacturaID VARCHAR(36) NOT NULL,
            productoSKU VARCHAR(36) NOT NULL,
            precio_venta NUMERIC(18,2) NOT NULL DEFAULT 0,
            cantidad INT,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            EsEliminado BOOLEAN DEFAULT 0,
            FOREIGN KEY(FacturaID) REFERENCES Facturas(ID),
            FOREIGN KEY(productoSKU) REFERENCES Productos_Stock(SKU_Producto)
        );

        CREATE TABLE IF NOT EXISTS Facturas_pagos (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            FacturaID VARCHAR(36),
            medio_pago VARCHAR(36),
            monto NUMERIC(18,2),
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            EsEliminado BOOLEAN DEFAULT 0,
            FOREIGN KEY(FacturaID) REFERENCES Facturas(ID),
            FOREIGN KEY(medio_pago) REFERENCES Medios_pago(ID)
        );

        CREATE TABLE IF NOT EXISTS Medios_pago (
            ID VARCHAR(36) PRIMARY KEY,
            cuenta_asociada VARCHAR(36) NOT NULL,
            Nombre VARCHAR NOT NULL COLLATE NOCASE,
            EsEliminado BOOLEAN DEFAULT 0,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            UNIQUE (Nombre) ON CONFLICT IGNORE,
            FOREIGN KEY(cuenta_asociada) REFERENCES Cuentas_contables(id)
        );

        CREATE TABLE IF NOT EXISTS Sucursales (
            ID VARCHAR(36) PRIMARY KEY,
            Nombre VARCHAR(255) NOT NULL COLLATE NOCASE,
            Localidad VARCHAR(100) NOT NULL COLLATE NOCASE,    
            Calle VARCHAR(255) NOT NULL COLLATE NOCASE,
            Altura INT NOT NULL,
            Telefono VARCHAR(50),
            Latitud NUMERIC(8,6),
            Longitud NUMERIC(9,6),
            EsEliminado BOOLEAN DEFAULT 0,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            UNIQUE (Nombre, Localidad) ON CONFLICT IGNORE
        );

        CREATE IF NOT EXISTS TABLE Cuentas_contables (
            ID VARCHAR(36) PRIMARY KEY,
            nombre VARCHAR(255) COLLATE NOCASE,
            tipo_cuenta VARCHAR(255) COLLATE NOCASE,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            EsEliminado BOOLEAN DEFAULT 0,
            UNIQUE (nombre, tipo_cuenta) ON CONFLICT IGNORE
        );

        CREATE TABLE IF NOT EXISTS Productos_categorias (
            ID VARCHAR(36) PRIMARY KEY,
            Nombre TEXT NOT NULL COLLATE NOCASE,
            EsEliminado BOOLEAN NOT NULL DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            UNIQUE (Nombre) ON CONFLICT IGNORE
        );

        CREATE IF NOT EXISTS TABLE Productos_condiciones  (
            ID VARCHAR(36) PRIMARY KEY,
            Nombre VARCHAR(255) NOT NULL,
            EsEliminado BOOLEAN NOT NULL DEFAULT 0,
            FechaCreacion DATETIME,
            FechaModificacion DATETIME,
            UNIQUE (Nombre) ON CONFLICT IGNORE
        );

        CREATE TABLE IF NOT EXISTS Productos_titulos (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            producto_id VARCHAR(36),
            palabra VARCHAR NOT NULL COLLATE NOCASE,
            UNIQUE (producto_id, palabra) ON CONFLICT IGNORE,
            FOREIGN KEY(producto_id) REFERENCES Productos(id)
        );

        CREATE TABLE IF NOT EXISTS Productos_descripciones (
            ID INTEGER PRIMARY KEY AUTOINCREMENT,
            Palabra VARCHAR NOT NULL COLLATE NOCASE,
            producto_id VARCHAR(36) NOT NULL,
            UNIQUE (producto_id, palabra) ON CONFLICT IGNORE,
            FOREIGN KEY(producto_id) REFERENCES Productos(id)
        );

        CREATE TABLE IF NOT EXISTS Productos_versiones (
            ID VARCHAR(36) PRIMARY KEY,
            producto_id VARCHAR(36) NOT NULL,
            EAN VARCHAR(13),
            Marca_id VARCHAR(36),
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            EsEliminado BOOLEAN DEFAULT 0,
            formato_id VARCHAR(36),
            RutaRelativaImagen TEXT,
            condicion_id VARCHAR(36),
            UNIQUE (producto_id, EAN, Marca_id, formato_id, RutaRelativaImagen, condicion_id) ON CONFLICT IGNORE,
            FOREIGN KEY(formato_id) REFERENCES Productos_formatos(ID),
            FOREIGN KEY(condicion_id) REFERENCES Productos_condiciones(ID),
            FOREIGN KEY(producto_id) REFERENCES Productos(ID),
            FOREIGN KEY(Marca_id) REFERENCES Marcas(ID)
        );

        CREATE TABLE IF NOT EXISTS Productos_formatos (
            ID VARCHAR(36) PRIMARY KEY,
            descripcion TEXT NOT NULL COLLATE NOCASE,
            alto NUMERIC(18, 1) NOT NULL,
            ancho NUMERIC(18, 1) NOT NULL,
            largo NUMERIC(18, 1) NOT NULL,
            peso NUMERIC(18, 1) DEFAULT 1,
            EsEliminado BOOLEAN DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            UNIQUE (alto, ancho, largo, peso, descripcion) ON CONFLICT IGNORE
        );

        CREATE TABLE IF NOT EXISTS Productos_Stock (
            SKU_Producto VARCHAR(36) PRIMARY KEY,
            ubicacion_id VARCHAR(36),
            producto_version_id VARCHAR(36) NOT NULL,
            Haber INT,
            Precio NUMERIC(18,2),
            EsEliminado BOOLEAN DEFAULT 0,
            SeMuestraOnline BOOLEAN DEFAULT 0,
            PrecioPublico BOOLEAN DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME, 
            FOREIGN KEY(producto_version_id) REFERENCES Productos_versiones(ID),
            FOREIGN KEY(ubicacion_id) REFERENCES Ubicaciones_inventario(ID)
        );

        CREATE TABLE IF NOT EXISTS Marcas (
            ID VARCHAR(36) PRIMARY KEY,
            Nombre TEXT NOT NULL COLLATE NOCASE,
            EsEliminado BOOLEAN NOT NULL DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            UNIQUE (Nombre) ON CONFLICT IGNORE
        );

        CREATE TABLE IF NOT EXISTS Ubicaciones_inventario (
            ID VARCHAR(36) PRIMARY KEY,
            Descripcion TEXT NOT NULL COLLATE NOCASE,
            EsEliminado BOOLEAN NOT NULL DEFAULT 0,
            FechaModificacion DATETIME,
            FechaCreacion DATETIME,
            UNIQUE (Descripcion) ON CONFLICT IGNORE
        );";
        public SqliteConnection Conexion { get; private set; }
        public ConexionDBSQLite()
        {
            bool _BanderaCrearTablas = false;
            if (File.Exists(_rutaArchivo) == false)
            {
                _BanderaCrearTablas = true;
                Console.WriteLine($"Archivo de base de datos local no encontrado en : {_rutaArchivo}");
            }

            using (SqliteConnection conexion = ObtenerConexionDB())
            {
                try
                {
                    using (SqliteCommand command = new SqliteCommand("PRAGMA foreign_keys = ON;", conexion))
                    {
                        command.ExecuteNonQuery();
                    }
                    conexion.Close();
                    InicializarEsquema();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Error en la conexion a la base de datos {ex.Message}");
                    throw;
                }
            }
        }
        public bool ConexionInicial()
        {
            return false;
        }
        public void InicializarEsquema()
        {
            CrearTabla(esquemaDB);
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
            using (SqliteConnection conexion = ObtenerConexionDB())
            {
                string _PruebaConexion = $"SELECT * FROM {NombreTabla};";
                using (SqliteCommand comando = new SqliteCommand(_PruebaConexion, conexion))
                {
                    try
                    {
                        using (SqliteDataReader Lector = comando.ExecuteReader())
                        {
                            return true;
                        }
                    }
                    catch (SqliteException ex)
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
            using (SqliteConnection conexion = ObtenerConexionDB())
            {
                try
                {
                    using (SqliteCommand comando = new SqliteCommand(consulta, conexion))
                    {
                        comando.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (SqliteException ex)
                {
                    return false;
                }
            }
            
        }
        public SqliteConnection ObtenerConexionDB()
        {
            SqliteConnection conexion = new SqliteConnection(CadenaConexion);
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
