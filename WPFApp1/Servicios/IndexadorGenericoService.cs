using System.Text.RegularExpressions;

namespace WPFApp1.Servicios
{
    public class IndexadorGenericoService
    {
        private static readonly char[] Delimitadores = new char[]
        {
            ' ', ',', '.', ';', ':', '(', ')', '[', ']', '{', '}', '-', '_',
            '/', '\\', '&', '*', '+', '=', '<', '>', '?', '!', '@', '#', '$',
            '%', '^', '~', '`', '"', '\''
        };
        private static readonly HashSet<string> ListaExclusion = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "EL", "LA", "LOS", "LAS", "UN", "UNA", "UNOS", "UNAS", "DE", "DEL", "Y", "O", "PARA", "CON", "EN", "POR", "SE"
        };
        public List<string> ObtenerPalabrasAtomicas(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                return new List<string>();
            }

            texto = texto.ToUpperInvariant();

            string[] palabrasBrutas = texto.Split(Delimitadores, StringSplitOptions.RemoveEmptyEntries);
            List<string> palabrasValidadas = new List<string>();

            foreach(string palabra in palabrasBrutas)
            {
                string palabraLimpia = Regex.Replace(palabra, @"[^A-Z0-9]", "");
                if (!string.IsNullOrWhiteSpace(palabraLimpia))
                {
                    palabrasValidadas.Add(palabraLimpia);
                }
            }

            return palabrasValidadas.Distinct().ToList();
        }
        public List<string> ObtenerPalabrasAtomicasFiltradas(string texto)
        {
            List<string> palabras = ObtenerPalabrasAtomicas(texto);
            List<string> palabrasFiltradas = new List<string>();

            foreach (string palabra in palabras)
            {
                if (!ListaExclusion.Contains(palabra))
                {
                    palabrasFiltradas.Add(palabra);
                }
            }

            return palabrasFiltradas;
        }
    }
}
