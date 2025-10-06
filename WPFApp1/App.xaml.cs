using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WPFApp1.Conmutadores;
using WPFApp1.Entidades;
using WPFApp1.Interfaces;
using WPFApp1.Repositorios;
using WPFApp1.Servicios;
using WPFApp1.ViewModels;
using Forms = System.Windows.Forms;
namespace WPFApp1
{
    public partial class App : System.Windows.Application
    {
        private static ServiceProvider _serviceProvider;
        private SplashScreen _splashScreen;
        private static Mutex _mutex = null;
        private Forms.NotifyIcon _trayIcon = new NotifyIcon();
        private MainWindow _mainWindow;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigurarServicios(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        public static T GetService<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }
        private void ConfigurarServicios(IServiceCollection services)
        {

            services.AddSingleton<ConexionDBSQLite>();
            services.AddSingleton<ConexionDBSQLServer>();

            services.AddTransient<ProductosAccesoDatosSQLServer>();
            services.AddTransient<ProductosAcessoDatosSQLite>();

            services.AddTransient<RepoFormatosSQLite>();
            services.AddTransient<RepoFormatosSQLServer>();

            services.AddTransient<RepoVersionesSQLite>();
            services.AddTransient<RepoVersionesSQLServer>();

            services.AddTransient<RepoUbicacionesSQLite>();
            services.AddTransient<RepoUbicacionesSQLServer>();

            services.AddTransient<RepoMarcasSQLite>();
            services.AddTransient<RepoMarcasSQLServer>();

            services.AddTransient<RepoArquetiposSQLite>();
            services.AddTransient<RepoArquetiposSQLServer>();

            services.AddTransient<RepoCategoriasSQLite>();
            services.AddTransient<RepoCategoriasSQLServer>();

            services.AddTransient<RepoVentasSQLite>();
            services.AddTransient<RepoVentasSQLServer>();

            // Conmutadores
            services.AddTransient<IConmutadorEntidadGenerica<Formatos>, ConmutadorFormatos>();
            services.AddTransient<IConmutadorEntidadGenerica<Versiones>, VersionesConmutador>();
            services.AddTransient<IConmutadorEntidadGenerica<Marcas>, MarcasConmutador>();
            services.AddTransient<IConmutadorEntidadGenerica<Ubicaciones>, UbicacionesConmutador>();
            services.AddTransient<IConmutadorEntidadGenerica<Categorias>, CategoriasConmutador>();
            services.AddTransient<IConmutadorEntidadGenerica<Arquetipos>, ArquetiposConmutador>();
            services.AddTransient<IConmutadorEntidadGenerica<Ventas>, VentasConmutador>();

            services.AddTransient<IProductosServicio, ProductoConmutador>(provider =>
            {
                var repoServer = provider.GetRequiredService<ProductosAccesoDatosSQLServer>();
                var repoLocal = provider.GetRequiredService<ProductosAcessoDatosSQLite>();

                return new ProductoConmutador(repoServer, repoLocal);
            });
            
            services.AddTransient<IndexadorGenericoService>();

            services.AddTransient<IndexadorProductoSQLite>();
            services.AddTransient<IndexadorProductoSQLServer>();

            services.AddTransient<IIndexadorProductosRepositorio, IndexadorProductos>(provider =>
            {
                var repositorioLocal = provider.GetRequiredService<IndexadorProductoSQLite>();
                var repositorioRemoto = provider.GetRequiredService<IndexadorProductoSQLServer>();
                var conexionSqlServer = provider.GetRequiredService<ConexionDBSQLServer>();
                return new IndexadorProductos(conexionSqlServer, repositorioLocal, repositorioRemoto);
            });

            services.AddTransient<ServicioIndexacionProductos>(provider =>
            {
                var indexadorGenerico = provider.GetRequiredService<IndexadorGenericoService>();
                var indexadorRepositorioConmutador = provider.GetRequiredService<IIndexadorProductosRepositorio>();

                return new ServicioIndexacionProductos(
                    indexadorGenerico,
                    indexadorRepositorioConmutador
                );
            });

            // Factories
            services.AddTransient<Factories.SqliteRepositorioProductosFactory>(provider =>
            {
                var conexion = provider.GetRequiredService<ConexionDBSQLite>();
                return new Factories.SqliteRepositorioProductosFactory(conexion);
            });
            services.AddTransient<Factories.SqlServerRepositorioProductosFactory>(provider =>
            {
                var instanciaConexionSqlServer = provider.GetRequiredService<ConexionDBSQLServer>();
                ConfiguracionSQLServer configuracionServer = instanciaConexionSqlServer.LeerArchivoConfiguracion();
                return new Factories.SqlServerRepositorioProductosFactory(configuracionServer.CadenaConexion);
            });


            // Registrar ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<AniadirProductoViewModel>();
            services.AddTransient<ConfigurarSQLServerViewModel>();
            services.AddTransient<CatalogoViewModel>();
            services.AddTransient<ExportarProductosViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<OrquestadorProductos>();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            const string appName = "WPFApp1";
            _mutex = new Mutex(true, appName, out bool createdNew);
            if (!createdNew)
            {
                this.Shutdown();
                return;
            }

            _splashScreen = new SplashScreen();
            _splashScreen.Show();
            _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();

            await Task.Run(() => ValidarIntegridadDirectorios());

            // Iniciar icono en bandeja de Sistema
            _trayIcon.Icon = new System.Drawing.Icon("ico128.ico");
            _trayIcon.Text = "Aplicación";
            _trayIcon.Visible = true;
            _trayIcon.DoubleClick += TrayIcon_DoubleClick;

            // Crear menu contextual
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuItemAbrir = new ToolStripMenuItem("Abrir");
            ToolStripMenuItem menuItemSalir = new ToolStripMenuItem("Salir");
            menuItemAbrir.Click += MenuItemAbrir_Click;
            menuItemSalir.Click += MenuItemSalir_Click;
            contextMenu.Items.Add(menuItemAbrir);
            contextMenu.Items.Add(menuItemSalir);
            _trayIcon.ContextMenuStrip = contextMenu;

            _mainWindow.Closing += MainWindow_Closing;
            _mainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            _splashScreen.Close();
            _mainWindow.Activate();
        }
        private void ValidarIntegridadDirectorios()
        {
            string rutaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos");
            string rutaExportaciones = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Exportaciones");
            string rutaIconos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Iconos");
            string rutaGaleria = Path.Combine(rutaDatos, "Miniaturas");

            CrearDirectorioInexistente(rutaDatos);
            CrearDirectorioInexistente(rutaExportaciones);
            CrearDirectorioInexistente(rutaIconos);
            CrearDirectorioInexistente(rutaGaleria);
        }

        private bool CrearDirectorioInexistente(string Ruta)
        {
            if (!Directory.Exists(Ruta))
            {
                try
                {
                    Directory.CreateDirectory(Ruta);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el directorio {ex.Message}");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Cancela el cierre de la ventana y en su lugar la esconde, para evitar que se cierre la aplicación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            _mainWindow.Hide();
        }

        private void MenuItemAbrir_Click(object sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Maximized;
            _mainWindow.Activate();
        }

        private void MenuItemSalir_Click(object sender, EventArgs e)
        {
            _trayIcon.Dispose();
            this.Shutdown();
        }

        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            _mainWindow.Show();
            _mainWindow.WindowState = WindowState.Maximized;
            _mainWindow.Activate();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
            }

            if (_trayIcon != null)
            {
                _trayIcon.Dispose();
            }
            base.OnExit(e);
        }
    }

}
