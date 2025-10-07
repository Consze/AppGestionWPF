namespace WPFApp1.Entidades
{
    public class Factura : EntidadBase
    {
        private string _sucursalID;
        public string SucursalID
        {
            get => _sucursalID;
            set
            {
                if (_sucursalID != value)
                {
                    _sucursalID = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
