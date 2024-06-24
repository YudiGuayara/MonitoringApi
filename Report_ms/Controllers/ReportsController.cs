using Microsoft.AspNetCore.Mvc;
using MonitoringApi.Models;
using MonitoringApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportsService _reportService;

        public ReportController(ReportsService reportService)
        {
            _reportService = reportService;
        }

        // GET: api/report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            try
            {
                var reports = await _reportService.GetReportsAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error al obtener informes: {ex.Message}");

                // Return an error response with status code 500 (Internal Server Error)
                return StatusCode(500, $"Error al obtener informes. Por favor, inténtalo de nuevo.");
            }
        }

        // GET: api/report/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(string id)
        {
            try
            {
                var report = await _reportService.GetReportByIdAsync(id);
                if (report == null)
                {
                    return NotFound();
                }
                return Ok(report);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error al obtener el informe con ID {id}: {ex.Message}");

                // Return an error response with status code 500 (Internal Server Error)
                return StatusCode(500, $"Error al obtener el informe. Por favor, inténtalo de nuevo.");
            }
        }

        // POST: api/report
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport([FromBody] Report report)
        {
            try
            {
                await _reportService.CreateReportAsync(report);
                return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error al crear informe: {ex.Message}");

                // Return an error response with status code 500 (Internal Server Error)
                return StatusCode(500, $"Error al crear informe. Por favor, inténtalo de nuevo.");
            }
        }

        // PUT: api/report/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(string id, [FromBody] Report updatedReport)
        {
            if (updatedReport == null || updatedReport.Id != id)
            {
                return BadRequest("Invalid report data");
            }

            try
            {
                var result = await _reportService.UpdateReportAsync(id, updatedReport);
                if (!result)
                {
                    return NotFound();
                }
                return Ok(new { Message = "Report updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error al actualizar el informe con ID {id}: {ex.Message}");

                // Return an error response with status code 500 (Internal Server Error)
                return StatusCode(500, $"Error al actualizar el informe. Por favor, inténtalo de nuevo.");
            }
        }

        // DELETE: api/report/{id}
        // DELETE: api/report/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            try {
                var result = await _reportService.DeleteReportAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = "Report not found" });
                }
                return Ok(new { Message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error al eliminar el informe con ID {id}: {ex.Message}");

                // Return an error response with status code 500 (Internal Server Error)
                return StatusCode(500, $"Error al eliminar el informe. Por favor, inténtalo de nuevo.");
            }
        }

    }
}
