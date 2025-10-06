namespace WPFApp1.Entidades
{
    public class Cuentas_Contables : EntidadNombrada
    {
        private string _tipoCuenta;
        public string TipoCuenta
        {
            get => _tipoCuenta;
            set
            {
                if (_tipoCuenta != value)
                {
                    _tipoCuenta = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
