using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Threading;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;



namespace Tray_WPF
{
    public partial class MainWindow : Window
    {
       
        private readonly Dictionary<string, string> serviceMappings = new Dictionary<string, string>
        {
            //add the services reel and real name here MSSQLSERVER
            { "Service 1", "CscService" },
            { "Service 2", "RepairService" },
            { "Service 3", "Power" }
        };

        private ObservableCollection<ServiceDisplayItem> serviceDisplayItems;
        private DispatcherTimer serviceTimer;
        public MainWindow()
        {
            InitializeComponent();
            serviceDisplayItems = new ObservableCollection<ServiceDisplayItem>();
            foreach (var kv in serviceMappings)
            {
                serviceDisplayItems.Add(new ServiceDisplayItem
                {
                    DisplayName = kv.Key,
                    ActualServiceName = kv.Value
                });
            }
            StatusList.ItemsSource = serviceDisplayItems;

            serviceTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            serviceTimer.Tick += (s, e) => CheckServices();
            serviceTimer.Start();

            CheckServices(); // Immediate first check
            LoadServiceStatus(); // Populate ListView
        }


        private void CheckServices()
        {
            bool allRunning = true;
            foreach (var item in serviceDisplayItems)
            {
                allRunning &= UpdateServiceStatus(item);
            }

            // Change icon based on the service status
            string iconName = allRunning ? "green.ico" : "red.ico";
            UpdateTrayIcon(iconName);

            // Update the list view display
            LoadServiceStatus();
        }

        private void UpdateTrayIcon(string iconName)
        {
            try
            {
                var app = (App)Application.Current;

                if (app.trayIcon != null)
                {
                    app.trayIcon.Icon = LoadIcon(iconName);
                    app.trayIcon.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating tray icon: " + ex.Message);
            }
        }
        private bool UpdateServiceStatus(ServiceDisplayItem item)
        {
            try
            {
                using (ServiceController sc = new ServiceController(item.ActualServiceName))
                {
                    sc.Refresh();
                    item.IsRunning = sc.Status.ToString();

                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        item.StatusColor = Brushes.Green;
                        return true;
                    }
                    else
                    {
                        item.StatusColor = Brushes.Red;
                        try
                        {
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                            item.StatusColor = Brushes.Green;
                            item.IsRunning = ServiceControllerStatus.Running.ToString();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                item.StatusColor = Brushes.Gray;
                item.IsRunning = "Error";
                return false;
            }
        }


        public void LoadServiceStatus()
        {
            var list = new List<ServiceDisplayItem>();

            foreach (var pair in serviceMappings)
            {
                string displayName = pair.Key;
                string actualServiceName = pair.Value;
                try
                {
                    using (ServiceController sc = new ServiceController(actualServiceName))
                    {
                        sc.Refresh();
                        list.Add(new ServiceDisplayItem { DisplayName = displayName, IsRunning = sc.Status.ToString() });
                    }
                }
                catch (Exception ex)
                {
                    list.Add(new ServiceDisplayItem { DisplayName = displayName, IsRunning = "Error" });
                }
            }

            ServiceList.ItemsSource = list;
        }

        private System.Drawing.Icon LoadIcon(string iconName)
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", iconName);
            return new System.Drawing.Icon(path);
        }

        public class ServiceDisplayItem: INotifyPropertyChanged
        {
            public string DisplayName { get; set; }          // What user sees
            public string ActualServiceName { get; set; }     // What is used in code

            private Brush _statusColor = Brushes.Gray;
            public Brush StatusColor
            {
                get => _statusColor;
                set
                {
                    if (_statusColor != value)
                    {
                        _statusColor = value;
                        OnPropertyChanged(nameof(StatusColor));
                    }
                }
            }
            private string _isRunning = "Unknown";
            public string IsRunning
            {
                get => _isRunning;
                set
                {
                    if (_isRunning != value)
                    {
                        _isRunning = value;
                        OnPropertyChanged(nameof(IsRunning));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
