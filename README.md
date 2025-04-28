# System Health Monitor

Real-time Windows system monitoring application with cloud-based API integration, live Redis health tracking, automatic alerts, and historical logging.

---

## 📖 About the Project

**System Health Monitor** tracks live system metrics including CPU usage, RAM usage, storage utilization, and hardware temperatures.  
It securely transmits data to a cloud backend for historical storage and alerting.  
Built for IT teams, infrastructure monitoring, and proactive maintenance.

---

## 📋 Key Features

- 📊 Real-time system metrics display (CPU, RAM, Storage, Temperatures)
- ☁️ Cloud API integration (Flask + PostgreSQL backend on Heroku)
- 🔥 Redis-based online status tracking
- 🖥️ System Tray operation (minimize quietly to background)
- 📄 Local logging of system metrics (JSON and optional CSV)
- ✉️ Email alerts for high CPU, RAM, or Storage usage
- 🚀 Scalable architecture for future multi-device fleet monitoring

---

## ⚙️ System Requirements

- Windows 10 or later (64-bit)
- .NET Framework 4.7.2 or later
- Internet connection for cloud sync
- PostgreSQL database access (Heroku or self-hosted)
- Redis Cloud (optional, for live status tracking)

---

## 📦 Installation Instructions

### 1. Download and Extract
- Download the provided ZIP archive (`SystemTrayLogger-Alpha.zip` or newer).
- Extract all files to a directory of your choice.

### 2. Run the Application
- Launch `SystemTrayLogger.exe` (no separate installer required).
- The app will appear in the system tray.

---

## 🚀 First-Time Setup

1. **Allow Windows Firewall access** if prompted.
2. **Watch the Redis Status** — it will show "Connected ✅" if successful.
3. **Metrics will begin logging automatically** every few seconds.
4. **API POSTs** will start transmitting to the server backend.

---

## 🖥️ Using the Application

- **Double-click the Tray Icon** to open the app window.
- **Minimize** to return to quiet background operation.
- **Monitor live metrics**:
  - CPU Usage (%)
  - RAM Usage (% and GB)
  - Storage Usage (% and GB)
  - CPU Temperature (°C) *(where available)*
  - Disk Temperature (°C) *(where available)*

> *Note: Temperatures are hidden if hardware sensors are unavailable.*

---

## 🛠️ Troubleshooting

| Problem | Solution |
|:--------|:---------|
| Metrics not updating | Ensure internet connection and open firewall |
| Redis Status shows "Offline ⚠️" | Check cloud Redis credentials or server availability |
| Temperature shows 0°C | Some hardware does not expose temperature sensors |
| App crashes on launch | Verify .NET Framework 4.7.2 or later is installed |

---

## 📈 Expansion Roadmap

- Mobile app notifications for critical alerts
- Cloud dashboard for multi-device visualization
- User authentication and multi-tenant support
- Predictive maintenance analytics
- Enhanced installer (MSI / ClickOnce)

---

## 📧 Contact and Support

**Developer:** Michael Stiles  
**Email:** Stilesm93@gmail.com  
**Project Repository:** [Coming Soon] (GitHub/Portfolio link)

---
.

License
This project is licensed under the MIT License.
