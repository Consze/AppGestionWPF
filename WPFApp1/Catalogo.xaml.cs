using System.ComponentModel;
using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : Window
    {
        private CatalogoViewModel _viewModel;
        public Catalogo(CatalogoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
        }
    }
}
