using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using NPOI.SS.Formula.Functions;

namespace WPFApp1
{
    public enum LadoMasLargo
    {
        Ancho,
        Alto
    }
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        public bool EsModoEdicion { get; set; }  
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
        public int IDProducto { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CierreSolicitado;
        public ICommand ElegirImagenCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand BotonPresionadoCommand { get; }
        public ICommand CerrarVistaCommand { get; }

        public AniadirProductoViewModel()
        {
            //Imagen
            this.RutaImagenSeleccionada = string.Empty;
            this.AnchoImagenSeleccionada = 0;
            this.AltoImagenSeleccionada = 0;
            this.CalculoAlturaMarco = 0;
            this.CalculoAnchoMarco = 0;
            this.IDProducto = 0;

            //Entidad
            this.NombreProducto = string.Empty;
            this.CategoriaProducto = string.Empty;
            this.PrecioProducto = 0;

            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
            BotonPresionadoCommand = new RelayCommand<object>(BotonPresionado);
        }

        public void BotonPresionado(object parameter)
        {
            if(this.EsModoEdicion)
            {
                EditarProducto(0);
            }
            else
            {
                AniadirProducto(0);
            }
        }

        public void ConfigurarEdicionDeProducto(Productos Producto)
        {
            // Configurar Bindings
            this.EsModoEdicion = true;
            this.RutaImagenSeleccionada = Producto.RutaImagen;
            this.NombreProducto = Producto.Nombre;
            this.PrecioProducto= Producto.Precio;
            this.CategoriaProducto = Producto.Categoria;
            this.IDProducto = Producto.ID;
            CargarDimensionesImagen(this.RutaImagenSeleccionada);
            LadoMasLargo lado = new LadoMasLargo();
            int UBound = 0;
            int LBound = 0;
            int TamanioMaximo = 200;
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
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = (float)UBound / (float)LBound;
                switch (lado)
                {
                    case LadoMasLargo.Alto:
                        AltoImagenSeleccionada = TamanioMaximo;
                        AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                    case LadoMasLargo.Ancho:
                        AnchoImagenSeleccionada = TamanioMaximo;
                        AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                }
            }
        }
        public void EditarProducto(object parameter)
        {
            if(this.RutaImagenSeleccionada != string.Empty)
            {
                //comprobar si la imagen elegida existe en la carpeta de miniaturas
                string NombreArchivo = System.IO.Path.GetFileNameWithoutExtension(this.RutaImagenSeleccionada);
                string Extension = System.IO.Path.GetExtension(RutaImagenSeleccionada);
                string Destino = ".\\datos\\miniaturas\\" + NombreArchivo + Extension;
                bool SalirDelBucle = false;
                if (!File.Exists(Destino))
                {
                    try
                    {
                        File.Copy(RutaImagenSeleccionada, Destino, false);
                        RutaImagenSeleccionada = "./datos/miniaturas/" + NombreArchivo + Extension;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    this.RutaImagenSeleccionada = "./datos/miniaturas/" + NombreArchivo + Extension;
                }
            }

            //validar imagen
            Productos ProductoModificado = new Productos(this.IDProducto, this.NombreProducto, this.CategoriaProducto, this.PrecioProducto, this.RutaImagenSeleccionada);

            if (ProductosRepository.ModificarProducto(ProductoModificado))
            {
                CerrarVistaCommand.Execute(0);
                System.Windows.MessageBox.Show("El producto fue editado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CerrarVistaCommand.Execute(0);
                System.Windows.MessageBox.Show("Hubo un error al intentar editar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                int TamanioMaximo = 200;
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
                if (UBound > TamanioMaximo)
                {
                    float _RelacionAspecto = (float)UBound / (float)LBound;
                    switch (lado)
                    {
                        case LadoMasLargo.Alto:
                            AltoImagenSeleccionada = TamanioMaximo;
                            AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                            break;
                        case LadoMasLargo.Ancho:
                            AnchoImagenSeleccionada = TamanioMaximo;
                            AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                            break;
                    }
                }
            }
        }
        public void RedimensionarImagen(object parameter)
        {
            int UBound = 0;
            int LBound = 0;
            int TamanioMaximo = 200;
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
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = (float)UBound / (float)LBound;
                switch (lado)
                {
                    case LadoMasLargo.Alto:
                        AltoImagenSeleccionada = TamanioMaximo;
                        AnchoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
                    case LadoMasLargo.Ancho:
                        AnchoImagenSeleccionada = TamanioMaximo;
                        AltoImagenSeleccionada = Convert.ToInt32(TamanioMaximo / _RelacionAspecto);
                        break;
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
                    File.Copy(RutaImagenSeleccionada, Destino,false);
                    RutaImagenSalida = "./datos/miniaturas/" + NombreArchivo + Extension;
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

            Productos _nuevoProducto = new Productos(0,NombreProducto,CategoriaProducto,PrecioProducto, RutaImagenSalida);
            if (ProductosRepository.AniadirNuevoProducto(_nuevoProducto))
            {
                _nuevoProducto.RutaImagen = System.IO.Path.GetFullPath(_nuevoProducto.RutaImagen); 
                Messenger.Default.Publish(new ProductoAniadidoMensaje { NuevoProducto = _nuevoProducto});
                CerrarVistaCommand.Execute(0);
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
