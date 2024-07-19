using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class Country
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("CountryCode")]
    [JsonPropertyName("CountryCode")]
    public string CountryCode { get; set; }
    
    [BsonElement("CountryName")]
    [JsonPropertyName("CountryName")]
    public string CountryName { get; set; }
    
    [BsonElement("CountryMainLanguage")]
    [JsonPropertyName("CountryMainLanguage")]
    public string CountryMainLanguage { get; set; }
}