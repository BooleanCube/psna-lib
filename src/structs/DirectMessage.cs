using System.Net;

namespace psna_lib.structs;

public class DirectMessage
{
    private IPEndPoint _recipientEndPoint;
    private byte[] _buffer;

    public DirectMessage(IPEndPoint recipientEndPoint, byte[] buffer)
    {
        Recipient = recipientEndPoint;
        Buffer = buffer;
    }

    public IPEndPoint Recipient
    {
        get { return _recipientEndPoint; }
        set { _recipientEndPoint = value; }
    }

    public byte[] Buffer
    {
        get { return _buffer; }
        set { _buffer = value; }
    }
}