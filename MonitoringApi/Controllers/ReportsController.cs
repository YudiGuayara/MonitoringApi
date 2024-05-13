using Microsoft.AspNetCore.Mvc;
using MonitoringApi.Models;
using MonitoringApi.Services;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet]
        public async Task<List<Report>> GetAllReports()
        {
            var reports = await _reportsService.GetAllReportsAsync();
            return reports;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Report>> GetReportById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var report = await _reportsService.GetReportByIdAsync(objectId);
            if (report == null)
            {
                return NotFound();
            }
            return report;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Report newReport)
        {
            // No es necesario definir el Id aquí
            // MongoDB generará automáticamente un ObjectId para este campo
            await _reportsService.CreateReportAsync(newReport);
            
            // Retorna la respuesta con el nuevo reporte creado
            return CreatedAtAction(nameof(GetReportById), new { id = newReport.Id }, newReport);
        }



        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateReport(string id, Report updatedReport)
        {
            // Verificar si el id es válido
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            // Obtener el reporte existente por su id
            var existingReport = await _reportsService.GetReportByIdAsync(objectId);
            if (existingReport == null)
            {
                return NotFound("Report not found.");
            }

            // Actualizar solo los campos necesarios del reporte existente
            existingReport.Date = updatedReport.Date;
            existingReport.Observation = updatedReport.Observation;
            existingReport.UserId = updatedReport.UserId;
            existingReport.MeasurementId = updatedReport.MeasurementId;
            existingReport.AlertId = updatedReport.AlertId;

            // Actualizar el reporte existente en la base de datos
            await _reportsService.UpdateReportAsync(objectId, existingReport);

            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> RemoveReport(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            await _reportsService.RemoveReportAsync(objectId);
            return NoContent();
}
    }
}