namespace WPFApp1.Entidades
{
    public class EntidadBase
    {
        public string Nombre { get; set; }
        public bool EsEliminado { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ID { get; set; }
    }
}
