using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace psna_lib
{
    public class NetworkServer
    {
        private Dictionary<string, List<IPEndPoint>> _authorToSubscribers;
        private Socket _oSocket;
        private IPEndPoint _localEP;

        private Timer _scheduledExecutorService;

        private static int _maxBufferSize = 0x10000;
        private byte[] _gBuffer = new byte[_maxBufferSize];

        public NetworkServer() : this(new IPEndPoint(IPAddress.Any, 11000)) {}

        public NetworkServer(short port) : this(new IPEndPoint(IPAddress.Any, port)) {}

        public NetworkServer(IPEndPoint endPoint)
        {
            _authorToSubscribers = new Dictionary<string, List<IPEndPoint>>();
            _oSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _localEP = endPoint;
            OpenSocket.Bind(_localEP);
        }

        public Socket OpenSocket
        {
            get { return _oSocket; }
            set { _oSocket = value; }
        }

        public IPEndPoint ServerEndPoint
        {
            get { return _localEP; }
            set
            {
                _localEP = value;
                _oSocket.Bind(_localEP);
            }
        }

        public void Run()
        {
            _scheduledExecutorService = new Timer(ReadMessage, null, 100, 1);
        }

        public void Stop()
        {
            _scheduledExecutorService.Dispose();
            _scheduledExecutorService = null;
        }
        public void Close() {}

        private void ReadMessage(Object state)
        {
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            OpenSocket.BeginReceiveFrom(_gBuffer, 0, _maxBufferSize, SocketFlags.None, ref remoteEP,
                ParseMessage, null);
        }

        private void ParseMessage(IAsyncResult result)
        {
            EndPoint tempRemoteEP = new IPEndPoint(IPAddress.Any, 0);
            int bufferReceivedSize = OpenSocket.EndReceiveFrom(result, ref tempRemoteEP);
            IPEndPoint authorEndPoint = (IPEndPoint)tempRemoteEP;

            if (bufferReceivedSize < 2) return;

            switch (_gBuffer[0])
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    break;
            }

            if (bufferReceivedSize < _maxBufferSize)
            {
                byte[] buffer = new byte[bufferReceivedSize];
                // TODO double check if this doesn't work
                Buffer.BlockCopy(_gBuffer, 1, buffer, 0, bufferReceivedSize);
            }
        }
    }
}