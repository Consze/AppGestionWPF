using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace WPFApp1
{
    /// <summary>
    /// Lógica de interacción para ExportarProductos.xaml
    /// </summary>
    public partial class ExportarProductos : Window
    {
        private ExportarProductosViewModel _viewModel;
        public static ExportarProductos VentanaExportarProductosVigente { get; private set; }
        public static int Instancias { get; set; }
        public ExportarProductos(ExportarProductosViewModel ViewModel)
        {
            Instancias++;
            DataContext = ViewModel;
            _viewModel = ViewModel;
            VentanaExportarProductosVigente = this;
            InitializeComponent();
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
     
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.ExportacionEnProceso))
            {
                if (!_viewModel.ExportacionEnProceso)
                {
                    if (Throbber.Content is Path throbberPath)  
                    {
                        Storyboard cambioColorStoryboard = (Storyboard)this.FindResource("AnimacionCambioColorThrobber");
                        if (cambioColorStoryboard != null)
                        {
                            cambioColorStoryboard.Stop(throbberPath);
                        }
                    }
                }
                else
                {
                    if (Throbber.Content is Ellipse ellipse)
                    {
                        Storyboard storyboard = new Storyboard();
                        ColorAnimation colorAnimation = new ColorAnimation();

                        colorAnimation.From = Colors.Gray;
                        colorAnimation.To = Colors.Red;
                        colorAnimation.Duration = TimeSpan.FromSeconds(1);
                        colorAnimation.RepeatBehavior = RepeatBehavior.Forever;

                        Storyboard.SetTargetName(colorAnimation, "ThrobberFillBrush");
                        Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("Color"));

                        storyboard.Children.Add(colorAnimation);
                        storyboard.Begin(Throbber);
                    }
                }
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Instancias--;
        }
    }
}
