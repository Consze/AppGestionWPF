﻿using System.Windows;
using Forms = System.Windows.Forms;
namespace WPFApp1
{
    public partial class App : System.Windows.Application
    {
        private SplashScreen _splashScreen;
        private static Mutex _mutex = null;
        private Forms.NotifyIcon _trayIcon = new NotifyIcon();
        private MainWindow _mainWindow;
        protected override void OnStartup(StartupEventArgs e)
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

            // Inicializar el icono en la bandeja del sistema
            _trayIcon = new NotifyIcon();
            _trayIcon.Icon = new System.Drawing.Icon("ico128.ico");
            _trayIcon.Text = "Aplicación";
            _trayIcon.Visible = true;
            _trayIcon.DoubleClick += TrayIcon_DoubleClick;

            // Crear un menú contextual
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuItemAbrir = new ToolStripMenuItem("Abrir");
            ToolStripMenuItem menuItemSalir = new ToolStripMenuItem("Salir");
            menuItemAbrir.Click += MenuItemAbrir_Click;
            menuItemSalir.Click += MenuItemSalir_Click;
            contextMenu.Items.Add(menuItemAbrir);
            contextMenu.Items.Add(menuItemSalir);
            _trayIcon.ContextMenuStrip = contextMenu;

            _mainWindow.Closing += MainWindow_Closing;
            _splashScreen.Close();
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
            _mainWindow.WindowState = WindowState.Normal;
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
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.Activate();
        }

        protected override void OnExit(ExitEventArgs e)
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
