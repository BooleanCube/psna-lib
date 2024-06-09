using System.Net;
using System.Text;

namespace psna_lib.messages;

public class UnsubscribeRequest : Message
{
    private byte _subscription;
    private IPEndPoint _subscriberEndPoint;
    
    
    public UnsubscribeRequest(byte[] buffer, NetworkServer server, IPEndPoint subscriberEndPoint)
    {
        Buffer = buffer;
        
        SUBSCRIBER = subscriberEndPoint;
    }

    public byte TOPIC
    {
        get { return _subscription; }
        set { _subscription = value; }
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
            TOPIC = Buffer[1];

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
            Server.RemoveSubscriberConnection(SUBSCRIBER, TOPIC);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}