using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using security_scan.Data;
using security_scan.Models;
using System.Security.Claims;

namespace security_scan.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ReportsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetUserReports()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Reports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ScanDate)
                .ToListAsync();
        }

        // GET: api/reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (report == null)
                return NotFound();

            return report;
        }

        // DELETE: api/reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (report == null)
                return NotFound();

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
