using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonitoringApi.Models;
using MonitoringApi.Services; 
using MongoDB.Driver; 
 

namespace MonitoringApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsService _reportsService;

        public ReportsController(ReportsService reportsService) =>
        
            _reportsService = reportsService;
        

        [HttpGet]
        public async Task<List<Report>> GetAllReports() =>
            await _reportsService.GetAllReportsAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Report>> GetReportById(string id)
        {
            var report = await _reportsService.GetReportByIdAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Report newReport)
        {
            await _reportsService.CreateReportAsync(newReport);

            return CreatedAtAction(nameof(GetReportById), new { id = newReport.Id }, newReport);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateReport(string id, Report updatedReport)
        {
            var report = await _reportsService.GetReportByIdAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            updatedReport.Id = id;

            await _reportsService.UpdateReportAsync(id, updatedReport);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> RemoveReport(string id)
        {
            var report = await _reportsService.GetReportByIdAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            await _reportsService.RemoveReportAsync(id);

            return NoContent();
        }
    }
}

