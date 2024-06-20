using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using psna_lib.messages;
using psna_lib.structs;
using psna_lib.utils;

namespace psna_lib;

public class NetworkClient
{
    private Socket _oSocket;
    private IPEndPoint _localEP, _serverEP;
    private bool _isBroadcast = false;

    private Thread _scheduledThread;
    private bool _runThread;

    private static readonly int _maxBufferSize = 0x10000;
    private byte[] _gBuffer = new byte[_maxBufferSize];

    public NetworkClient(IPEndPoint serverEndPoint) : this(11000, serverEndPoint) {}

    public NetworkClient(short port, IPEndPoint serverEndPoint) : this(new IPEndPoint(IPAddress.Any, port), serverEndPoint) {}

    public NetworkClient(IPEndPoint localEndPoint, IPEndPoint serverEndPoint)
    {
        _oSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        _localEP = localEndPoint; _oSocket.Bind(_localEP);
        _serverEP = serverEndPoint;
        
        _runThread = false;
        _scheduledThread = new Thread(() =>
        {
            while (_runThread)
            {
                ReadMessage();
                Thread.Sleep(10);
            }
        });
    }

    public Socket OpenSocket
    {
        get { return _oSocket; }
        set { _oSocket = value; }
    }

    public IPEndPoint ClientEndPoint
    {
        get { return _localEP; }
        set { _localEP = value; _oSocket.Bind(_localEP); }
    }

    public IPEndPoint ServerEndPoint
    {
        get { return _serverEP; }
        set { _serverEP = value; _oSocket.Bind(_serverEP); }
    }

    public Thread ClientThread
    {
        get { return _scheduledThread; }
        set { _scheduledThread = value;  }
    }

    public void Run()
    {
        _runThread = true;
        ClientThread.Start();
    }

    public void Stop()
    {
        try
        {
            _runThread = false;
            ClientThread.Abort();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public void Close() {}
    
    private void CheckForBroadcast(IPAddress ipAddress)
    {
        if (!_isBroadcast && ipAddress.Equals(IPAddress.Broadcast))
        {
            _isBroadcast = true;
            OpenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        }
    }

    private void SendMessage(byte[] buffer)
    {
        CheckForBroadcast(ServerEndPoint.Address);
        OpenSocket.SendTo(buffer, 0, buffer.Length, SocketFlags.None, ServerEndPoint);
    }

    public void SubscribeRequestion(byte topicId)
    {
        byte[] buffer = new byte[2];
        buffer[0] = ID.m_SUBSCRIBE_REQUEST; // Message ID
        buffer[1] = topicId; // Topic ID
        
        SendMessage(buffer);
    }
    
    public void UnsubscribeRequestion(byte topicId)
    {
        byte[] buffer = new byte[2];
        buffer[0] = ID.m_UNSUBSCRIBE_REQUEST; // Message ID
        buffer[1] = topicId; // Topic ID
        
        SendMessage(buffer);
    }
    
    public void NetworkPublishMessage(byte topicId, dynamic value)
    {
        byte[] encodedStruct = ContentHelper.EncryptContent(value);

        byte[] buffer = ContentHelper.MarkBuffer(encodedStruct, ID.m_NETWORK_PUBLISH, topicId);
        
        SendMessage(buffer);
    }
    
    public void DirectPublishMessage(byte topicId, dynamic value, IPEndPoint recipient)
    {
        byte[] encodedValue = ContentHelper.EncryptContent(value);
        DirectMessage dm = new DirectMessage(recipient, encodedValue);
        byte[] encodedStruct = ContentHelper.EncryptContent(dm);

        byte[] buffer = ContentHelper.MarkBuffer(encodedStruct, ID.m_DIRECT_PUBLISH, topicId);
        
        SendMessage(buffer);
    }

    private void ReadMessage()
    {
        EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        OpenSocket.BeginReceiveFrom(_gBuffer, 0, _maxBufferSize, SocketFlags.None, ref remoteEP,
            ReceiveMessage, null);
    }

    private void ReceiveMessage(IAsyncResult result)
    {
        EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        int receivedBufferSize = OpenSocket.EndReceiveFrom(result, ref remoteEP);
        IPEndPoint authorEndPoint = (IPEndPoint) remoteEP;

        if (receivedBufferSize < _maxBufferSize)
        {
            byte[] buffer = new byte[receivedBufferSize];
            // TODO double check if this doesn't work
            Buffer.BlockCopy(_gBuffer, 1, buffer, 0, receivedBufferSize);
        }
    }
}