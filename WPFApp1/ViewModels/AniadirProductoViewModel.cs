using System.Collections.ObjectModel;
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
        private readonly IProductosServicio _productoService;
        private readonly ServicioIndexacionProductos _servicioIndexacion;
        public ObservableCollection<EntidadNombrada> Formatos { get; set; }
        public bool EsModoEdicion { get; set; }
        public string NombreDeVentana { get; set; }
        private int CalculoAlturaMarco;
        private int CalculoAnchoMarco;

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


        //---- Propiedades de registro -----
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
        private DateTime _fechaCreacionProducto;
        public DateTime FechaCreacionProducto
        {
            get { return _fechaCreacionProducto; }
            set
            {
                if (_fechaCreacionProducto != value)
                {
                    _fechaCreacionProducto = value;
                    OnPropertyChanged(nameof(FechaCreacionProducto));
                }
            }
        }
        private DateTime _fechaModificacionProducto;
        public DateTime FechaModificacionProducto
        {
            get { return _fechaModificacionProducto; }
            set
            {
                if (_fechaModificacionProducto != value)
                {
                    _fechaModificacionProducto = value;
                    OnPropertyChanged(nameof(FechaModificacionProducto));
                }
            }
        }


        //CADENA
        private string _ean;
        public string EAN
        {
            get { return _ean; }
            set
            {
                if (_ean != value)
                {
                    _ean = value;
                    OnPropertyChanged(nameof(EAN));
                }
            }
        }
        private string _marcaNombre;
        public string MarcaNombre
        {
            get { return _marcaNombre; }
            set
            {
                if (_marcaNombre != value)
                {
                    _marcaNombre = value;
                    OnPropertyChanged(nameof(MarcaNombre));
                }
            }
        }
        private string _categoriaNombre;
        public string CategoriaNombre
        {
            get { return _categoriaNombre; }
            set
            {
                if (_categoriaNombre != value)
                {
                    _categoriaNombre = value;
                    OnPropertyChanged(nameof(CategoriaNombre));
                }
            }
        }
        private string _ubicacionNombre;
        public string UbicacionNombre
        {
            get { return _ubicacionNombre; }
            set
            {
                if (_ubicacionNombre != value)
                {
                    _ubicacionNombre = value;
                    OnPropertyChanged(nameof(UbicacionNombre));
                }
            }
        }


        //NUMERICAS
        private decimal _precioProducto;
        public decimal PrecioProducto
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
        private int _cantidadEnStock;
        public int CantidadEnStock
        {
            get { return _cantidadEnStock; }
            set
            {
                if (_cantidadEnStock != value)
                {
                    _cantidadEnStock = value;
                    OnPropertyChanged(nameof(CantidadEnStock));
                }
            }
        }
        private decimal _alto;
        public decimal Alto
        {
            get { return _alto; }
            set
            {
                if (_alto != value)
                {
                    _alto = value;
                    OnPropertyChanged(nameof(Alto));
                }
            }
        }
        private decimal _profundidad;
        public decimal Profundidad
        {
            get { return _profundidad; }
            set
            {
                if (_profundidad != value)
                {
                    _profundidad = value;
                    OnPropertyChanged(nameof(Profundidad));
                }
            }
        }
        private decimal _largo;
        public decimal Largo
        {
            get { return _largo; }
            set
            {
                if (_largo != value)
                {
                    _largo = value;
                    OnPropertyChanged(nameof(Largo));
                }
            }
        }
        private decimal _peso;
        public decimal Peso
        {
            get { return _peso; }
            set
            {
                if (_peso != value)
                {
                    _peso = value;
                    OnPropertyChanged(nameof(Peso));
                }
            }
        }


        //BOOLEANAS
        private bool _visibilidadWeb;
        public bool VisibilidadWeb
        {
            get { return _visibilidadWeb; }
            set
            {
                if (_visibilidadWeb != value)
                {
                    _visibilidadWeb = value;
                    OnPropertyChanged(nameof(VisibilidadWeb));
                }
            }
        }
        private bool _precioPublico;
        public bool PrecioPublico
        {
            get { return _precioPublico; }
            set
            {
                if (_precioPublico != value)
                {
                    _precioPublico = value;
                    OnPropertyChanged(nameof(PrecioPublico));
                }
            }
        }
        private bool _esProductoEliminado;
        public bool EsProductoEliminado
        {
            get { return _esProductoEliminado; }
            set
            {
                if (_esProductoEliminado != value)
                {
                    _esProductoEliminado = value;
                    OnPropertyChanged(nameof(EsProductoEliminado));
                }
            }
        }


        //CLAVES
        private string _marcaProductoID;
        public string MarcaProductoID
        {
            get { return _marcaProductoID; }
            set
            {
                if (_marcaProductoID != value)
                {
                    _marcaProductoID = value;
                    OnPropertyChanged(nameof(MarcaProductoID));
                }
            }
        }
        private string _formato;
        public string FormatoID
        {
            get { return _formato; }
            set
            {
                if (_formato != value)
                {
                    _formato = value;
                    OnPropertyChanged(nameof(FormatoID));
                }
            }
        }
        private string _productoVersionID;
        public string ProductoVersionID
        {
            get { return _productoVersionID; }
            set
            {
                if (_productoVersionID != value)
                {
                    _productoVersionID = value;
                    OnPropertyChanged(nameof(ProductoVersionID));
                }
            }
        }
        private string _ubicacionID;
        public string UbicacionID
        {
            get { return _ubicacionID; }
            set
            {
                if (_ubicacionID != value)
                {
                    _ubicacionID = value;
                    OnPropertyChanged(nameof(UbicacionID));
                }
            }
        }
        private string _categoriaProductoID;
        public string CategoriaProductoID
        {
            get { return _categoriaProductoID; }
            set
            {
                if (_categoriaProductoID != value)
                {
                    _categoriaProductoID = value;
                    OnPropertyChanged(nameof(CategoriaProductoID));
                }
            }
        }
        private string _productoID;
        public string ProductoID
        {
            get { return _productoID; }
            set
            {
                if (_productoID != value)
                {
                    _productoID = value;
                    OnPropertyChanged(nameof(ProductoID));
                }
            }
        }
        public string ProductoSKU { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CierreSolicitado;
        private ServicioSFX _servicioSFX { get; set; }
        public ICommand ElegirImagenCommand{ get; }
        public ICommand AniadirProductoCommand { get; }
        public ICommand BotonPresionadoCommand { get; }
        public ICommand CerrarVistaCommand { get; }
        public ICommand ModificarCantidadStockCommand { get; }

        public AniadirProductoViewModel(IProductosServicio productoServicio, ServicioIndexacionProductos ServicioIndexacion)
        {
            //Imagen
            RutaImagenSeleccionada = string.Empty;
            AnchoImagenSeleccionada = 0;
            AltoImagenSeleccionada = 0;
            CalculoAlturaMarco = 0;
            CalculoAnchoMarco = 0;
            ProductoSKU = null;

            //Entidad
            NombreProducto = string.Empty;
            CategoriaNombre = string.Empty;
            PrecioProducto = 0;
            VisibilidadWeb = false;
            PrecioPublico = false;
            CantidadEnStock = 0;

            NombreDeVentana = "Añadir Producto";
            ElegirImagenCommand = new RelayCommand<object>(ElegirImagen);
            AniadirProductoCommand = new RelayCommand<object>(AniadirProducto);
            CerrarVistaCommand = new RelayCommand<object>(CerrarVista);
            BotonPresionadoCommand = new RelayCommand<object>(BotonPresionado);
            ModificarCantidadStockCommand = new RelayCommand<object>(ModificarCantidadStock);

            Messenger.Default.Subscribir<VistaAniadirProductosCantidadModificada>(OnCantidadModificada);

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
        public void ConfigurarEdicionDeProducto(ProductoCatalogo Producto)
        {
            // Configurar Bindings
            EsModoEdicion = true;
            NombreDeVentana = "Editar Producto";

            //--PK--
            ProductoSKU = Producto.ProductoSKU;

            //Cadena
            RutaImagenSeleccionada = Producto.RutaImagen;
            NombreProducto = Producto.Nombre;
            EAN = Producto.EAN;
            CategoriaNombre = Producto.CategoriaNombre;
            MarcaNombre = Producto.MarcaNombre;

            //Fecha
            FechaCreacionProducto = Producto.FechaCreacion;
            FechaModificacionProducto = Producto.FechaModificacion;

            //Claves
            CategoriaProductoID = Producto.Categoria;
            ProductoVersionID = Producto.ProductoVersionID;
            MarcaProductoID = Producto.MarcaID;
            UbicacionID = Producto.UbicacionID;
            FormatoID = Producto.FormatoProductoID;
            ProductoID = Producto.ID;

            //Booleanas
            VisibilidadWeb = Producto.VisibilidadWeb;
            PrecioPublico = Producto.PrecioPublico;
            EsProductoEliminado = Producto.EsEliminado;

            //Numericas
            CantidadEnStock = Producto.Haber;
            PrecioProducto = Producto.Precio;
            Alto = Producto.Alto;
            Peso = Producto.Peso;
            Profundidad = Producto.Profundidad;
            Largo = Producto.Largo;

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
            ProductoCatalogo ProductoModificado = new ProductoCatalogo {
                ID = ProductoID,
                UbicacionID = UbicacionID,
                ProductoSKU = ProductoSKU,
                FormatoProductoID = FormatoID,
                Categoria = CategoriaProductoID,
                ProductoVersionID = ProductoVersionID,
                MarcaID = MarcaProductoID,

                Nombre = NombreProducto,
                RutaImagen = RutaImagenSeleccionada,
                CategoriaNombre = CategoriaNombre,
                MarcaNombre = MarcaNombre,
                EAN = EAN,

                VisibilidadWeb = VisibilidadWeb,
                PrecioPublico = PrecioPublico,
                EsEliminado = EsProductoEliminado,

                FechaCreacion = FechaCreacionProducto,
                FechaModificacion = FechaModificacionProducto,

                Precio = PrecioProducto,
                Haber = CantidadEnStock,
                Alto = Alto,
                Profundidad = Profundidad,
                Largo = Largo,
                Peso = Peso

            };
            ProductoBase ProductoMensaje = ProductoModificado;

            if (_productoService.ModificarProducto(ProductoModificado))
            {
                ProductoMensaje.Categoria = ProductoModificado.CategoriaNombre;
                _servicioIndexacion.IndexarProducto(ProductoMensaje);
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
            if (string.IsNullOrWhiteSpace(NombreProducto) || string.IsNullOrWhiteSpace(CategoriaNombre) || PrecioProducto == 0)
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

            ProductoCatalogo _nuevoProducto = new ProductoCatalogo
            {
                ID = ProductoID,
                UbicacionID = UbicacionID,
                ProductoSKU = ProductoSKU,
                FormatoProductoID = FormatoID,
                Categoria = CategoriaProductoID,
                ProductoVersionID = ProductoVersionID,
                MarcaID = MarcaProductoID,

                Nombre = NombreProducto,
                RutaImagen = RutaImagenSalida,
                CategoriaNombre = CategoriaNombre,
                MarcaNombre = MarcaNombre,
                EAN = EAN,

                VisibilidadWeb = VisibilidadWeb,
                PrecioPublico = PrecioPublico,
                EsEliminado = EsProductoEliminado,

                FechaCreacion = FechaCreacionProducto,
                FechaModificacion = FechaModificacionProducto,

                Precio = PrecioProducto,
                Haber = CantidadEnStock,
                Alto = Alto,
                Profundidad = Profundidad,
                Largo = Largo,
                Peso = Peso
            };
            ProductoBase ProductoMensaje = _nuevoProducto;

            string NuevoProductoSKU = _productoService.CrearProducto(_nuevoProducto);
            if (NuevoProductoSKU != null)
            {
                ProductoMensaje.Categoria = _nuevoProducto.CategoriaNombre;
                _servicioIndexacion.IndexarProducto(ProductoMensaje);
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
        public void ModificarCantidadStock(object parameter)
        {
            string caso = parameter as string;

            if (caso == "SUMAR")
            {
                CantidadEnStock++;
            }
            else
            {
                if (CantidadEnStock > 0)
                {
                    CantidadEnStock--;
                }
            }
        }
        public void OnCantidadModificada(VistaAniadirProductosCantidadModificada _mensaje)
        {
            // Invocar como dialogo (modal) entrada de usuario
            InputUsuarioViewModel _viewModel = new InputUsuarioViewModel("Ingrese Cantidad de Stock");
            InputUsuario _vista = new InputUsuario(_viewModel);
            _vista.ShowDialog();
            int Cantidad;

            if (int.TryParse(_viewModel.Entrada, out Cantidad))
            {
                CantidadEnStock = Cantidad;
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
