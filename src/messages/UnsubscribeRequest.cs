using System.Net;
using System.Text;

namespace psna_lib.messages;

public class UnsubscribeRequest : Message
{
    private IPEndPoint _authorEndPoint;
    private IPEndPoint _subscriberEndPoint;
    
    
    public UnsubscribeRequest(byte[] buffer, IPEndPoint subscriberEndPoint, NetworkServer server)
    {
        Buffer = buffer;
        MessageTypeName = "Unsubscribe Request Message";
        GetFormatHelp = "stuff to fill out";
        
        SUBSCRIBER = subscriberEndPoint;
    }

    public IPEndPoint AUTHOR
    {
        get { return _authorEndPoint; }
        set { _authorEndPoint = value;  }
    }

    public IPEndPoint SUBSCRIBER
    {
        get { return _subscriberEndPoint; }
        set { _subscriberEndPoint = value; }
    }

    public override bool ParseMessage()
    {
        try
        {
            string message = Encoding.ASCII.GetString(Buffer);
            AUTHOR = IPEndPoint.Parse(message.Remove(0, 1));

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public override bool RunAction()
    {
        try
        {
            Server.RemoveSubscriberConnection(AUTHOR, SUBSCRIBER);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}