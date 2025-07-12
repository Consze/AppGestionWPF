using System.Data.SqlClient;

namespace WPFApp1
{
    public interface IDatosAccesoServicio
    {
        void ActualizarProducto(Productos producto);
        Productos ObtenerProductoPorId(int id);
    }
    public class SQLiteAccesoDatos : IDatosAccesoServicio
    {
        private readonly string _conexionCadena = ".\\datos\\base.db";
    }

    public static class SQLServerPruebaConexion
    {
        public static bool ProbarConexion(string cadenaConexion)
        {
            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                try
                {
                    conexion.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }
    }
}
