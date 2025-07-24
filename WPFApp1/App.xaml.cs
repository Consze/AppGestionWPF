using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddTransient<Factories.SqliteRepositorioProductosFactory>(provider =>
            {
                string sqliteConnectionString = @"Data Source=.\datos\base.db;Version=3;";
                return new Factories.SqliteRepositorioProductosFactory(sqliteConnectionString);
            });

            services.AddTransient<Factories.SqlServerRepositorioProductosFactory>(provider =>
            {
                ConexionDBSQLServer _instancia = new ConexionDBSQLServer();
                ConfiguracionSQLServer configuracionServer = _instancia.LeerArchivoConfiguracion();
                if (configuracionServer.ConexionValida && configuracionServer.CadenaConexion != null)
                {
                    return new Factories.SqlServerRepositorioProductosFactory(configuracionServer.CadenaConexion);
                }
                return null;
            });

            services.AddScoped<WPFApp1.Interfaces.IProductoServicio, WPFApp1.Servicios.ProductoServicio>();
            services.AddSingleton<ConexionDBSQLite>();
            services.AddTransient<IndexadorProductosRepositorio>();
            services.AddTransient<IndexadorProductoService>();

            // Registrar ViewModels
            services.AddScoped<MainWindowViewModel>();
            services.AddTransient<AniadirProductoViewModel>();
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
            _mainWindow = new MainWindow();

            await Task.Run(() => ValidarIntegridadDirectorios());

            // Iniciar el icono en bandeja del sistema
            _trayIcon = new NotifyIcon();
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
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
            }
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
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
            }
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
