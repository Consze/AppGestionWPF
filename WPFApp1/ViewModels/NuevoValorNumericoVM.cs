using System.ComponentModel;
using WPFApp1.Interfaces;
using WPFApp1.Servicios;

public class NuevoValorNumericoViewModel : INotifyPropertyChanged, IControlValidadoVM
{
    private string _valorString;
    private readonly string nombrePropiedad;
    public event PropertyChangedEventHandler PropertyChanged;
    public string ValorString
    {
        get => _valorString;
        set
        {
            if (_valorString != value)
            {
                _valorString = value;
                OnPropertyChanged(nameof(ValidarInput));
            }
        }
    }
    public bool ValidarInput
    {
        get
        {
            if (!decimal.TryParse(ValorString, out decimal valorNumerico))
                return false;

            if (nombrePropiedad.Equals("Haber", StringComparison.OrdinalIgnoreCase))
            {
                return valorNumerico > 0;
            }

            return valorNumerico >= 0;
        }
    }
    public object InputUsuario
    {
        get
        {
            if (ValidarInput && decimal.TryParse(ValorString, out decimal valorNumerico))
            {
                return valorNumerico;
            }
            return null;
        }
    }
    public NuevoValorNumericoViewModel(string _nombrePropiedad)
    {
        nombrePropiedad = _nombrePropiedad;
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}