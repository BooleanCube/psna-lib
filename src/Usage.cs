// See https://aka.ms/new-console-template for more information

using psna_lib;

Console.WriteLine("Hello, World!");

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

// Client-Client Message Architecture
// - Publish Secret (doesn't communicate through server)

// Topic Architecture
// - Author End Point
// - Custom Structs
// - Valuable Data
// - Topic ID

// Useful Struct Ideas
// - Transform
// - IRLPosition

// Utils Functions
// - Transform to IRLPosition Converter
// - Topic to Buffer (Appending mId and tId to prefix of protocol buffer
// - Buffer to Topic