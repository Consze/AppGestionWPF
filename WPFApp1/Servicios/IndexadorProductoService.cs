using System.Text.RegularExpressions;
using WPFApp1.Repositorios;

namespace WPFApp1.Servicios
{
    public class IndexadorProductoService
    {
        private readonly IndexadorProductosRepositorio _repositorio;

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
        public IndexadorProductoService(IndexadorProductosRepositorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public static List<string> ObtenerPalabrasAtomicas(string texto)
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
        public static List<string> ObtenerPalabrasAtomicasFiltradas(string texto)
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
        public void IndexarProducto(string titulo, int ID)
        {
            if(!string.IsNullOrWhiteSpace(titulo))
            {
                List<string> TituloPalabras = ObtenerPalabrasAtomicasFiltradas(titulo);
                
                foreach(string palabra in TituloPalabras)
                {
                    _repositorio.InsertarRegistro(palabra, ID);
                }
            }
        }
    }
}
