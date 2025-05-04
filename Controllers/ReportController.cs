using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using security_scan.Data;
using security_scan.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace security_scan.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Report/
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reports = await _context.Reports
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ScanDate)
                .ToListAsync();

            return View(reports);
        }

        // GET: /Report/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Report/Create (Handles both AJAX and standard form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Report reportJson)
        {
            Report report;

            if (Request.ContentType.Contains("application/json"))
            {
                // Coming from AJAX
                if (string.IsNullOrWhiteSpace(reportJson.Note))
                    return BadRequest("Note is required.");

                report = new Report { Note = reportJson.Note };
            }
            else
            {
                // Fallback for non-AJAX, bind from form
                report = new Report();
                await TryUpdateModelAsync(report, prefix: "", includeExpressions: m => m.Note);
                if (!ModelState.IsValid)
                    return View(report);
            }

            var findings = new List<string>();
            try
            {
                var jsonPath = Path.Combine(_env.WebRootPath, "mock-data", "mockScanResult.json");
                var json = await System.IO.File.ReadAllTextAsync(jsonPath);
                var scanData = JsonSerializer.Deserialize<MockScanResult>(json);

                findings.AddRange(scanData.S3Buckets.Where(b => b.PublicAccess).Select(b => $"S3 Bucket '{b.Name}' is public."));
                findings.AddRange(scanData.IamPolicies.Where(p => p.IsTooPermissive).Select(p => $"IAM Policy '{p.Name}' is overly permissive."));
                findings.AddRange(scanData.SecurityGroups.Where(sg => sg.OpenPorts.Any()).Select(sg => $"Security Group '{sg.Name}' has open ports: {string.Join(", ", sg.OpenPorts)}"));
            }
            catch (Exception ex)
            {
                findings.Add($"Error loading scan data: {ex.Message}");
            }

            report.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            report.ScanDate = DateTime.UtcNow;
            report.SeverityLevel = findings.Any() ? "High" : "Low";
            report.Summary = findings.Any() ? string.Join("\n", findings) : "No issues found.";

            _context.Add(report);
            await _context.SaveChangesAsync();

            if (Request.ContentType.Contains("application/json"))
                return Ok(); // AJAX response
            else
                return RedirectToAction(nameof(Index));
        }

        // GET: /Report/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (report == null) return NotFound();

            return View(report);
        }

        // GET: /Report/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (report == null) return NotFound();

            return View(report);
        }

        // POST: /Report/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Report/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
            if (report == null) return NotFound();

            return View(report);
        }

        // POST: /Report/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Note")] Report report)
        {
            var dbReport = await _context.Reports.FindAsync(id);
            if (dbReport == null || dbReport.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            dbReport.Note = report.Note;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
