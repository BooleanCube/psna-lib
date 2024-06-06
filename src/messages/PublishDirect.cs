using System.Net;
using System.Net.Sockets;

namespace psna_lib.messages;

public class PublishDirect : Message
{
    private IPEndPoint _authorEndPoint;
    
    private bool _isBroadcast;

    public PublishDirect(byte[] buffer, IPEndPoint authorEndPoint, NetworkServer server)
    {
        Server = server;
        Buffer = buffer;
        MessageTypeName = "Direct Publish To Subscribers";
        GetFormatHelp = "add stuff later";

        AUTHOR = authorEndPoint;
    }

    public IPEndPoint AUTHOR
    {
        get { return this._authorEndPoint; }
        set { this._authorEndPoint = value; }
    }

    public override bool RunAction()
    {
        foreach (IPEndPoint subscriber in Server.GetSubscribers(AUTHOR))
        {
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