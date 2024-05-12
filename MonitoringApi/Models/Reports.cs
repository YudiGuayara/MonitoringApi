using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MonitoringApi.Models{
public class Report
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } // ObjectId es el tipo de dato para el ID en MongoDB
 
    public DateTime Date { get; set; }
    public string? Observation { get; set; }
    public string? UserId { get; set; } // ID del usuario relacionado
    public string? MeasurementId { get; set; } // ID de la medición relacionada
    public string?  AlertId { get; set; } // ID de la alerta relacionada
} }