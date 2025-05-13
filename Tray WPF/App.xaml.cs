
using Serilog;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using Forms = System.Windows.Forms;


namespace Tray_WPF
{

    public partial class App : Application
    {

        private Forms.NotifyIcon _trayIcon;
        public Forms.NotifyIcon trayIcon => _trayIcon;

        private MainWindow mainWindow;


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            mainWindow = new MainWindow();
            mainWindow.Hide(); // Start hidden
            SetupTrayIcon();
            serilog();
            Log.Information("Application started.");
        }
        private void SetupTrayIcon()
        {
            _trayIcon = new Forms.NotifyIcon
            {
                Icon = new Icon("green.ico"),
                Visible = true,
                Text = "All Services are running",
                ContextMenuStrip = new Forms.ContextMenuStrip()
            };

            _trayIcon.ContextMenuStrip.Items.Add("Open", null, (s, ev) => ShowMainWindow());
            _trayIcon.ContextMenuStrip.Items.Add("Exit", null, (s, ev) => ExitApp());

            _trayIcon.DoubleClick += (s, ev) => ShowMainWindow();
        }
        private void ShowMainWindow()
        {
            if (!mainWindow.IsVisible)
            {
                mainWindow.Show();
                mainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                mainWindow.Activate();
            }

            mainWindow.LoadServiceStatus();
        }

        private void ExitApp()
        {
            _trayIcon.Visible = false;
            mainWindow.Close();
            Shutdown();
            Log.Information("Application exited.");
            Log.CloseAndFlush();
        }

        public void serilog()
        {

            // Define folder and path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string logFolder = Path.Combine(desktopPath, "MyAppLogs");

            string logPath = Path.Combine(logFolder, "log-.txt");

            // Ensure directory exists
            Directory.CreateDirectory(logFolder);
            Log.Logger = new LoggerConfiguration()
                           .MinimumLevel.Debug()
                           .WriteTo.File(
                               path: logPath,
                               rollingInterval: RollingInterval.Day,
                               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}"
                           )
                           .CreateLogger();

        }
    }
}
