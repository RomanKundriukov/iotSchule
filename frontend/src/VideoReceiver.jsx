import React, { useEffect, useRef } from 'react';
import Peer from 'simple-peer';
import { useSignalR } from './hooks/SignalRContext.jsx';

export default function VideoReceiver() {
    const videoRef = useRef(null);
    const { connection, connected } = useSignalR();
    const peerRef = useRef(null);

    useEffect(() => {
        if (!connected || !connection) return;

        peerRef.current = new Peer({ trickle: false });

        peerRef.current.on('signal', data => {
            connection.invoke('SendOffer', JSON.stringify(data));
        });

        connection.on('ReceiveOffer', async offer => {
            peerRef.current!.signal(JSON.parse(offer));
        });

        peerRef.current.on('stream', stream => {
            if (videoRef.current) {
                videoRef.current.srcObject = stream;
            }
        });

        return () => {
            peerRef.current?.destroy();
        };
    }, [connected, connection]);

    return <video ref={videoRef} autoPlay playsInline style={{ width: '100%' }} />;
}
