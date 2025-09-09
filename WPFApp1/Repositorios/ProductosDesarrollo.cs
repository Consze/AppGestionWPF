using Microsoft.Data.Sqlite;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;

namespace WPFApp1.Repositorios
{
    public class ProductosDesarrollo
    {
        private readonly ConexionDBSQLite _accesoDB;
        public ProductosDesarrollo(ConexionDBSQLite accesoDB)
        {
            _accesoDB = accesoDB;
        }
        public ProductoCatalogo RecuperarProductoPorID(string ProductoID)
        {
            ProductoCatalogo registro = new ProductoCatalogo();
            string Consulta = @"
            SELECT 
                s.SKU_Producto,
                s.ubicacion_id,
                s.producto_version_id,
                s.haber,
                s.precio,
                s.VisibilidadWeb,
                s.PrecioPublico,
                s.FechaModificacion,
                s.FechaCreacion,
                s.EsEliminado,
                v.RutaRelativaImagen,
                v.EAN,
                v.producto_id,
                v.Marca_id,
                v.formato_id,
                f.alto AS Altura,
                f.ancho,
                f.largo,
                f.peso,
                p.nombre,
                p.categoria 
            FROM Productos_stock AS s
            INNER JOIN Productos_versiones AS v ON s.producto_version_id = v.id
            INNER JOIN Productos_Formatos AS f ON v.formato_id = f.id
            INNER JOIN Productos AS p ON v.producto_id = p.id
            WHERE s.SKU_Producto = @IdBuscada
                AND s.haber > 0
                AND s.EsEliminado = 0;";
            
            try
            {
                using (SqliteCommand comando = new SqliteCommand(Consulta, _accesoDB.ObtenerConexionDB()))
                {
                    comando.Parameters.AddWithValue("@IdBuscada",ProductoID);
                    using(SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            registro.Altura = Convert.ToInt32(lector["Altura"]);
                        }
                    }
                }
            }
            catch(SqliteException ex)
            {

            }
            return registro;
        }
    }
}
