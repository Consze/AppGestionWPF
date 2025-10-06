namespace WPFApp1.Entidades
{
    public class Medios_Pago : EntidadNombrada
    {
        private string _cuentaAsociadaID;
        public string CuentaAsociadaID
        {
            get => _cuentaAsociadaID;
            set
            {
                if (_cuentaAsociadaID != value)
                {
                    _cuentaAsociadaID = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
