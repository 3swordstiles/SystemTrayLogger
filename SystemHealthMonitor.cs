using System;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Win32;
using OpenHardwareMonitor.Hardware;

namespace SystemTrayLogger
{
    public partial class SystemHealthMonitor : Form
    {
        // Tray icon and context menu for system tray
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private System.Windows.Forms.Timer timer;

        // UI Elements to display CPU, RAM, and Storage usage on the form
        private Label cpuLabel;
        private Label ramLabel;
        private Label storageLabel;
        private Label cpuTemperatureLabel;
        private Label diskTemperatureLabel;

        public SystemHealthMonitor()
        {
            EnsureLogDirectory();
            InitializeComponent();
            InitializeFormUI();
            InitializeTrayApp();     // Initialize tray functionality
            InitializeLogging();     // Initialize logging functionality
            InitializeHardwareMonitor(); // Initialize hardware monitoring

            AddToStartup(); // Initialize the form UI with labels for system stats

            this.Shown += (sender, e) =>
            {
                // Log system metrics after the form is shown
                LogSystemMetrics();
            };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Ensure that hardware monitor is closed when the form is closing
            CloseHardwareMonitor();
            base.OnFormClosing(e);
        }
        private void InitializeFormUI()
        {
            this.BackColor = Color.LightGray;
            var customFont = new Font("Segoe UI", 14, FontStyle.Bold);
            // Initialize Labels for displaying system information
            cpuLabel = new Label
            {
                Text = "CPU Usage: 0%",
                Location = new Point(10, 10),
                Size = new Size(300, 30)
            };
            ramLabel = new Label
            {
                Text = "RAM Usage: 0 MB",
                Location = new Point(10, 50),
                Size = new Size(300, 30)
            };
            storageLabel = new Label
            {
                Text = "Storage Usage: 0 GB",
                Location = new Point(10, 90),
                Size = new Size(300, 30)
            };
            cpuTemperatureLabel = new Label
            {
                Text = "CPU Temperature: 0°C",
                Location = new Point(10, 130),  // Adjust position
                Size = new Size(300, 30)
            };

            // Add Disk Temperature label
            diskTemperatureLabel = new Label
            {
                Text = "Disk Temperature: 0°C",
                Location = new Point(10, 170),  // Adjust position
                Size = new Size(300, 30)
            };

            // Add labels to the form
            this.Controls.Add(cpuLabel);
            this.Controls.Add(ramLabel);
            this.Controls.Add(storageLabel);
            this.Controls.Add(cpuTemperatureLabel); // Add the CPU temperature label
            this.Controls.Add(diskTemperatureLabel); // Add the Disk temperature label
            
            Button exportButton = new Button
            {
                Text = "Export to CSV",
                Location = new Point(10, 210),
                Size = new Size(150, 30)
            };
            exportButton.Click += (sender, e) => ConvertLogToCsv();
            this.Controls.Add(exportButton);

        }

        private MetricsWindow metricsWindow;
        // Method to set up tray functionality (tray icon and context menu)
        private void InitializeTrayApp()
        {
            // Create a context menu for the tray icon
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Show Metrics", null, OnShowMetrics);  // Add a "Show Metrics" option
            trayMenu.Items.Add("Exit", null, OnExit);  // Add an "Exit" option to the menu

            // Create the tray icon and set its properties
            trayIcon = new NotifyIcon
            {
                Icon = new Icon(GetType(), "system_health_monitor_DKO_icon.ico"),  // Custom icon here!
                ContextMenuStrip = trayMenu,
                Text = "System Tray Logger",
                Visible = true
            };

            // Minimize the window to system tray and hide the window from the taskbar
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;  // Hide the form from the taskbar

            // Optional: Add double-click functionality to restore the window from the tray
            trayIcon.DoubleClick += (sender, e) =>
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Show();  // Show the form again if double-clicked in the tray
            };
        }
        private void AddToStartup()
        {
            string runKey = "SystemHealthMonitor";
            string exePath = Application.ExecutablePath;
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            object currentValue = key.GetValue(runKey);
            if (currentValue == null || currentValue.ToString() != exePath)
            {
                key.SetValue(runKey, exePath);
            }
        }

       

        // Action when "Exit" is selected from the tray menu
        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;  // Hide the tray icon when exiting
            Application.Exit();        // Close the application
        }

        // Initialize the form UI elements like Labels for CPU, RAM, and Storage usage
        

        // Method to initialize logging and periodically log system metrics
        private void InitializeLogging()
        {
            // Create a timer that runs every 10 minutes
            timer = new System.Windows.Forms.Timer
            {
                Interval = 10 * 60 * 2000 // 20 minutes in milliseconds
            };
            timer.Tick += (sender, e) => LogSystemMetrics();  // Log system metrics every tick
            timer.Start();  // Start the timer

            // Also log immediately when the app starts
            LogSystemMetrics();
        }

        // Method to gather system metrics (CPU, RAM, Storage)
        private DateTime? cpuLastAlertTime = null;
        private DateTime? ramLastAlertTime = null;
        private DateTime? storageLastAlertTime = null;
        private DateTime? cpuTempLastAlertTime = null;
        private DateTime? diskTempLastAlertTime = null;
        private const double alertThresholdMinutes = 60; // 1 hour in minutes


        private void OnShowMetrics(object sender, EventArgs e)
        {
            // Create the MetricsWindow if it doesn't exist yet
            if (metricsWindow == null || metricsWindow.IsDisposed)
            {
                metricsWindow = new MetricsWindow();
            }

            // Show the MetricsWindow
            metricsWindow.Show();
        }
        private void LogSystemMetrics()
        {
            // Gather system metrics (already in percentage for CPU, calculate percentage for RAM and Storage)
            var cpuUsage = GetCpuUsage();
            var ramUsage = GetRamUsage();
            var storageUsage = GetStorageUsage();

            // Get total RAM (in MB)
            double totalRam = GetTotalRam();
            // Get total storage (in GB)
            double totalStorage = GetTotalStorage();

            // Calculate RAM percentage
            double ramPercentage = (ramUsage / totalRam) * 100;

            // Calculate Storage percentage
            double storagePercentage = (storageUsage / totalStorage) * 100;

            // Get other metrics like disk and CPU temperatures
            var diskTemperature = GetDiskTemperature();  // New metric
            var cpuTemperature = GetCpuTemperature();  // New CPU temperature

            // Update the UI with these metrics
            UpdateHealthStatus(cpuUsage, ramPercentage, storagePercentage, cpuTemperature, diskTemperature);
           
            // Log the system metrics to a file
            var systemMetrics = new
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                CPU = cpuUsage,
                RAM = ramUsage,
                Storage = storageUsage,
                DiskTemperature = diskTemperature,
                CpuTemperature = cpuTemperature
            };

            var json = JsonConvert.SerializeObject(systemMetrics);
            SaveLogToFile(json);
        }

        // Method to get total RAM (in MB)
        private double GetTotalRam()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            double totalRam = 0;

            foreach (ManagementObject obj in searcher.Get())
            {
                totalRam = Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024 * 1024); // Convert bytes to MB
            }

            return totalRam;
        }

        // Method to get total Storage (in GB)
        private double GetTotalStorage()
        {
            double totalStorage = 0;

            DriveInfo[] drives = DriveInfo.GetDrives();

            // Check the system drive (typically C:)
            foreach (var drive in drives)
            {
                if (drive.IsReady && drive.Name == "C:\\")
                {
                    totalStorage = drive.TotalSize / (1024 * 1024 * 1024); // Convert bytes to GB
                    break;
                }
               
               
            }

            return totalStorage;
        }


        // Method to apply color coding to the labels based on values
        private void ApplyColorCoding(Label label, double value)
        {
            if (value < 50)
            {
                label.ForeColor = Color.Green;
            }
            else if (value < 80)
            {
                label.ForeColor = Color.Yellow;
            }
            else if (value < 95)
            {
                label.ForeColor = Color.Orange;
            }
            else
            {
                label.ForeColor = Color.Red;
            }
        }

        // Method to check if a metric has been in the red/orange zone for over an hour
        private void CheckForAlerts(double value, string metric)
        {
            DateTime? lastAlertTime = null;

            // Determine the last alert time based on the metric type
            if (metric == "CPU")
            {
                lastAlertTime = cpuLastAlertTime;
            }
            else if (metric == "RAM")
            {
                lastAlertTime = ramLastAlertTime;
            }
            else if (metric == "Storage")
            {
                lastAlertTime = storageLastAlertTime;
            }
            else if (metric == "CPU Temperature")
            {
                lastAlertTime = cpuTempLastAlertTime;
            }
            else if (metric == "Disk Temperature")
            {
                lastAlertTime = diskTempLastAlertTime;
            }

            // Check if the metric is in the red/orange zone
            if (value >= 95)  // Red or orange zone
            {
                if (lastAlertTime == null)
                {
                    // Set the time when the metric enters the red/orange zone
                    lastAlertTime = DateTime.Now;
                }

                var timeSpentInRedOrangeZone = DateTime.Now - lastAlertTime.Value;
                if (timeSpentInRedOrangeZone.TotalMinutes >= alertThresholdMinutes)
                {
                    // Alert the user with a message box or notification
                    MessageBox.Show($"{metric} has been in the red/orange zone for over an hour!", $"{metric} Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lastAlertTime = DateTime.Now;  // Reset alert time after alert is triggered
                }
            }
            else
            {
                // Reset the alert time if the metric is in the green or yellow zone
                if (metric == "CPU")
                {
                    cpuLastAlertTime = null;
                }
                else if (metric == "RAM")
                {
                    ramLastAlertTime = null;
                }
                else if (metric == "Storage")
                {
                    storageLastAlertTime = null;
                }
                else if (metric == "CPU Temperature")
                {
                    cpuTempLastAlertTime = null;
                }
                else if (metric == "Disk Temperature")
                {
                    diskTempLastAlertTime = null;
                }
            }
        }
        private void UpdateHealthStatus(double cpuUsage, double ramPercentage, double storagePercentage, double cpuTemperature, double diskTemperature)
        {
            // Update the labels with the latest metrics
            cpuLabel.Text = $"CPU Usage: {cpuUsage}%";
            ramLabel.Text = $"RAM Usage: {ramPercentage}%";
            storageLabel.Text = $"Storage Usage: {storagePercentage}%";
            cpuTemperatureLabel.Text = $"CPU Temperature: {cpuTemperature}°C";
            diskTemperatureLabel.Text = $"Disk Temperature: {diskTemperature}°C";

            // Apply color-coding based on thresholds for each metric
            ApplyColorCoding(cpuLabel, cpuUsage);
            ApplyColorCoding(ramLabel, ramPercentage);
            ApplyColorCoding(storageLabel, storagePercentage);
            ApplyColorCoding(cpuTemperatureLabel, cpuTemperature);
            ApplyColorCoding(diskTemperatureLabel, diskTemperature);
        }

        private void EnsureLogDirectory()
        {
            string logDirectory = "C:\\SystemTrayLoggerLogs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFile = Path.Combine(logDirectory, "system_metrics_log.json");
            if (!File.Exists(logFile))
            {
                File.WriteAllText(logFile, "[]"); // Initialize as empty JSON array
            }
        }

        private void ConvertLogToCsv()
        {
            string logPath = "C:\\SystemTrayLoggerLogs\\system_metrics_log.json";
            string csvPath = "C:\\SystemTrayLoggerLogs\\metrics_output.csv";

            if (!File.Exists(logPath))
            {
                MessageBox.Show("Log file not found.");
                return;
            }

            var lines = File.ReadAllLines(logPath);
            var csvLines = new List<string> { "Time,CPU (%),RAM (MB),Storage (GB), CPU Temperature (°C), Disk Temperature (°C)" };

            foreach (var line in lines)
            {
                try
                {
                    dynamic entry = JsonConvert.DeserializeObject(line);
                    string time = entry.Time;
                    string cpu = entry.CPU;
                    string ram = entry.RAM;
                    string storage = entry.Storage;
                    string diskTemperature = entry.DiskTemperature;
                    string CPUTemperature = entry.CPUTemperature;

                    csvLines.Add($"{time},{cpu},{ram},{storage},{diskTemperature}{CPUTemperature}");
                }
                catch (Exception ex)
                {
                    // Optional: Skip bad lines or alert the user
                    Console.WriteLine("Error parsing line: " + ex.Message);
                }
            }

            File.WriteAllLines(csvPath, csvLines);
            MessageBox.Show("Log converted to CSV:\n" + csvPath);
        }

        // Get current CPU usage percentage
        private double GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();  // First call returns 0, so we need to call it twice
            System.Threading.Thread.Sleep(1000);
            return cpuCounter.NextValue();
        }
        // Inside your SystemHealthMonitor class, add this:
        private Computer computer;

        // Initialize Open Hardware Monitor object
        private void InitializeHardwareMonitor()
        {
            // Set up the computer object to monitor CPU, GPU, and HDD hardware
            computer = new Computer()
            {
                CPUEnabled = true,
                GPUEnabled = true,
                HDDEnabled = true,
                MainboardEnabled = true  // This enables motherboard and disk sensors if available
            };

            computer.Open(); // Open the connection to the hardware sensors
        }

        // Close the OpenHardwareMonitor when application exits
        private void CloseHardwareMonitor()
        {
            computer.Close();
        }

        // Method to get CPU Temperature using Open Hardware Monitor
        // Inside your SystemHealthMonitor class, replace the following methods:

        private double GetCpuTemperature()
        {
            double cpuTemp = 0.0;

            // Check if the computer object is open
            if (computer == null) return cpuTemp;

            // Iterate through all the hardware components
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();  // Make sure the hardware data is updated

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)  // Look for the temperature sensor
                        {
                            cpuTemp = sensor.Value.GetValueOrDefault();  // Get the value of the CPU temperature
                            break;  // Break after finding the first temperature sensor
                        }
                    }
                }
            }
            return cpuTemp;  // Return the CPU temperature
        }

        private double GetDiskTemperature()
        {
            double diskTemp = 0.0;

            // Check if the computer object is open
            if (computer == null) return diskTemp;

            // Iterate through all hardware components
            foreach (IHardware hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.HDD)  // Look for HDD sensors
                {
                    hardware.Update();  // Update hardware data

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)  // Look for temperature sensor
                        {
                            diskTemp = sensor.Value.GetValueOrDefault();  // Get the disk temperature
                            break;  // Break after finding the first temperature sensor
                        }
                    }
                }
            }
            return diskTemp;  // Return the disk temperature
        }



        // Get current RAM usage (in MB)
        private double GetRamUsage()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }

        // Get current storage usage (in GB) of the system drive
        private double GetStorageUsage()
        {
            var drive = new System.IO.DriveInfo("C");
            double totalSpace = drive.TotalSize / (1024 * 1024 * 1024); // GB
            double freeSpace = drive.AvailableFreeSpace / (1024 * 1024 * 1024); // GB
            return totalSpace - freeSpace;  // Used space
        }
        private async System.Threading.Tasks.Task SendLogToRemoteServerAsync(string json)
        {
            try
            {
                string apiUrl = "https://localhost:7118/api/Logs";

                using (var client = new System.Net.Http.HttpClient())
                {
                    
                    var content = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    // Optional: Add auth header if needed
                    // client.DefaultRequestHeaders.Add("Authorization", "Bearer YOUR_TOKEN");

                    var response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send log remotely: " + ex.Message);
            }
        }
        // Save log data to a JSON file
        private async void SaveLogToFile(string logData)
        {
            string logDirectory = "C:\\SystemTrayLoggerLogs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFile = Path.Combine(logDirectory, "system_metrics_log.json");
            File.AppendAllText(logFile, logData + Environment.NewLine);

            // 🛰️ Send to remote server as well
            await SendLogToRemoteServerAsync(logData);
        }
    }
}
