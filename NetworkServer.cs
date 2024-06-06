using System.Numerics;

namespace psna_lib;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetworkServer
{
    private static Dictionary<IPEndPoint, HashSet<IPEndPoint>> _authorToSubscribers;
    private Socket _oSocket;
    private IPEndPoint _localEP;

    private Thread _scheduledThread;

    private static readonly int _maxBufferSize = 0x10000;
    private byte[] _gBuffer = new byte[_maxBufferSize];

    public NetworkServer() : this(new IPEndPoint(IPAddress.Any, 11000)) {}

    public NetworkServer(short port) : this(new IPEndPoint(IPAddress.Any, port)) {}

    public NetworkServer(IPEndPoint endPoint)
    {
        _authorToSubscribers = new Dictionary<IPEndPoint, HashSet<IPEndPoint>>();
        _oSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
        _localEP = endPoint;
        OpenSocket.Bind(_localEP);
        
        _scheduledThread = new Thread(() =>
        {
            ReadMessage();
            Thread.Sleep(100);
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
        ServerThread.Start();
    }

    public void Stop()
    {
        try
        {
            ServerThread.Abort();
        }
        catch (ThreadStateException e)
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

        switch (_gBuffer[0])
        {
            case 0:
                break;
            case 1:
                break;
            default:
                break;
        }

        if (receivedBufferSize < _maxBufferSize)
        {
            byte[] buffer = new byte[receivedBufferSize];
            // TODO double check if this doesn't work
            Buffer.BlockCopy(_gBuffer, 1, buffer, 0, receivedBufferSize);
        }
    }

    public static bool AddSubscriberConnection(IPEndPoint author, IPEndPoint subscriber)
    {
        return _authorToSubscribers[author].Add(subscriber);
    }

    public static bool RemoveSubscriberConnection(IPEndPoint author, IPEndPoint subscriber)
    {
        return _authorToSubscribers[author].Remove(subscriber);
    }

    public static HashSet<IPEndPoint> GetSubscribers(IPEndPoint author)
    {
        return _authorToSubscribers[author];
    }
}