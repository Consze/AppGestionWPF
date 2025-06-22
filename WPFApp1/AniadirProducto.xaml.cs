using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para AniadirProducto.xaml
    /// </summary>
    public partial class AniadirProducto : Window
    {
        public AniadirProducto(AniadirProductoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
