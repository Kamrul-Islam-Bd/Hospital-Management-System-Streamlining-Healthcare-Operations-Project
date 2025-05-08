using HMSProjectOfMine.Data;
using HMSProjectOfMine.DTOs;
using HMSProjectOfMine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMSProjectOfMine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestReportsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TestReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestReportDTO>>> GetAll()
        {
            var reports = await _context.TestReports
                .Select(r => new TestReportDTO
                {
                    TestReportId = r.TestReportId,
                    PrescriptionTestId = r.PrescriptionTestId,
                    TestResult = r.TestResult,
                    IsFinalized = r.IsFinalized
                })
                .ToListAsync();

            return Ok(reports);
        }

        // GET: api/TestReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestReportDTO>> Get(int id)
        {
            var report = await _context.TestReports
                .Where(r => r.TestReportId == id)
                .Select(r => new TestReportDTO
                {
                    TestReportId = r.TestReportId,
                    PrescriptionTestId = r.PrescriptionTestId,
                    TestResult = r.TestResult,
                    IsFinalized = r.IsFinalized
                })
                .FirstOrDefaultAsync();

            if (report == null) return NotFound();
            return Ok(report);
        }

        // POST: api/TestReports
        [HttpPost]
        public async Task<ActionResult<TestReportDTO>> Create(TestReportDTO dto)
        {
            var report = new TestReport
            {
                PrescriptionTestId = dto.PrescriptionTestId,
                TestResult = dto.TestResult,
                IsFinalized = dto.IsFinalized
            };

            _context.TestReports.Add(report);
            await _context.SaveChangesAsync();

            dto.TestReportId = report.TestReportId;
            return CreatedAtAction(nameof(Get), new { id = report.TestReportId }, dto);
        }

        // PUT: api/TestReports/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TestReportDTO dto)
        {
            if (id != dto.TestReportId) return BadRequest();

            var report = await _context.TestReports.FindAsync(id);
            if (report == null) return NotFound();

            if (report.IsFinalized)
                return BadRequest("Cannot update finalized report.");

            report.TestResult = dto.TestResult;
            report.IsFinalized = dto.IsFinalized;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/TestReports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var report = await _context.TestReports.FindAsync(id);
            if (report == null) return NotFound();

            if (report.IsFinalized)
                return BadRequest("Cannot delete finalized report.");

            _context.TestReports.Remove(report);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
