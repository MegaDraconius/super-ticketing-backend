using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class ITGuys
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("ItGuyName")]
    [JsonPropertyName("ItGuyName")]
    public string ItGuyName { get; set; }
    
    [BsonElement("Surname")]
    [JsonPropertyName("Surname")]
    public string Surname { get; set; }
    
    [BsonElement("Pwd")]
    [JsonPropertyName("Pwd")]
    public string Pwd { get; set; }
    
    [BsonElement("Role")]
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    
    [BsonElement("CountryId")]
    [JsonPropertyName("CountryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CountryId { get; set; }
    
    [BsonElement("email")]
    [JsonPropertyName("email")]
    public string ItGuyEmail { get; set; }
       
}