using System.Windows;

namespace WPFApp1
{
 
    public partial class SplashScreen : Window
    {
        public bool Procesando { get; set; }
        public SplashScreen()
        {
            DataContext = this;
            this.Procesando = true;
            InitializeComponent();
        }
    }
}
