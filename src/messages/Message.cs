using System.Net;

namespace psna_lib.messages;

public abstract class Message
{
    private int _bufferSize;
    private byte[] _buffer = [];
    private NetworkServer _server;
    private IPEndPoint _authorEndPoint;
    
    public Message() {}

    // Constructor Template
    public Message(byte[] buffer, NetworkServer server)
    {
        Server = server;
        Buffer = buffer;
    }

    public int Bytes
    {
        get { return _bufferSize; }
        set { _bufferSize = value; }
    }

    public byte[] Buffer
    {
        get { return _buffer; }
        set { _buffer = value; Bytes = _buffer.Length; }
    }

    public NetworkServer Server
    {
        get { return _server; }
        set { this._server = value; }
    }
    
    public IPEndPoint Author
    {
        get { return _authorEndPoint; }
        set { _authorEndPoint = value; }
    }
    
    // Parse Message Function Template
    public virtual bool ParseMessage()
    {
        
        try
        {
            // attempt to parse the buffer for the corresponding message format
            
            // successfully parsed the message
            return true;
        }
        catch (Exception e)
        {
            /*
             * returns false if there was an error parsing message
             * due to wrong message type format, or parsing issues, etc.
             * 
             * signals to send a help message.
             */
            return false;
        }
    }

    // Run Action Function Template
    public virtual bool RunAction()
    {
        try
        {
            // commands to run after parsing the message

            // successfully ran the action attached to the corresponding message type
            // signals to send a success message
            return true;
        }
        catch (Exception e)
        {
            /*
             * returns false if there was an error running the action
             * possibly network connection issues, internal server issues, etc.
             * 
             * signals to send a action failed message.
             */
            return false;
        }
    }
}