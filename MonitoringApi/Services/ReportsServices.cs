using MongoDB.Driver;
using MonitoringApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MonitoringApi.Services
{
    public class ReportsServices
    {
        private readonly IMongoCollection<Report> _reports;

        public ReportsServices(IMongoClient mongoClient, IOptions<ReportsDatabaseSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _reports = database.GetCollection<Report>(settings.Value.ReportsCollectionName);
        }

        public async Task<List<Report>> GetReportsAsync()
        {
            return await _reports.Find(_ => true).ToListAsync();
        }

        public async Task<Report> GetReportByIdAsync(string id)
        {
            return await _reports.Find(report => report.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateReportAsync(Report report)
        {
            await _reports.InsertOneAsync(report);
        }

        public async Task<bool> UpdateReportAsync(string id, Report updatedReport)
        {
            var filter = Builders<Report>.Filter.Eq(r => r.Id, id);
            var update = Builders<Report>.Update
                .Set(r => r.Date, updatedReport.Date)
                .Set(r => r.Observation, updatedReport.Observation)
                .Set(r => r.UserId, updatedReport.UserId)
                .Set(r => r.MeasurementId, updatedReport.MeasurementId)
                .Set(r => r.AlertId, updatedReport.AlertId);

            var result = await _reports.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteReportAsync(string id)
        {
            var result = await _reports.DeleteOneAsync(report => report.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public void PrintConnectionStatus()
        {
            try
            {
                _reports.Find(_ => false).FirstOrDefault();
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
            catch (Exception)
            {
                Console.WriteLine("Error al conectar a la base de datos.");
            }
        }
    }
}
