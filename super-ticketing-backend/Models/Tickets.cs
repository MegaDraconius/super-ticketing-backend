using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace super_ticketing_backend.Models;

public class Tickets
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("TrackingId")]
    [JsonPropertyName("TrackingId")]
    public string TrackingId { get; set; }
    
    [BsonElement("Title")]
    [JsonPropertyName("Title")]
    public string Title { get; set; }
    
    [BsonElement("Description")]
    [JsonPropertyName("Description")]
    public string Description { get; set; }
    
    [BsonElement("ReportDate")]
    [JsonPropertyName("ReportDate")]
    public DateTime ReportDate { get; set; }
    
    [BsonElement("Status")]
    [JsonPropertyName("Status")]
    public string Status { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("Country")]
    [JsonPropertyName("Country")]
    public string Country { get; set; }
    
    [BsonElement("Priority")]
    [JsonPropertyName("Priority")]
    public string Priority { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("UserId")]
    [JsonPropertyName("UserId")]
    public string UserId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("ITEmployees")]
    [JsonPropertyName("ITEmployees")]
    public string ITEmployees { get; set; }
}