using System.Net;

namespace psna_lib.messages;

public class DirectPublish : Message
{
    private int _bufferSize;
    private byte[] _buffer;

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
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}