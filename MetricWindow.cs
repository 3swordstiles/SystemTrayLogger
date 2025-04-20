// MetricsWindow.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemTrayLogger
{
    public partial class MetricsWindow : Form
    {
        // Labels for displaying the system metrics
        private Label cpuLabel;
        private Label ramLabel;
        private Label storageLabel;
        private Label cpuTemperatureLabel;  // Label for CPU Temperature
        private Label diskTemperatureLabel; // Label for Disk Temperature

        // Constructor to initialize the window and UI components
        public MetricsWindow()
        {
            InitializeComponent();
            InitializeMetricsWindowUI();
        }

        // Method to initialize the UI for the metrics window
        private void InitializeMetricsWindowUI()
        {
            this.Text = "System Metrics";
            this.Size = new System.Drawing.Size(250, 250); // Adjusted size to fit more labels

            // Initialize CPU usage label
            cpuLabel = new Label
            {
                Text = "CPU Usage: 0%",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(230, 25)
            };

            // Initialize RAM usage label
            ramLabel = new Label
            {
                Text = "RAM Usage: 0 MB",
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(230, 25)
            };

            // Initialize Storage usage label
            storageLabel = new Label
            {
                Text = "Storage Usage: 0 GB",
                Location = new System.Drawing.Point(10, 70),
                Size = new System.Drawing.Size(230, 25)
            };

            // Initialize CPU Temperature label
            cpuTemperatureLabel = new Label
            {
                Text = "CPU Temperature: 0°C",
                Location = new System.Drawing.Point(10, 100),  // Adjusted position
                Size = new System.Drawing.Size(230, 25)
            };

            // Initialize Disk Temperature label
            diskTemperatureLabel = new Label
            {
                Text = "Disk Temperature: 0°C",
                Location = new System.Drawing.Point(10, 130),  // Adjusted position
                Size = new System.Drawing.Size(230, 25)
            };

            // Add the labels to the window
            this.Controls.Add(cpuLabel);
            this.Controls.Add(ramLabel);
            this.Controls.Add(storageLabel);
            this.Controls.Add(cpuTemperatureLabel);  // Add CPU Temperature label
            this.Controls.Add(diskTemperatureLabel); // Add Disk Temperature label
        }

        // Method to update the metrics when new data is available
        public void UpdateMetrics(string cpu, string ram, string storage, string cpuTemp, string diskTemp)
        {
            // Update the label texts with the provided values
            cpuLabel.Text = $"CPU Usage: {cpu}";
            ramLabel.Text = $"RAM Usage: {ram}";
            storageLabel.Text = $"Storage Usage: {storage}";
            cpuTemperatureLabel.Text = $"CPU Temperature: {cpuTemp}°C";  // Update CPU temperature
            diskTemperatureLabel.Text = $"Disk Temperature: {diskTemp}°C"; // Update Disk temperature

            ApplyColorCoding(cpuLabel, double.Parse(cpu.TrimEnd('%')));
            // Apply color coding for RAM usage
            ApplyColorCoding(ramLabel, double.Parse(ram.TrimEnd(" MB".ToCharArray())));
            // Apply color coding for Storage usage
            ApplyColorCoding(storageLabel, double.Parse(storage.TrimEnd(" GB".ToCharArray())));
        }
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
    }
}
