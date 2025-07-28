using System.Media;
using System.Runtime.CompilerServices;

namespace WPFApp1.Servicios
{
    public class ServicioSFX
    {
        public static void Confirmar()
        {
            ReproducirEfecto("confirmacion1.wav");
        }
        public static void Suspenso()
        {
            ReproducirEfecto("suspenso1.wav");
        }
        public static void Swipe()
        {
            ReproducirEfecto("swipe1.wav");
        }
        public static void Shuffle()
        {
            ReproducirEfecto("shuffle.wav");
        }
        public static void Shutter()
        {
            ReproducirEfecto("shutter.wav");
        }
        private static void ReproducirEfecto(string NombreArchivo)
        {
            string rutaSonido = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", NombreArchivo);
            if (System.IO.File.Exists(rutaSonido))
            {
                try
                {
                    SoundPlayer player = new SoundPlayer(rutaSonido);
                    player.Load();
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al reproducir sonido: {ex.Message}");
                }
            }
        }
    }
}
