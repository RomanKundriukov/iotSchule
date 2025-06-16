import React, { createContext, useContext, useEffect, useState, useMemo } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

// 1. Context erzeugen
export const SignalRContext = createContext(null);

// 2. Provider-Component
export const SignalRProvider = ({ children }) => {
    const [connection, setConnection] = useState(null);
    const [connected, setConnected] = useState(false);
    const [clients, setClients] = useState([]);

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl("https://iot-schule.runasp.net/notificationHub")
            .withAutomaticReconnect()
            .build();

        conn.on("ClientListUpdated", (clientList) => {
            setClients(clientList);
        });

        conn.start()
            .then(() => {
                setConnection(conn);
                setConnected(true);
                console.log("SignalR verbunden.");
            })
            .catch((err) => {
                console.error("Verbindung fehlgeschlagen:", err);
                setConnected(false);
            });

        return () => {
            conn.stop().then(() => console.log("SignalR getrennt."));
        };
    }, []);

    const contextValue = useMemo(() => ({
        connection,
        connected,
        clients
    }), [connection, connected, clients]);

    return (
        <SignalRContext.Provider value={contextValue}>
            {children}
        </SignalRContext.Provider>
    );
};

// 3. Custom Hook zur Verwendung
export const useSignalR = () => useContext(SignalRContext);
