using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WPFApp1
{
    public enum LadoMasLargo
    {
        Ancho,
        Alto
    }
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int CalculoAlturaMarco { get; set; }
        private int CalculoAnchoMarco { get; set; }

        private string _rutaImagenSeleccionada;
        public string RutaImagenSeleccionada 
        {
            get { return _rutaImagenSeleccionada; }
            set
            {
                if (_rutaImagenSeleccionada != value)
                {
                    _rutaImagenSeleccionada = value;
                    OnPropertyChanged(nameof(RutaImagenSeleccionada));
                    CargarDimensionesImagen(value);
                }
            }
        }

        private int _altoImagenSeleccionada;
        public int AltoImagenSeleccionada 
        {
            get { return _altoImagenSeleccionada; }
            set
            {
                if (_altoImagenSeleccionada != value)
                {
                    _altoImagenSeleccionada = value;
                    OnPropertyChanged(nameof(AltoImagenSeleccionada));
                }
            }
        }

        private int _anchoImagenSeleccionada;
        public int AnchoImagenSeleccionada
        {
            get { return _anchoImagenSeleccionada; }
            set
            {
                if (_anchoImagenSeleccionada != value)
                {
                    _anchoImagenSeleccionada = value;
                    OnPropertyChanged(nameof(AnchoImagenSeleccionada));
                }
            }
        }

        public ICommand ElegirImagenCommand{ get; }

        public AniadirProductoViewModel()
        {
            RutaImagenSeleccionada = string.Empty;
            AnchoImagenSeleccionada = 0;
            AltoImagenSeleccionada = 0;
            CalculoAlturaMarco = 0;
            CalculoAnchoMarco = 0;
            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
        }

        public void ElegirImagen(object parameter)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Todos los archivos (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Title = "Seleccionar imagen";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            Nullable<bool> resultado = openFileDialog.ShowDialog();

            if (resultado == true)
            {
                int UBound = 0;
                int LBound = 0;
                string rutaArchivo = openFileDialog.FileName;
                RutaImagenSeleccionada = rutaArchivo;
                LadoMasLargo lado = new LadoMasLargo();
                
                // Calcular relación de aspecto del marco
                if (this.CalculoAlturaMarco > this.CalculoAnchoMarco)
                {
                    UBound = this.CalculoAlturaMarco;
                    LBound = this.CalculoAnchoMarco;
                    lado = LadoMasLargo.Alto;
                }
                else
                {
                    UBound = this.CalculoAnchoMarco;
                    LBound = this.CalculoAlturaMarco;
                    lado = LadoMasLargo.Ancho;
                }

                // Aplicar reducción
                if (UBound > 300)
                {
                    float _RelacionAspecto = (float)UBound / (float)LBound;
                    switch (lado)
                    {
                        case LadoMasLargo.Alto:
                            AltoImagenSeleccionada = 300;
                            AnchoImagenSeleccionada = Convert.ToInt32(300 / _RelacionAspecto);
                            break;
                        case LadoMasLargo.Ancho:
                            AnchoImagenSeleccionada = 300;
                            AltoImagenSeleccionada = Convert.ToInt32(300 / _RelacionAspecto);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene y almacena las dimensiones de una imagen seleccionada. Se utilizan para reducir la imagen sin alterar su relación de aspecto.
        /// </summary>
        /// <param name="rutaArchivo">La ruta a una imagen elegida por el usuario</param>
        private void CargarDimensionesImagen(string rutaArchivo)
        {
            if (!string.IsNullOrEmpty(rutaArchivo))
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(rutaArchivo);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    this.CalculoAnchoMarco = bitmapImage.PixelWidth;
                    this.CalculoAlturaMarco = bitmapImage.PixelHeight;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    this.CalculoAnchoMarco = 0;
                    this.CalculoAlturaMarco = 0;
                }
            }
            else
            {
                this.CalculoAnchoMarco = 0;
                this.CalculoAlturaMarco = 0;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
