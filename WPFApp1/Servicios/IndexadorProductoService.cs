using System.Text.RegularExpressions;
using WPFApp1.DTOS;
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
        public List<CoincidenciasBusqueda> RecuperarRegistros(string TerminoBusqueda)
        {
            Dictionary<int, CoincidenciasBusqueda> resultadosPorProductoId = new Dictionary<int, CoincidenciasBusqueda>();
            List<string> TituloPalabras = IndexadorProductoService.ObtenerPalabrasAtomicasFiltradas(TerminoBusqueda);

            foreach (string palabra in TituloPalabras) // 1 - Buscar cada palabra del titulo
            {
                List<PalabrasTitulosProductos> palabraCoincidencias = _repositorio.BuscarPalabra(palabra); // 2 - Recuperar los registros donde se encuentre la palabra
                foreach (PalabrasTitulosProductos coincidencia in palabraCoincidencias)
                {
                    if (resultadosPorProductoId.TryGetValue(coincidencia.producto_id, out CoincidenciasBusqueda productoEncontrado))
                    {
                        productoEncontrado.CantidadPalabrasCoincidentes++;
                    }
                    else
                    {
                        resultadosPorProductoId.Add(coincidencia.producto_id, new CoincidenciasBusqueda
                        {
                            producto_id = coincidencia.producto_id,
                            palabra = coincidencia.palabra,
                            CantidadPalabrasCoincidentes = 1
                        });
                    }
                }
            }

            return resultadosPorProductoId.Values.OrderByDescending(c => c.CantidadPalabrasCoincidentes).ToList();
        }
        public List<Productos> RecuperarProductos(List<CoincidenciasBusqueda> CoincidenciasProductos)
        {
            List<Productos> ProductosCoincidentes = new List<Productos>();

            foreach (CoincidenciasBusqueda Coincidencia in CoincidenciasProductos)
            {
                Productos registro = ProductosRepository.RecuperarRegistro(Coincidencia.producto_id);
                if (registro != null && registro.ID != 0)
                {
                    ProductosCoincidentes.Add(registro);
                }
            }

            return ProductosCoincidentes;
        }
        public List<Productos> BuscarTituloProductos(string TituloBuscado)
        {
            List<CoincidenciasBusqueda> coincidencias = RecuperarRegistros(TituloBuscado);
            List<Productos> ColeccionProductosCoincidentes = RecuperarProductos(coincidencias);
            return ColeccionProductosCoincidentes;
        }
    }
}
