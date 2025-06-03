import React, { createContext, useContext, useEffect, useRef, useState } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

// ✅ 1. Context erzeugen
export const SignalRContext = createContext(null);

// ✅ 2. Provider-Component
export const SignalRProvider = ({ children }) => {
    const connectionRef = useRef(null);
    const [connected, setConnected] = useState(false);
    const [clients, setClients] = useState([]);

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            //.withUrl("https://localhost:44399/notificationHub")
            .withUrl("https://aleksssandra-001-site1.anytempurl.com/notificationHub")
            .withAutomaticReconnect()
            .build();

        connectionRef.current = connection;

        connection
            .start()
            .then(async () => {
                setConnected(true);
                //await connection.send("RegisterClient", "ReactClient");

                connection.on("ClientListUpdated", (clientList) => {
                    setClients(clientList);
                });
            })
            .catch(() => setConnected(false));


        return () => connection.stop();
    }, []);

    return (
        <SignalRContext.Provider value={{ connection: connectionRef.current, connected, clients }}>
            {children}
        </SignalRContext.Provider>
    );
};

// ✅ 3. Custom Hook zur Verwendung
export const useSignalR = () => useContext(SignalRContext);
