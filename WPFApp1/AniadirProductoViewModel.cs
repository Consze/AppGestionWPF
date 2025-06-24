using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;

namespace WPFApp1
{
    public enum LadoMasLargo
    {
        Ancho,
        Alto
    }
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        private int CalculoAlturaMarco;
        private int CalculoAnchoMarco;

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

        private string _nombreProducto;
        public string NombreProducto
        {
            get { return _nombreProducto; }
            set
            {
                if (_nombreProducto != value)
                {
                    _nombreProducto = value;
                    OnPropertyChanged(nameof(NombreProducto));
                }
            }
        }
        private string _categoriaProducto;
        public string CategoriaProducto
        {
            get { return _categoriaProducto; }
            set
            {
                if (_categoriaProducto != value)
                {
                    _categoriaProducto = value;
                    OnPropertyChanged(nameof(CategoriaProducto));
                }
            }
        }
        private int _precioProducto;
        public int PrecioProducto
        {
            get { return _precioProducto; }
            set
            {
                if (_precioProducto != value)
                {
                    _precioProducto = value;
                    OnPropertyChanged(nameof(PrecioProducto));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CierreSolicitado;
        public ICommand ElegirImagenCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand CerrarVistaCommand { get; }

        public AniadirProductoViewModel()
        {
            //Imagen
            this.RutaImagenSeleccionada = string.Empty;
            this.AnchoImagenSeleccionada = 0;
            this.AltoImagenSeleccionada = 0;
            this.CalculoAlturaMarco = 0;
            this.CalculoAnchoMarco = 0;

            //Entidad
            this.NombreProducto = string.Empty;
            this.CategoriaProducto = string.Empty;
            this.PrecioProducto = 0;

            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
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

        public void AniadirProducto(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NombreProducto) || string.IsNullOrWhiteSpace(CategoriaProducto) || PrecioProducto == 0)
            {
                System.Windows.MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string RutaImagenSalida = string.Empty;
            if (RutaImagenSeleccionada != string.Empty)
            { 
                string NombreArchivo = System.IO.Path.GetFileNameWithoutExtension(RutaImagenSeleccionada);
                string Extension = System.IO.Path.GetExtension(RutaImagenSeleccionada);
                string Destino = ".\\datos\\miniaturas\\" + NombreArchivo + Extension;
                bool SalirDelBucle = false;
                int NumeroIntento = 0;

                try
                {
                    File.Copy(RutaImagenSeleccionada, Destino);
                    RutaImagenSalida = "./datos/miniaturas/" + NombreArchivo + Extension;
                    RutaImagenSalida = System.IO.Path.GetFullPath(RutaImagenSalida);
                    SalirDelBucle = true;
                }
                catch(Exception ex)
                {
                    while (!SalirDelBucle)
                    {
                        if (File.Exists(Destino) && NumeroIntento < 100)
                        { 
                            string DestinoPrueba = ".\\datos\\miniaturas\\" + NombreArchivo + NumeroIntento + Extension;
                            try
                            { 
                                File.Copy(RutaImagenSeleccionada, DestinoPrueba); 
                            }
                            catch (Exception ex2)
                            { 
                                if(File.Exists(DestinoPrueba))
                                {
                                    SalirDelBucle = true;
                                    RutaImagenSalida = "./datos/miniaturas/" + NombreArchivo + NumeroIntento + Extension;
                                    RutaImagenSalida = System.IO.Path.GetFullPath(RutaImagenSalida);
                                }
                                else
                                {
                                    NumeroIntento += 1;
                                }
                            }
                        }
                        else
                        {
                            SalirDelBucle = true;
                            RutaImagenSalida = string.Empty;
                        }
                    }
                }
            }

            Productos _nuevoProducto = new Productos(NombreProducto,CategoriaProducto,PrecioProducto, RutaImagenSalida);
            if (ProductosRepository.AniadirNuevoProducto(_nuevoProducto))
            {
                System.Windows.MessageBox.Show("El producto fue añadido.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Hubo un error al intentar añadir el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void CerrarVista(object parameter)
        {
            CierreSolicitado?.Invoke(this, EventArgs.Empty);
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
