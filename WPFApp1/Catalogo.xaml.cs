using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : Window
    {
        public Catalogo(CatalogoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
