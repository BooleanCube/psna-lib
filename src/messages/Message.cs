namespace psna_lib.messages;

public class Message
{
    private int _bufferSize;
    private byte[] _buffer = [];
    private NetworkServer _server;

    private static string _typeName;
    private static string _formatHelp;
    
    public Message() {}

    // Constructor Template
    public Message(byte[] buffer, NetworkServer server)
    {
        Server = server;
        Buffer = buffer;
        MessageTypeName = "Message Type Name";
        GetFormatHelp = "SERVER MESSAGE HELP =>\n \n" +
                        "Message Type Specification (required):\n" +
                        "Specify the message type to the server within the buffer stream, by allotting the first " +
                        "byte of your buffer stream (at index 0), to the corresponding message type. For Example:\n" +
                        "byte[] buffer; // loaded with your message content\n" +
                        "buffer[0]; // contains the byte value corresponding to one of the applicable message types.";
        // TODO add message types from static array
        // TODO add each message type format help to the end of the string wid a loop
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

    public string GetFormatHelp
    {
        get { return _formatHelp; }
        set { _formatHelp = value; }
    }

    public string MessageTypeName
    {
        get { return _typeName;  }
        set { _typeName = value; }
    }

    public NetworkServer Server
    {
        get { return _server; }
        set { this._server = value; }
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