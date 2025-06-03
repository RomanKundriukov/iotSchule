import * as React from 'react';
import {useState} from 'react';
import Switch from '@mui/material/Switch';
import { useSignalR } from "../../hooks/SignalRContext.jsx";
import cl from './DefaultSwitch.module.scss'

export default function DefaultSwitch({item}) {
    const [checked, setChecked] = useState(false);
    const { connection, connected } = useSignalR();

    const handleChange = async (event) => {
        const newState = event.target.checked;
        setChecked(newState);

        if (connected && connection) {
            try {
                if(item === 'licht')
                {
                    await connection.send("UpdateLichtState", newState);
                }
                console.log("🔦 Licht gesendet:", newState);
            } catch (err) {
                console.error("❌ Fehler beim Senden:", err);
            }
        }
    };

    return (
        <div className={cl.boxShadow}>
            <p>Licht: {checked ? "AN" : "AUS"}</p>
            <Switch checked={checked} onChange={handleChange} disabled={!connected} />
        </div>
    );
}
