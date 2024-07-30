using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class TicketStatus
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("StatusValue")]
    [JsonPropertyName("StatusValue")]
    public string StatusValue { get; set; }
}