import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { SignalRProvider } from "../src/hooks/SignalRContext.jsx";

createRoot(document.getElementById('root')).render(
    <SignalRProvider>
        <App/>
    </SignalRProvider>,
)
