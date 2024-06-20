// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text.Json;
using psna_lib.structs;

// serialization and deserialization testing
DirectMessage dm = new DirectMessage(new IPEndPoint(IPAddress.Any, 1024), new byte[] { 1, 2, 3, 4 });

Console.WriteLine(dm.Recipient);
string jsonString = JsonSerializer.Serialize(dm);

Console.WriteLine(jsonString);

DirectMessage deserialized = JsonSerializer.Deserialize<DirectMessage>(jsonString);
Console.WriteLine(deserialized.Recipient);

/*
 * TODO: 
 * Finish ReceiveMessage() in Network Server to recognize message format type
 * Finish Serialization and Deserialization of Transform structs using Google Protobuf
 * Finish enter NetworkClient structure
 * Write Message descriptions
 */
 
 
// Client-Server Message Architecture:
// - Network Publish
// - Publish Direct (communicates through server)
// - Subscribe Request
// - Unsubscribe Request

// For Network Publish and Direct Publish, create a secondary message struct that stores authors endpoint
// so subscribers can respond.

// Client-Client Message Architecture
// - Publish Secret (doesn't communicate through server)

// Topic Architecture
// - Author End Point
// - Struct Content
// - Topic ID

// Useful Struct Ideas
// - Transform (done)
// - Vector3 (done)
// - Quaternion (done)
// - IRLPosition

// Utils Functions
// - Transform to IRLPosition Converter
// - Topic to Buffer (Appending mId and tId to prefix of protocol buffer (done)
// - Buffer to Topic (done)

// make a separate file to manage threading