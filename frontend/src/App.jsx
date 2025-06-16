import cl from './App.module.scss';
import {useContext} from "react";
import {SignalRContext} from './hooks/SignalRContext.jsx';
import DefaultSwitch from "./UI/Switch/DefaultSwitch.jsx";


export default function App() {

    const {connected, clients } = useContext(SignalRContext);

    return (
        <div className={cl.container}>
            <div className={cl.row}>
                <div className={cl.col}>
                    <div>
                        <h1 style={{ color: connected ? "green" : "red" }}>Status: {connected ? "Verbunden" : "Nicht verbunden"}</h1>
                    </div>
                    <div>
                        <h1>Verbundene Geräte:</h1>
                        <ul>
                            {clients && Object.keys(clients).length > 0 ? (
                                Object.values(clients).map((client, index) => <li key={index}>{client}</li>)
                            ) : (
                                <li>Keine Geräte verbunden</li>
                            )}
                        </ul>
                    </div>
                </div>


            </div>
            <div className={cl.row}>
                <DefaultSwitch/>
            </div>
        </div>
    );
}
