using System.Windows;

namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para Catalogo.xaml
    /// </summary>
    public partial class Catalogo : Window
    {
        private CatalogoViewModel _viewModel;
        public static Catalogo VentanaCatalogoVigente { get;private set; }
        public static int Instancias{get;set;}
        public Catalogo(CatalogoViewModel viewModel)
        {
            Instancias++;
            DataContext = viewModel;
            _viewModel = viewModel;
            VentanaCatalogoVigente = this;
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Instancias--;
        }
    }
}
