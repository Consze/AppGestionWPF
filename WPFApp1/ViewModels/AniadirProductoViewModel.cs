using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFApp1.DTOS;
using WPFApp1.Factories;
using WPFApp1.Interfaces;
using WPFApp1.Servicios;
using WPFApp1.Mensajes;
using System.Data.Entity.Core.Metadata.Edm;

namespace WPFApp1.ViewModels
{
    public enum LadoMasLargo
    {
        Ancho,
        Alto
    }

    public class AbrirVistaAniadirProductoMensaje { }
    public class CerrarVistaAniadirProductoMensaje { }
    public class AniadirProductoViewModel : INotifyPropertyChanged
    {
        private readonly IProductosFactory _repositorioFactory;
        private readonly IProductoServicio _productoService;
        private readonly IndexadorProductoService _indexadorProductoService;
        public bool EsModoEdicion { get; set; }
        public string NombreDeVentana { get; set; }
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

        public AniadirProductoViewModel(IProductoServicio productoServicio, IndexadorProductoService indexadorProductoService)
        {
            //Imagen
            RutaImagenSeleccionada = string.Empty;
            AnchoImagenSeleccionada = 0;
            AltoImagenSeleccionada = 0;
            CalculoAlturaMarco = 0;
            CalculoAnchoMarco = 0;
            IDProducto = 0;

            //Entidad
            NombreProducto = string.Empty;
            CategoriaProducto = string.Empty;
            PrecioProducto = 0;

            NombreDeVentana = "Añadir Producto";
            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
            BotonPresionadoCommand = new RelayCommand<object>(BotonPresionado);

            //Repositorio de entidad
            _productoService = productoServicio;
            _indexadorProductoService = indexadorProductoService;
        }

        public void BotonPresionado(object parameter)
        {
            if(EsModoEdicion)
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
            EsModoEdicion = true;
            RutaImagenSeleccionada = Producto.RutaImagen;
            NombreProducto = Producto.Nombre;
            PrecioProducto= Producto.Precio;
            CategoriaProducto = Producto.Categoria;
            IDProducto = Producto.ID;
            NombreDeVentana = "Editar Producto";

            // Obtener dimensiones de imagen
            CargarDimensionesImagen(RutaImagenSeleccionada);
            LadoMasLargo lado = new LadoMasLargo();
            int UBound = 0;
            int LBound = 0;
            int TamanioMaximo = 200;

            // Calcular relación de aspecto del marco
            if (CalculoAlturaMarco > CalculoAnchoMarco)
            {
                UBound = CalculoAlturaMarco;
                LBound = CalculoAnchoMarco;
                lado = LadoMasLargo.Alto;
            }
            else
            {
                UBound = CalculoAnchoMarco;
                LBound = CalculoAlturaMarco;
                lado = LadoMasLargo.Ancho;
            }

            // Aplicar reducción
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = UBound / (float)LBound;
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
            if(RutaImagenSeleccionada != string.Empty)
            {
                //comprobar si la imagen elegida existe en la carpeta de miniaturas
                string NombreArchivo = Path.GetFileNameWithoutExtension(RutaImagenSeleccionada);
                string Extension = Path.GetExtension(RutaImagenSeleccionada);
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
                    RutaImagenSeleccionada = "./datos/miniaturas/" + NombreArchivo + Extension;
                }
            }

            //validar imagen
            Productos ProductoModificado = new Productos(IDProducto, NombreProducto, CategoriaProducto, PrecioProducto, RutaImagenSeleccionada);

            if (_productoService.ActualizarProducto(ProductoModificado))
            {
                _indexadorProductoService.IndexarProducto(ProductoModificado.Nombre, ProductoModificado.ID);
                Messenger.Default.Publish(new ProductoModificadoMensaje { ProductoModificado = ProductoModificado });
                CerrarVistaCommand.Execute(0);
                ServicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Item editado exitosamente", Titulo = "Operación Completada",IconoRuta  = Path.GetFullPath(IconoNotificacion.OK) ,Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                //System.Windows.MessageBox.Show("El producto fue editado.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                CerrarVistaCommand.Execute(0);
                ServicioSFX.Suspenso();
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo editar el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                //System.Windows.MessageBox.Show("Hubo un error al intentar editar el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            bool? resultado = openFileDialog.ShowDialog();

            if (resultado == true)
            {
                int UBound = 0;
                int LBound = 0;
                int TamanioMaximo = 200;
                string rutaArchivo = openFileDialog.FileName;
                RutaImagenSeleccionada = rutaArchivo;
                LadoMasLargo lado = new LadoMasLargo();
                
                // Calcular relación de aspecto del marco
                if (CalculoAlturaMarco > CalculoAnchoMarco)
                {
                    UBound = CalculoAlturaMarco;
                    LBound = CalculoAnchoMarco;
                    lado = LadoMasLargo.Alto;
                }
                else
                {
                    UBound = CalculoAnchoMarco;
                    LBound = CalculoAlturaMarco;
                    lado = LadoMasLargo.Ancho;
                }

                // Aplicar reducción
                if (UBound > TamanioMaximo)
                {
                    float _RelacionAspecto = UBound / (float)LBound;
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
            if (CalculoAlturaMarco > CalculoAnchoMarco)
            {
                UBound = CalculoAlturaMarco;
                LBound = CalculoAnchoMarco;
                lado = LadoMasLargo.Alto;
            }
            else
            {
                UBound = CalculoAnchoMarco;
                LBound = CalculoAlturaMarco;
                lado = LadoMasLargo.Ancho;
            }

            // Aplicar reducción
            if (UBound > TamanioMaximo)
            {
                float _RelacionAspecto = UBound / (float)LBound;
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
                string NombreArchivo = Path.GetFileNameWithoutExtension(RutaImagenSeleccionada);
                string Extension = Path.GetExtension(RutaImagenSeleccionada);
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
            int Resultado = _productoService.CrearProducto(_nuevoProducto);
            if (Resultado > -1 )
            {
                _nuevoProducto.ID = Convert.ToInt32(Resultado);
                _nuevoProducto.RutaImagen = Path.GetFullPath(_nuevoProducto.RutaImagen);
                _indexadorProductoService.IndexarProducto(_nuevoProducto.Nombre, _nuevoProducto.ID);
                Messenger.Default.Publish(new ProductoAniadidoMensaje { NuevoProducto = _nuevoProducto});
                CerrarVistaCommand.Execute(0);
                ServicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Item añadido con exito", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                //System.Windows.MessageBox.Show("El producto fue añadido.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ServicioSFX.Suspenso();
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo añadir el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
                //System.Windows.MessageBox.Show("Hubo un error al intentar añadir el producto.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    CalculoAnchoMarco = bitmapImage.PixelWidth;
                    CalculoAlturaMarco = bitmapImage.PixelHeight;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
                    CalculoAnchoMarco = 0;
                    CalculoAlturaMarco = 0;
                }
            }
            else
            {
                CalculoAnchoMarco = 0;
                CalculoAlturaMarco = 0;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
