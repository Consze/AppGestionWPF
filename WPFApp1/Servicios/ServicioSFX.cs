using System.IO;
using System.Windows.Media;

namespace WPFApp1.Servicios
{
    public class ServicioSFX
    {   
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        public double VolumenGeneral { get; set; } = 0.3;
        public void Confirmar()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "confirmacion1.wav");
            ReproducirSonido(rutaSonido, 0.2); 
        }
        public void Suspenso()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "suspenso1.wav");
            ReproducirSonido(rutaSonido, 0.15);
        }
        public void Swipe()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "swipe1.wav");
            ReproducirSonido(rutaSonido, 1.0);
        }
        public void Shuffle()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "shuffle.wav");
            ReproducirSonido(rutaSonido, 0.1);
        }
        public void Shutter()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "shutter.wav");
            ReproducirSonido(rutaSonido, 0.10);
        }
        public void Paginacion()
        {
            string rutaSonido = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SFX", "turning-page1.wav");
            ReproducirSonido(rutaSonido, 0.4);
        }
        public ServicioSFX()
        {
            _mediaPlayer.MediaEnded += (sender, e) =>
            {
                _mediaPlayer.Close();
            };
        }
        public void ReproducirSonido(string rutaArchivo, double volumen = -1)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
            {
                Console.WriteLine($"Error: {rutaArchivo}");
                return;
            }

            try
            {
                _mediaPlayer.Open(new Uri(rutaArchivo, UriKind.RelativeOrAbsolute));
                _mediaPlayer.Volume = (volumen >= 0 && volumen <= 1) ? volumen : VolumenGeneral;
                _mediaPlayer.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reproducir sonido '{rutaArchivo}': {ex.Message}");
            }
        }
    }
}
