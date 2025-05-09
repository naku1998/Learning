using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceProcess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using static Tray_WPF.MainWindow;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;



namespace Tray_WPF
{
    public partial class MainWindow : Window
    {
       
        private readonly Dictionary<string, string> serviceMappings = new Dictionary<string, string>
        {
            //add the services reel and real name here MSSQLSERVER
            { "Service 1", "TieringEngineService" },
            { "Service 2", "RepairService" },
            { "Service 3", "Power" }
        };

        public List<string> offlineserices = new List<string>();

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

            serviceTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            serviceTimer.Tick += (s, e) => CheckServices();
            serviceTimer.Start();

            CheckServices(); // Immediate first check
            LoadServiceStatus(); // Populate ListView
        }


        private void CheckServices()
        {
            bool allRunning = true;
            offlineserices.Clear();
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
                    app.trayIcon.Text = iconName == "red.ico" ? string.Join(", ", offlineserices) + " not running" : "All services are Running";
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
                        item.Stopped = false;
                        return true;
                    }
                    else
                    {
                        item.StatusColor = Brushes.Red;
                        item.Stopped = true;
                        try
                        {
                            offlineserices.Add(item.DisplayName);
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                            item.StatusColor = Brushes.Green;
                            item.IsRunning = ServiceControllerStatus.Running.ToString();
                            return true;
                        }
                        catch
                        {
                            item.Stopped = true;
                            return false;
                        }
                    }
                }
            }
            catch
            {
                offlineserices.Add(item.DisplayName);
                item.StatusColor = Brushes.Gray;
                item.Stopped = true;
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
                        list.Add(new ServiceDisplayItem { DisplayName = displayName, ActualServiceName = actualServiceName, IsRunning = sc.Status.ToString(), Stopped = sc.Status != ServiceControllerStatus.Running });
                    }
                }
                catch (Exception ex)
                {
                    list.Add(new ServiceDisplayItem { DisplayName = displayName, IsRunning = "Error", ActualServiceName = actualServiceName,Stopped = true });
                }
            }

            ServiceList.ItemsSource = list;
        }

        private System.Drawing.Icon LoadIcon(string iconName)
        {
            var path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iconName);
            return new System.Drawing.Icon(path);
        }

            private void StartServiceButton_Click(object sender, RoutedEventArgs e)
            {
                if (sender is Button button && button.Tag is ServiceDisplayItem item)
                {
                    try
                    {
                        using (ServiceController sc = new ServiceController(item.ActualServiceName))
                        {
                            if (sc.Status != ServiceControllerStatus.Running)
                            {
                                sc.Start();
                                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                                item.StatusColor = Brushes.Green;
                                item.IsRunning = ServiceControllerStatus.Running.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to start service {item.DisplayName}:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
                        Stopped = (_isRunning == "Stopped");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            private bool _Stopped { get; set; } = true;
            public bool Stopped
            {
                get => _Stopped;
                set
                {
                        _Stopped = value;
                        OnPropertyChanged(nameof(Stopped));   
                }
            }

        }

    }
}

