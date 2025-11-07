namespace WPFApp1.DTOS
{
    public class ResultadosBusquedaEAN
    {
        public List<ProductoCatalogo> Productos { get; set; }
        public bool HayVersionesObsoletas { get; set; }
    }
}
