using System.IO;
using System.Text.Json;

namespace WPFApp1
{
    public class ConfiguracionCatalogo
    {
        public VistaElegida UltimaVista { get; set; }
    }

    public class PersistenciaConfiguracion
    {
        private static string _rutaArchivoConfiguracion = ".\\configuracion.json";

        public static bool GuardarUltimaVista(VistaElegida Vista)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaArchivoConfiguracion));
            var configuracion = new ConfiguracionCatalogo { UltimaVista = Vista };
            string jsonString = JsonSerializer.Serialize(configuracion);
            try
            {
                File.WriteAllText(_rutaArchivoConfiguracion, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la configuración del catálogo: {ex.Message}");
                return false;
            }
        }

        public static VistaElegida LeerUltimaVista()
        {
            try
            {
                if(File.Exists(_rutaArchivoConfiguracion))
                {
                    string jsonString = File.ReadAllText(_rutaArchivoConfiguracion);
                    var configuracion = JsonSerializer.Deserialize<ConfiguracionCatalogo>(jsonString);
                    return configuracion?.UltimaVista ?? VistaElegida.Ninguna;
                }
                else
                {
                    return VistaElegida.Ninguna;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                return VistaElegida.Ninguna;
            }
        }
    }
}
