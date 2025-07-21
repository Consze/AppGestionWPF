using System.Windows;
using WPFApp1.ViewModels;

namespace WPFApp1
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public bool PermitirCierrre { get; set; }
        public MainWindow()
        {
            PermitirCierrre = false;
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            this.Closing += Window_Closing;
            InitializeComponent();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!PermitirCierrre)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}