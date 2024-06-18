using System.Net;
using System.Net.Sockets;

namespace psna_lib.messages;

public class NetworkPublish : Message
{
    private bool _isBroadcast;

    public NetworkPublish(byte[] buffer, NetworkServer server, IPEndPoint authorEndPoint)
    {
        Server = server;
        Buffer = buffer;

        Author = authorEndPoint;
    }

    public byte TOPIC
    {
        get { return Buffer[1]; }
    }

    public override bool RunAction()
    {
        foreach (IPEndPoint subscriber in Server.GetSubscribers(TOPIC))
        {
            if (subscriber.Equals(Author)) continue;
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