namespace WPFApp1.DTOS
{
    public record ProductoSKU_Propiedad_Valor
    {
        public string ProductoSKU { get; init; }
        public string PropiedadNombre { get; init; }
        public object Valor { get; init; }
    }
}
