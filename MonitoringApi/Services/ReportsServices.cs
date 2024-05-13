using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MonitoringApi.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonitoringApi.Controllers;
using MonitoringApi.Services;
 
namespace MonitoringApi.Services
{
    public class ReportsService
    {
        private readonly IMongoCollection<Report> _reportsCollection;

        public ReportsService(IOptions<ReportsDatabaseSettings> reportsDatabaseSettings)
        {
            var client = new MongoClient(reportsDatabaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(reportsDatabaseSettings.Value.DatabaseName);
            _reportsCollection = database.GetCollection<Report>(reportsDatabaseSettings.Value.ReportsCollectionName);
        }

        public async Task<List<Report>> GetAllReportsAsync() =>
            await _reportsCollection.Find(_ => true).ToListAsync();

        public async Task<Report> GetReportByIdAsync(ObjectId id) =>
            await _reportsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task CreateReportAsync(Report newReport) =>
            await _reportsCollection.InsertOneAsync(newReport);

        public async Task UpdateReportAsync(ObjectId id, Report updatedReport) =>
            await _reportsCollection.ReplaceOneAsync(r => r.Id == id, updatedReport);

        public async Task RemoveReportAsync(ObjectId id) =>
            await _reportsCollection.DeleteOneAsync(r => r.Id == id);
    

        // Método para verificar el estado de la conexión e imprimir un mensaje
        public void PrintConnectionStatus()
        {
            try
            {
                _reportsCollection.Find(_ => false).FirstOrDefault();
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
            catch (Exception)
            {
                Console.WriteLine("Error al conectar a la base de datos.");
            }
        }
    }
}
