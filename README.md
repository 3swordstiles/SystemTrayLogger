System Health Monitor
Overview
The System Health Monitor is a lightweight desktop application designed to provide real-time monitoring of your system's health. It tracks various system metrics such as:

CPU Usage

RAM Usage

Storage Usage

CPU Temperature

Disk Temperature

The application sits in your System Tray and continuously monitors and logs these metrics. It also allows you to export your system logs to a CSV file for further analysis.

Features
Real-time Monitoring: Continuously tracks system metrics like CPU usage, RAM usage, storage percentage, CPU temperature, and disk temperature.

System Tray Support: The app runs in the background with an icon in your system tray, keeping it out of sight but easily accessible.

Dynamic UI Adjustment: The application's UI dynamically adjusts based on the visibility of data points, ensuring that UI elements (like the "Export to CSV" button) are positioned below all visible data metrics.

Logging: System metrics are logged at regular intervals.

CSV Export: Export logs to a CSV file for further analysis or record-keeping.

Fully Transparent Startup: The application window starts as fully transparent and hidden, only becoming visible after system metrics have been loaded and adjusted for better user experience.
How to Install and Run the Application
Download the ZIP File
Download the ZIP file containing the System Health Monitor application from the release page.

Extract the ZIP File
Once the download is complete, extract the contents of the ZIP file to a location on your computer.

Install the Application
Open File Explorer and navigate to the folder where you extracted the ZIP file.

Inside the extracted folder, locate and open the SystemHealthInstaller folder.

Navigate to the Debug folder inside SystemHealthInstaller.

In the Debug folder, locate and double-click setup.exe to start the installation process.

Run the Application
After installation, the app will automatically sit in your System Tray, monitoring and logging your system’s health metrics. You can right-click the tray icon for options like showing the metrics window or exiting the application.
How to Use the Application
Once the application is running, it will continuously monitor and log your system’s health metrics in the background. The data can be accessed via the system tray icon or by opening the metrics window.

Key Metrics Displayed:
CPU Usage: Percentage of CPU being used.

RAM Usage: Amount of RAM in use (in MB).

Storage Usage: Percentage of used storage on your primary drive (usually C:).

CPU Temperature: Current temperature of your CPU (in Celsius).

Disk Temperature: Current temperature of your primary disk (in Celsius).

Exporting Logs
You can export the logged data into a CSV file for analysis:

Right-click the tray icon and select Export to CSV to generate the CSV file.

The exported file will be saved to the logs directory.

Dynamic GUI Adjustments
The UI elements, including the Export to CSV button, are dynamically positioned below all visible system metrics to ensure a clean and intuitive layout. The Export to CSV button will automatically appear in the correct position once the data metrics are fully loaded, ensuring that the user interface is always appropriately adjusted.

On startup, the application window is transparent and hidden, waiting for the necessary data (CPU, RAM, temperature metrics) to load.

Once the data is available, the application becomes visible and adjusts the size and layout based on which metrics are displayed.

The Export to CSV button is repositioned below all visible metrics, ensuring that it is easily accessible and not overlapping any data points.

Troubleshooting
Cannot Find the setup.exe: If you cannot find the setup.exe file after extracting the ZIP, make sure you've extracted all files correctly and that no files were blocked by your antivirus.

App Not Running in Tray: If the app does not show up in the system tray after installation, ensure that it's not being blocked by antivirus software or Windows Defender.

Export Button Behind Data: If the "Export to CSV" button is appearing behind other data elements, ensure that the system metrics are fully loaded before the UI is displayed. The dynamic layout logic ensures the button appears below all visible controls.

License
This project is licensed under the MIT License.
