using System.Net;

namespace psna_lib.messages;

public class PublishDirect : Message
{
    private IPEndPoint _authorEndPoint;
    
    public PublishDirect(byte[] buffer, IPEndPoint authorEndPoint)
    {
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
        foreach (IPEndPoint subscriber in NetworkServer.GetSubscribers(AUTHOR))
        {
            
        }
        return true;
    }

    private void CheckForBroadcast(IPAddress ipAddress)
    {
        // if(NetworkServer)
    }
}