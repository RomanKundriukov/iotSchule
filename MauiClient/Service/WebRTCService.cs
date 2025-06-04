using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using SIPSorceryMedia.FFmpeg;

namespace MauiClient.Service
{
    public class WebRTCService
    {
        private RTCPeerConnection? _peerConnection;
        private MediaStreamTrack? _videoTrack;

        public event Action<string>? OnSignal;

        public void Initialize()
        {
            var config = new RTCConfiguration
            {
                iceServers = new List<RTCIceServer>
                {
                    new RTCIceServer { urls = "stun:stun.l.google.com:19302" }
                }
            };
            _peerConnection = new RTCPeerConnection(config);

            _peerConnection.OnIceCandidate += candidate =>
            {
                if(candidate != null)
                {
                    OnSignal?.Invoke(candidate.toJSON());
                }
            };
        }

        public void AddVideoFile(string filePath)
        {
            if (_peerConnection == null)
                Initialize();

            var videoSource = new VideoFileSource(filePath);
            _videoTrack = new MediaStreamTrack(videoSource.GetVideoSourceFormats(), MediaStreamStatusEnum.SendOnly);
            _peerConnection!.addTrack(_videoTrack);
        }

        public async Task<string> CreateOfferAsync()
        {
            if (_peerConnection == null)
                Initialize();

            var offer = await _peerConnection!.createOffer(null);
            await _peerConnection.setLocalDescription(offer);
            return offer.sdp;
        }

        public async Task StartConnectionAsync()
        {
            var offer = await CreateOfferAsync();
            OnSignal?.Invoke(offer);
        }

        public async Task SetRemoteDescriptionAsync(string sdp)
        {
            if (_peerConnection == null)
                Initialize();

            var desc = SDP.ParseSDPDescription(sdp);
            var type = desc.type == "offer" ? RTCSdpType.offer : RTCSdpType.answer;
            var remoteDesc = new RTCSessionDescription { sdp = sdp, type = type };
            await _peerConnection!.setRemoteDescription(remoteDesc);
        }

        public async Task<string> CreateAnswerAsync()
        {
            if(_peerConnection == null)
                throw new InvalidOperationException("Peer connection not initialised");
            var answer = await _peerConnection.createAnswer(null);
            await _peerConnection.setLocalDescription(answer);
            return answer.sdp;
        }
    }
}
