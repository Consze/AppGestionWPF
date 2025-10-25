namespace WPFApp1.DTOS
{
    public record ProductoEditar_Propiedad_Valor
    {
        public ProductoCatalogo ProductoEditar { get; init; }
        public string PropiedadNombre { get; init; }
        public object Valor { get; init; }
    }
}
