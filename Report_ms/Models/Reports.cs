using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MonitoringApi.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("observation")]
        public string Observation { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("measurementId")]
        public string MeasurementId { get; set; }

        [BsonElement("alertId")]
        public string AlertId { get; set; }

        public Report()
        {
            Id = ObjectId.GenerateNewId().ToString(); // Genera un ObjectId nuevo al crear el objeto
        }
    }
}
