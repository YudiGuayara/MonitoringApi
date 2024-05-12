using MongoDB.Driver;
using MonitoringApi.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringApi.Services
{
    public class ReportsService
    {
        private readonly IMongoCollection<Report> _reportsCollection;
        private readonly string _connectionString;

        public ReportsService(IOptions<ReportsDatabaseSettings> reportsDatabaseSettings)
        {
            _connectionString = reportsDatabaseSettings.Value.ConnectionString;

            var mongoClient = new MongoClient(_connectionString);
            var mongoDatabase = mongoClient.GetDatabase(reportsDatabaseSettings.Value.DatabaseName);
            _reportsCollection = mongoDatabase.GetCollection<Report>(reportsDatabaseSettings.Value.ReportsCollectionName);
        }

        public async Task<List<Report>> GetAllReportsAsync() =>
            await _reportsCollection.Find(_ => true).ToListAsync();

        public async Task<Report?> GetReportByIdAsync(string id) =>
            await _reportsCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task CreateReportAsync(Report newReport) =>
            await _reportsCollection.InsertOneAsync(newReport);

        public async Task UpdateReportAsync(string id, Report updatedReport) =>
            await _reportsCollection.ReplaceOneAsync(r => r.Id == id, updatedReport);

        public async Task RemoveReportAsync(string id) =>
            await _reportsCollection.DeleteOneAsync(r => r.Id == id);

        // Método para verificar el estado de la conexión e imprimir un mensaje
        public void PrintConnectionStatus()
        {
            try
            {
                var client = new MongoClient(_connectionString);
                client.ListDatabaseNames(); // Intenta listar los nombres de las bases de datos
                Console.WriteLine("Conexión exitosa a la base de datos.");
            }
            catch (Exception)
            {
                Console.WriteLine("Error al conectar a la base de datos.");
            }
        }
    }
}
