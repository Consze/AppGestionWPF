namespace WPFApp1.Entidades
{
    public class Factura_pagos : EntidadBase
    {
        private string _facturaID;
        public string FacturaID
        {
            get => _facturaID;
            set
            {
                if (_facturaID != value)
                {
                    _facturaID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _medioPagoID;
        public string MedioPagoID
        {
            get => _medioPagoID;
            set
            {
                if (_medioPagoID != value)
                {
                    _medioPagoID = value;
                    OnPropertyChanged();
                }
            }
        }
        private decimal _monto;
        public decimal Monto
        {
            get => _monto;
            set
            {
                if (_monto != value)
                {
                    _monto = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
