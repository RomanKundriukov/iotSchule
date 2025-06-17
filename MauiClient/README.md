# MauiClient

Eine .NET MAUI App, die sich mit dem SignalR-Hub der Web-API verbindet und den Status der Taschenlampe auf dem Android-Gerät steuert.

## 🔧 Funktionen

- Verbindet sich beim Start automatisch mit dem SignalR-Hub unter `/notificationHub`
- Reagiert auf `LichtGeaendert`-Events vom Server
- Schaltet die Taschenlampe entsprechend ein oder aus

## 🚀 Projekt starten

Für Android kann die App wie folgt gebaut und gestartet werden:

```bash
dotnet build
dotnet maui run -f net8.0-android
