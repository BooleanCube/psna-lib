using System.Net;
using System.Net.Sockets;

namespace psna_lib.messages;

public class NetworkPublish : Message
{
    private bool _isBroadcast;
    private IPEndPoint _authorEndPoint;

    public NetworkPublish(byte[] buffer, NetworkServer server, IPEndPoint authorEndPoint)
    {
        Server = server;
        Buffer = buffer;

        AUTHOR = authorEndPoint;
    }

    public byte TOPIC
    {
        get { return Buffer[1]; }
    }

    public IPEndPoint AUTHOR
    {
        get { return _authorEndPoint; }
        set { _authorEndPoint = value; }
    }

    public override bool RunAction()
    {
        foreach (IPEndPoint subscriber in Server.GetSubscribers(TOPIC))
        {
            if (subscriber.Equals(AUTHOR)) continue;
            CheckForBroadcast(subscriber.Address);
            Server.OpenSocket.SendTo(Buffer, 0, Bytes, SocketFlags.None, subscriber);
        }
        
        return true;
    }

    private void CheckForBroadcast(IPAddress ipAddress)
    {
        if (!_isBroadcast && ipAddress.Equals(IPAddress.Broadcast))
        {
            _isBroadcast = true;
            Server.OpenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
        }
    }
}