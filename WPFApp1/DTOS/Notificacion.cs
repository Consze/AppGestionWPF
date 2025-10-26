namespace WPFApp1.DTOS
{
    public enum MatrizEisenhower
    {
        C1 = 1,
        C2 = 2,
        C3 = 3
    }

    public static class IconoNotificacion
    {
        public static string OK = @"./iconos/check.png";
        public static string NOTIFICACION = @"./iconos/notificaciones.png";
        public static string SUSPENSO1 = @"./iconos/suspenso1.png";
    }
    public class Notificacion
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string IconoRuta { get; set; }
        public MatrizEisenhower Urgencia { get; set; }
    }
}
