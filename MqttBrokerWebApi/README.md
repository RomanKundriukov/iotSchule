# MqttBrokerWebApi

Dies ist eine kleine ASP.NET Core Web-API, die als zentrale Schnittstelle für IoT-Beispiele dient.  
Ein integrierter SignalR-Hub verteilt Statusänderungen in Echtzeit an alle verbundenen Clients.

## ⚙️ Funktionen

- **HTTP-Endpoint**:  
  `POST /licht` – nimmt einen booleschen Wert (true/false) entgegen und sendet ihn per SignalR an alle verbundenen Clients

- **SignalR-Hub**:  
  erreichbar unter `/notificationHub`

## 🚀 Projekt starten

```bash
dotnet run --project MqttBrokerWebApi
