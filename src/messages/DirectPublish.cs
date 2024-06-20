using System.Net;
using System.Net.Sockets;
using psna_lib.structs;
using psna_lib.utils;

namespace psna_lib.messages;

public class DirectPublish : Message
{
    private int _bufferSize;
    private byte[] _buffer;

    private bool _isBroadcast = false;

    private DirectMessage directMessage;

    public DirectPublish()
    {
        Buffer = [];
    }

    // Constructor Template
    public DirectPublish(byte[] buffer, NetworkServer server, IPEndPoint authorEndPoint)
    {
        Server = server;
        Buffer = buffer;

        Author = authorEndPoint;
    }
    
    public virtual bool ParseMessage()
    {
        try
        {
            directMessage = ContentHelper.DecryptContent<DirectMessage>(
                ContentHelper.CopyFrom(Buffer, 2)
            );
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public virtual bool RunAction()
    {
        try
        {
            byte[] buffer = ContentHelper.MarkBuffer(directMessage.Buffer, Buffer[0], Buffer[1]);
            
            CheckForBroadcast(directMessage.Recipient.Address);
            Server.OpenSocket.SendTo(buffer, 0, buffer.Length, SocketFlags.None, directMessage.Recipient);
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
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