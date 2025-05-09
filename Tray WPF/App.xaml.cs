
using System.Drawing;
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
        }
    }
}
