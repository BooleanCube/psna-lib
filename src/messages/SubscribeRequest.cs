using System.Net;
using System.Text;

namespace psna_lib.messages;

public class SubscribeRequest : Message
{
    private byte _subscription;
    
    public SubscribeRequest(byte[] buffer, NetworkServer server, IPEndPoint authorEndPoint)
    {
        Server = server;
        Buffer = buffer;
        
        Author = authorEndPoint;
    }

    public byte TOPIC
    {
        get { return _subscription; }
        set { _subscription = value; }
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
            return Server.AddSubscriberConnection(Author, TOPIC);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}