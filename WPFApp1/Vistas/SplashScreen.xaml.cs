using System.ComponentModel;
using System.Windows;

namespace WPFApp1
{
 
    public partial class SplashScreen : Window, INotifyPropertyChanged
    {
        private bool _procesando = true;
        public bool Procesando
        {
            get { return _procesando; }
            set
            {
                if(_procesando != value)
                {
                    if (_procesando != value)
                    {
                        _procesando = value;
                        OnPropertyChanged(nameof(Procesando));
                    }
                }
            }
        }
        public SplashScreen()
        {
            DataContext = this;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
