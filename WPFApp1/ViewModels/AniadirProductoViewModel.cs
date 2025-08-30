using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFApp1.DTOS;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Mensajes;
using WPFApp1.Servicios;

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
        private readonly IProductoServicio _productoService;
        private readonly ServicioIndexacionProductos _servicioIndexacion;
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
        private bool _itemConCategoria;
        public bool ItemConCategoria
        {
            get { return _itemConCategoria; }
            set
            {
                if (_itemConCategoria != value)
                {
                    _itemConCategoria = value;
                    OnPropertyChanged(nameof(ItemConCategoria));
                }
            }
        }
        public string IDProducto { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CierreSolicitado;
        private ServicioSFX _servicioSFX { get; set; }
        public ICommand ElegirImagenCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand BotonPresionadoCommand { get; }
        public ICommand CerrarVistaCommand { get; }

        public AniadirProductoViewModel(IProductoServicio productoServicio, ServicioIndexacionProductos ServicioIndexacion)
        {
            //Imagen
            RutaImagenSeleccionada = string.Empty;
            AnchoImagenSeleccionada = 0;
            AltoImagenSeleccionada = 0;
            CalculoAlturaMarco = 0;
            CalculoAnchoMarco = 0;
            IDProducto = null;

            //Entidad
            NombreProducto = string.Empty;
            CategoriaProducto = string.Empty;
            PrecioProducto = 0;
            ItemConCategoria = true;

            NombreDeVentana = "Añadir Producto";
            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
            BotonPresionadoCommand = new RelayCommand<object>(BotonPresionado);

            //Servicios
            _productoService = productoServicio;
            _servicioSFX = new ServicioSFX();
            _servicioIndexacion = ServicioIndexacion;
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
        public void ConfigurarEdicionDeProducto(ProductoBase Producto)
        {
            // Configurar Bindings
            EsModoEdicion = true;
            RutaImagenSeleccionada = Producto.RutaImagen;
            NombreProducto = Producto.Nombre;
            PrecioProducto= Producto.Precio;
            CategoriaProducto = Producto.Categoria;
            IDProducto = Producto.ID;
            NombreDeVentana = "Editar Producto";

            //
            if (Producto is ProductoBase _producto)
            {
                ItemConCategoria = true;
            }
            else
            {
                ItemConCategoria = false;
            }

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

            // Solicitar cambio a servicio de Productos y mostrar resultado
            Productos ProductoModificado = new Productos(IDProducto, NombreProducto, CategoriaProducto, PrecioProducto, RutaImagenSeleccionada);
            ProductoCatalogo ProductoMensaje = new ProductoCatalogo
            { ID = ProductoModificado.ID,
                Nombre = ProductoModificado.Nombre,
                Categoria = ProductoModificado.Categoria,
                Precio = ProductoModificado.Precio,
                RutaImagen = ProductoModificado.RutaImagen
            };
            if(string.IsNullOrWhiteSpace(ProductoModificado.Categoria))
            {
                ProductoMensaje.MostrarCategoria = false;
            }
            else
            {
                ProductoMensaje.MostrarCategoria = true;
            }

            if (_productoService.ActualizarProducto(ProductoModificado))
            {
                _servicioIndexacion.IndexarProducto(ProductoModificado);
                ProductoMensaje.RutaImagen = string.IsNullOrWhiteSpace(ProductoMensaje.RutaImagen) ? string.Empty : Path.GetFullPath(ProductoMensaje.RutaImagen);
                Messenger.Default.Publish(new ProductoModificadoMensaje { ProductoModificado = ProductoMensaje });
                CerrarVistaCommand.Execute(0);
                _servicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Item editado exitosamente", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                CerrarVistaCommand.Execute(0);
                _servicioSFX.Suspenso();
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo editar el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
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

            Productos _nuevoProducto = new Productos(null,NombreProducto,CategoriaProducto,PrecioProducto, RutaImagenSalida);
            ProductoCatalogo ProductoMensaje = new ProductoCatalogo
            {
                ID = null,
                Nombre = _nuevoProducto.Nombre,
                Categoria = _nuevoProducto.Categoria,
                Precio = _nuevoProducto.Precio,
                RutaImagen = _nuevoProducto.RutaImagen
            };
            if(string.IsNullOrWhiteSpace(_nuevoProducto.Categoria))
            {
                ProductoMensaje.MostrarCategoria = false;
            }
            else
            {
                ProductoMensaje.MostrarCategoria = true;
            }

            string Resultado = _productoService.CrearProducto(_nuevoProducto);
            if (Resultado != null )
            {
                _servicioIndexacion.IndexarProducto(_nuevoProducto);
                ProductoMensaje.ID = Resultado.ToString();
                ProductoMensaje.RutaImagen = string.IsNullOrWhiteSpace(ProductoMensaje.RutaImagen)  ? string.Empty : Path.GetFullPath(ProductoMensaje.RutaImagen); ;
                Messenger.Default.Publish(new ProductoAniadidoMensaje { NuevoProducto = ProductoMensaje });
                CerrarVistaCommand.Execute(0);
                _servicioSFX.Confirmar();
                Notificacion _notificacion = new Notificacion { Mensaje = "Item añadido con exito", Titulo = "Operación Completada", IconoRuta = Path.GetFullPath(IconoNotificacion.OK), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
            }
            else
            {
                _servicioSFX.Suspenso();
                Notificacion _notificacion = new Notificacion { Mensaje = "No se pudo añadir el Item", Titulo = "Operación Cancelada", IconoRuta = Path.GetFullPath(IconoNotificacion.SUSPENSO1), Urgencia = MatrizEisenhower.C1 };
                Messenger.Default.Publish(new NotificacionEmergente { NuevaNotificacion = _notificacion });
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
