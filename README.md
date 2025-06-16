# 📡 IoT Schule – Ein einfaches IoT-Projekt mit .NET, MAUI & React

Dieses Repository demonstriert ein einfaches IoT-Szenario mit Echtzeitkommunikation über SignalR. Es besteht aus drei vernetzten Anwendungen:

- **`MqttBrokerWebApi`** – ASP.NET Core Web-API mit SignalR-Hub zur Zustandsverteilung  
- **`MauiClient`** – .NET MAUI App zur Steuerung der Taschenlampe eines mobilen Geräts  
- **`frontend`** – Ein modernes React-Frontend (Vite) zur Visualisierung und Steuerung

> 👉 Projektseite: [github.com/RomanKundriukov/iotSchule](https://github.com/RomanKundriukov/iotSchule)

## 🧠 Projektidee

Dieses Beispiel zeigt, wie Geräte mithilfe eines zentralen Servers und SignalR in Echtzeit miteinander kommunizieren können. Die Lichtsteuerung dient als Anwendungsbeispiel für Fernbedienung via IoT.

## 🔗 Architektur

```
+-------------+         SignalR         +------------------+         SignalR         +-------------+
| React       |  <--------------------> | ASP.NET Web API  |  <--------------------> | MAUI Client |
| Frontend    |                        | + SignalR-Hub     |                        | (Smartphone)|
+-------------+                        +------------------+                        +-------------+
```

- **Zentrale Kommunikation:** SignalR-Hub in `MqttBrokerWebApi`  
- **Echtzeit-Updates:** UI und Taschenlampe reagieren auf Signaländerungen  
- **Flexibles Setup:** Web- und Mobile-Clients können Zustände steuern und empfangen

## 📁 Projekte im Überblick

### ✅ 1. `MqttBrokerWebApi` – .NET Web API mit SignalR

- Endpoint: `POST /licht` zum Senden von Statusänderungen (z. B. `{ "status": "on" }`)
- Broadcast über SignalR an alle verbundenen Clients

**Starten:**
```bash
dotnet run --project MqttBrokerWebApi
```

### 💡 2. `MauiClient` – MAUI-App für Android/iOS

- Verbindet sich mit dem SignalR-Hub
- Reagiert auf Signale zur Steuerung des Taschenlampen-Status

**Starten (z. B. für Android):**
```bash
dotnet build MauiClient/MauiClient.csproj -t:Run -f net9.0-android
```

### 🌐 3. `frontend` – React + Vite Web-App

- Visualisiert Verbindungsstatus und Geräte
- Steuert das Licht per Button-Klick → Signal wird an den Server gesendet

**Starten:**
```bash
cd frontend
npm install
npm run dev
```

## ⚙️ Voraussetzungen

- [.NET 9 SDK (Preview)](https://dotnet.microsoft.com/)
- [Node.js & npm](https://nodejs.org/)
- Android/iOS-Emulator oder physisches Gerät für MAUI
- Optional: [Visual Studio 2022/2025 mit MAUI & Web-Tools](https://visualstudio.microsoft.com/)

## 🤝 Mitwirkende

- **Roman Kundriukov** – Idee, Umsetzung & Dokumentation  
> 💻 GitHub: [@RomanKundriukov](https://github.com/RomanKundriukov)
