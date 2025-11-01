using System.Windows;
using WPFApp1.ViewModels;

namespace WPFApp1
{
    public partial class MainWindow : Window
    {
        public bool PermitirCierrre { get; set; }
        public MainWindow(MainWindowViewModel viewModel)
        {
            PermitirCierrre = false;
            DataContext = viewModel;
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