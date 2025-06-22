namespace WPFApp1
{
    public class Persona
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int altura { get; set; }
        public int peso { get; set; }

        public Persona(int id, string nombre, int altura, int peso)
        {
            this.id = id;
            this.nombre = nombre;
            this.altura = altura;
            this.peso = peso;
        }
    }
    public class FlagsCambios
    {
        public bool idCambiado { get; set; } = false;
        public bool nombreCambiado { get; set; } = false;
        public bool alturaCambiada { get; set; } = false;
        public bool pesoCambiado { get; set; } = false;
        public int Count { get; set; } = 0;

        public FlagsCambios()
        {
        }
    }
}
