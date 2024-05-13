using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MonitoringApi.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public ObjectId Id { get; set; } // Utiliza ObjectId para representar el _id en MongoDB

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("observation")]
        public string? Observation { get; set; }

        [BsonElement("userId")]
        public string? UserId { get; set; }

        [BsonElement("measurementId")]
        public string? MeasurementId { get; set; }

        [BsonElement("alertId")]
        public string? AlertId { get; set; }
    }
}

