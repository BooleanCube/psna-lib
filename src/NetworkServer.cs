using System.Numerics;
using psna_lib.messages;

namespace psna_lib;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetworkServer
{
    private HashSet<IPEndPoint>[] _topicToClients;
    private Socket _oSocket;
    private IPEndPoint _localEP;

    private Thread _scheduledThread;
    private bool _runThread;

    private static readonly int _maxBufferSize = 0x10000;
    private byte[] _gBuffer = new byte[_maxBufferSize];

    public NetworkServer() : this(new IPEndPoint(IPAddress.Any, 11000)) {}

    public NetworkServer(short port) : this(new IPEndPoint(IPAddress.Any, port)) {}

    public NetworkServer(IPEndPoint endPoint)
    {
        _topicToClients = new HashSet<IPEndPoint>[256];
        _oSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        _localEP = endPoint; _oSocket.Bind(_localEP);
        
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

    public IPEndPoint ServerEndPoint
    {
        get { return _localEP; }
        set { _localEP = value; _oSocket.Bind(_localEP); }
    }

    public Thread ServerThread
    {
        get { return _scheduledThread; }
        set { _scheduledThread = value;  }
    }

    public void Run()
    {
        _runThread = true;
        ServerThread.Start();
    }

    public void Stop()
    {
        try
        {
            _runThread = false;
            ServerThread.Abort();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public void Close() {}

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

        Message message;

        switch (_gBuffer[0])
        {
            case ID.m_SUBSCRIBE_REQUEST:
                message = new SubscribeRequest(_gBuffer, this, authorEndPoint);
                break;
            case ID.m_UNSUBSCRIBE_REQUEST:
                message = new UnsubscribeRequest(_gBuffer, this, authorEndPoint);
                break;
            case ID.m_NETWORK_PUBLISH:
                message = new NetworkPublish(_gBuffer, this, authorEndPoint);
                break;
            case ID.m_DIRECT_PUBLISH:
                message = new DirectPublish(_gBuffer, this, authorEndPoint);
                break;
            default:
                return;
        }

        if(!message.ParseMessage()) return;
        if(!message.RunAction()) return;

        if (receivedBufferSize < _maxBufferSize)
        {
            byte[] buffer = new byte[receivedBufferSize];
            // TODO double check if this doesn't work
            Buffer.BlockCopy(_gBuffer, 1, buffer, 0, receivedBufferSize);
        }
    }

    public bool AddSubscriberConnection(IPEndPoint client, byte topicId)
    {
        return _topicToClients[topicId].Add(client);
    }

    public bool RemoveSubscriberConnection(IPEndPoint client, byte topicId)
    {
        return _topicToClients[topicId].Remove(client);
    }

    public HashSet<IPEndPoint> GetSubscribers(byte topicId)
    {
        return _topicToClients[topicId];
    }

    public bool IsSubscribed(IPEndPoint client, byte topicId)
    {
        return _topicToClients[topicId].Contains(client);
    }
}