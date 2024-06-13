using System.Net;

namespace psna_lib.structs;

public class DirectMessage
{
    private IPEndPoint _recipientEndPoint;
    private byte[] _buffer;

    public DirectMessage(IPEndPoint recipientEndPoint, byte[] buffer)
    {
        
    }
}