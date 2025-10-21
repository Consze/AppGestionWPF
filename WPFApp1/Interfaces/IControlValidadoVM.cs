namespace WPFApp1.Interfaces
{
    public interface IControlValidadoVM : IDisposable
    {
        bool ValidarInput { get; }
        object InputUsuario { get; }
    }
}
