using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class ITGuys
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("Name")]
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    
    [BsonElement("Surname")]
    [JsonPropertyName("Surname")]
    public string Surname { get; set; }
    
    [BsonElement("Pwd")]
    [JsonPropertyName("Pwd")]
    public string Pwd { get; set; }
    
    [BsonElement("Role")]
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    
    [BsonElement("Country")]
    [JsonPropertyName("Country")]
    public string Country { get; set; }
    
    [BsonElement("email")]
    [JsonPropertyName("email")]
    public string Email { get; set; }
       
}