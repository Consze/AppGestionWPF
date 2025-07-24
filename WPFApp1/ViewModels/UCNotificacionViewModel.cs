using System.ComponentModel;

namespace WPFApp1.ViewModels
{
    public class UCNotificacionViewModel : INotifyPropertyChanged
    {
        private string _titulo;
        public string Titulo
        {
            get { return _titulo; }
            set
            {
                if (_titulo != value)
                {
                    _titulo = value;
                    OnPropertyChanged(nameof(Titulo));
                }
            }
        }
        private string _cuerpo;
        public string Cuerpo
        {
            get { return _cuerpo; }
            set
            {
                if (_cuerpo != value)
                {
                    _cuerpo = value;
                    OnPropertyChanged(nameof(Cuerpo));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public UCNotificacionViewModel()
        {
            this._titulo = null;
            this._cuerpo = null;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
