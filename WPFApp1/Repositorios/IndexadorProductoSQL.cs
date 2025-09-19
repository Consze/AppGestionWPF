using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using System.IO;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
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
            string Consulta = "SELECT (palabra, producto_id) FROM Productos_titulos WHERE palabra = @palabra COLLATE NOCASE;";
            try
            {
                using(SqliteCommand comand = new SqliteCommand(Consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comand.Parameters.AddWithValue("@palabra", Palabra);
                    using(SqliteDataReader lector = comand.ExecuteReader())
                    {
                        int IDXPalabra = lector.GetOrdinal("palabra");
                        int IDXProductoID = lector.GetOrdinal("producto_id");

                        while (lector.Read())
                        {
                            PalabrasTitulosProductos palabraEncontrada = new PalabrasTitulosProductos {
                                palabra = lector.GetString(IDXPalabra),
                                producto_id = lector.GetString(IDXProductoID)
                            };
                            
                            if(palabraEncontrada.palabra != null)
                            {
                                palabraColeccionTitulos.Add(palabraEncontrada);
                            }
                        }
                    }
                }
            }
            catch(SqliteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            
            return palabraColeccionTitulos;
        }
        public List<ProductoBase> BuscarProductos(List<string> ColeccionPalabras)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, ColeccionPalabras.Count).Select(i => $"@palabra{i}"));
            List<ProductoBase> registrosCoincidentes = new List<ProductoBase>();
            if (string.IsNullOrEmpty(parametros))
            {
                return registrosCoincidentes;
            }

            string Consulta = $@"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.precio AS ProductoPrecio,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                p.nombre AS ProductoNombre,
                c.nombre AS ProductoCategoria,
                COUNT(t.palabra) AS CantidadCoincidencias
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos AS p ON v.producto_id = p.id            
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Productos_titulos AS t ON t.producto_id = p.id
            WHERE t.palabra IN ({parametros})
                AND s.haber > 0
                AND s.EsEliminado = FALSE
            GROUP BY  ProductoSKU, ProductoPrecio, RutaRelativaImagen, ProductoNombre, ProductoCategoria
            ORDER BY CantidadCoincidencias DESC;";


            using (SqliteCommand comando = new SqliteCommand(Consulta, _dbConexionSQLite.ObtenerConexionDB()))
            {
                for (int i = 0; i < ColeccionPalabras.Count; i++)
                {
                    comando.Parameters.AddWithValue($"@palabra{i}", ColeccionPalabras[i]);
                }

                using (SqliteDataReader lector = comando.ExecuteReader())
                {
                    int IDXNombreProducto = lector.GetOrdinal("ProductoNombre");
                    int IDXNombreCategoria = lector.GetOrdinal("ProductoCategoria");
                    int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                    int IDXSKU = lector.GetOrdinal("ProductoSKU");
                    int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");

                    while(lector.Read())
                    {
                        ProductoBase producto = new ProductoBase
                        {
                            Nombre = lector.IsDBNull(IDXNombreProducto) ? "" : lector.GetString(IDXNombreProducto),
                            Categoria = lector.IsDBNull(IDXNombreCategoria) ? "" : lector.GetString(IDXNombreCategoria),
                            RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                            Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                            ProductoSKU = lector.GetString(IDXSKU)
                        };

                        registrosCoincidentes.Add(producto);
                    }

                    return registrosCoincidentes;
                }
            }
        }
        public List<IDX_Prod_Titulos> RecuperarIndicesPorProductoID(string producto_id)
        {
            List<IDX_Prod_Titulos> registrosIDX = new List<IDX_Prod_Titulos>();

            string consulta = "SELECT * FROM Productos_titulos WHERE producto_id = @id";
            try
            {
                using (SqliteCommand comando = new SqliteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@id", producto_id);
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        int IDXPalabra = lector.GetOrdinal("palabra");
                        int IDXProductoID = lector.GetOrdinal("producto_id");
                        int IDXIndiceID = lector.GetOrdinal("id");

                        while (lector.Read())
                        {
                            IDX_Prod_Titulos registro = new IDX_Prod_Titulos
                            {
                                ID = lector.GetInt32(IDXIndiceID),
                                producto_id = lector.GetString(IDXProductoID),
                                palabra = lector.GetString(IDXPalabra)
                            };
                            registrosIDX.Add(registro);
                        }
                    }
                }
            }
            catch (SqliteException ex)
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
            using (SqliteCommand comando = new SqliteCommand(consulta, _dbConexionSQLite.ObtenerConexionDB()))
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
        public List<ProductoBase> BuscarProductos(List<string> ColeccionPalabras)
        {
            string parametros = string.Join(", ", Enumerable.Range(0, ColeccionPalabras.Count).Select(i => $"@palabra{i}"));
            List<ProductoBase> registrosCoincidentes = new List<ProductoBase>();
            if (string.IsNullOrEmpty(parametros))
            {
                return registrosCoincidentes;
            }

            string Consulta = $@"SELECT 
                s.SKU_Producto AS ProductoSKU,
                s.precio AS ProductoPrecio,
                v.RutaRelativaImagen AS RutaRelativaImagen,
                p.nombre AS ProductoNombre,
                c.nombre AS ProductoCategoria,
                COUNT(t.palabra) AS CantidadCoincidencias
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos AS p ON v.producto_id = p.id            
            INNER JOIN Productos_categorias AS c ON p.categoria_id = c.id
            INNER JOIN Productos_titulos AS t ON t.producto_id = p.id
            WHERE t.palabra IN ({parametros})
                AND s.haber > 0
                AND s.EsEliminado = 0
            GROUP BY  s.SKU_Producto, s.precio, v.RutaRelativaImagen, p.nombre, c.nombre
            ORDER BY CantidadCoincidencias DESC;";

            using (SqlCommand comando = new SqlCommand(Consulta, _dbSQLServer.ObtenerConexionDB()))
            {
                for (int i = 0; i < ColeccionPalabras.Count; i++)
                {
                    comando.Parameters.AddWithValue($"@palabra{i}", ColeccionPalabras[i]);
                }

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    int IDXNombreProducto = lector.GetOrdinal("ProductoNombre");
                    int IDXNombreCategoria = lector.GetOrdinal("ProductoCategoria");
                    int IDXPrecio = lector.GetOrdinal("ProductoPrecio");
                    int IDXSKU = lector.GetOrdinal("ProductoSKU");
                    int IDXRutaImagen = lector.GetOrdinal("RutaRelativaImagen");

                    while (lector.Read())
                    {
                        ProductoBase producto = new ProductoBase
                        {
                            Nombre = lector.IsDBNull(IDXNombreProducto) ? "" : lector.GetString(IDXNombreProducto),
                            Categoria = lector.IsDBNull(IDXNombreCategoria) ? "" : lector.GetString(IDXNombreCategoria),
                            RutaImagen = lector.IsDBNull(IDXRutaImagen) ? "" : Path.GetFullPath(lector.GetString(IDXRutaImagen)),
                            Precio = lector.IsDBNull(IDXPrecio) ? 0 : lector.GetDecimal(IDXPrecio),
                            ProductoSKU = lector.GetString(IDXSKU)
                        };

                        registrosCoincidentes.Add(producto);
                    }

                    return registrosCoincidentes;
                }
            }
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
