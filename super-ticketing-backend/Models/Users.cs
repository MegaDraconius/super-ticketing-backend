using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class Users
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("CountryId")]
    [JsonPropertyName("CountryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CountryId { get; set; }
    
    [BsonElement("Email")]
    [JsonPropertyName("Email")]
    public string UserEmail { get; set; }
    
    [BsonElement("pwd")]
    [JsonPropertyName("pwd")]
    public string Pwd { get; set; }
    
    [BsonElement("Role")]
    [JsonPropertyName("Role")]
    public string Role { get; set; }
}
