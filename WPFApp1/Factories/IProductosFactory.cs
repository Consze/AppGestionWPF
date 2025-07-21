using WPFApp1.Interfaces;

namespace WPFApp1.Factories
{
    public interface IProductosFactory
    {
        IProductosAccesoDatos CrearRepositorio();
    }
}
