using Microsoft.AspNetCore.Mvc;
using MonitoringApi.Models;
using MonitoringApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportsServices _reportService;

        public ReportController(ReportsServices reportService)
        {
            _reportService = reportService;
        }

        // GET: api/report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            var reports = await _reportService.GetReportsAsync();
            return Ok(reports);
        }

        // GET: api/report/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(string id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/report/{id}
        // DELETE: api/report/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            try
            {
                var result = await _reportService.DeleteReportAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = "Report not found" });
                }
                return Ok(new { Message = "Report deleted successfully" }); // Devuelve 200 OK con un mensaje
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
