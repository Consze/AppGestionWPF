namespace WPFApp1.DTOS
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
        public bool idCambiado { get; set; }
        public bool nombreCambiado { get; set; } 
        public bool alturaCambiada { get; set; } 
        public bool pesoCambiado { get; set; } 
        public int Count { get; set; }

        public FlagsCambios()
        {
            idCambiado = false;
            nombreCambiado = false;
            alturaCambiada = false;
            pesoCambiado = false;
            Count = 0;
        }
    }
}
