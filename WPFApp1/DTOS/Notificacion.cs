namespace WPFApp1.DTOS
{
    public enum MatrizEisenhower
    {
        C1 = 1,
        C2 = 2,
        C3 = 3
    }
    public class Notificacion
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public MatrizEisenhower Urgencia { get; set; }
    }
}
