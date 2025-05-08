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
    public class TokensController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TokensController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new { d.DoctorId, d.FirstName, d.LastName })
                .ToListAsync();
            return Ok(doctors);
        }

        [HttpGet("chambers")]
        public async Task<ActionResult<IEnumerable<Chamber>>> GetChambers()
        {
            var chambers = await _context.Chambers
                .Select(c => new { c.ChamberId, c.ChamberNo, c.Location })
                .ToListAsync();
            return Ok(chambers);
        }


        [HttpGet("patient")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            var patients = await _context.Patients
                .Select(c => new { c.PatientId, c.FirstName, c.LastName })
                .ToListAsync();
            return Ok(patients);
        }

        // POST: api/Tokens
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] TokenDto dto)
        {
            // Validate doctor, patient, and chamber
            var patientExists = await _context.Patients.AnyAsync(p => p.PatientId == dto.PatientId);
            if (!patientExists) return BadRequest("Invalid Patient");

            var doctorExists = await _context.Doctors.AnyAsync(d => d.DoctorId == dto.DoctorId);
            if (!doctorExists) return BadRequest("Invalid Doctor");

            var chamberExists = await _context.Chambers.AnyAsync(c => c.ChamberId == dto.ChamberId);
            if (!chamberExists) return BadRequest("Invalid Chamber");

            // Step 1: Get today's token count
            var today = DateTime.Today;
            int countToday = await _context.Tokens.CountAsync(t => t.IssueTime.Date == today);

            // Step 2: Generate Token Number like "TKN-001"
            string generatedTokenNumber = $"TKN-{(countToday + 1).ToString("D3")}";

            // Step 3: Create new token
            var token = new Token
            {
                TokenNumber = generatedTokenNumber,
                TokenFee = dto.TokenFee,
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ChamberId = dto.ChamberId,
                IssueTime = DateTime.Now
            };

            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            dto.TokenId = token.TokenId;
            dto.TokenNumber = token.TokenNumber;
            dto.IssueTime = token.IssueTime;

            return CreatedAtAction(nameof(GetTokenById), new { id = token.TokenId }, dto);
        }

        // GET: api/Tokens?doctorId=1&date=2025-04-29
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TokenDto>>> GetTokens([FromQuery] int doctorId, [FromQuery] DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);

            var tokens = await _context.Tokens
                .Where(t => t.DoctorId == doctorId && t.IssueTime >= startOfDay && t.IssueTime <= endOfDay)
                .Select(t => new TokenDto
                {
                    TokenId = t.TokenId,
                    TokenNumber = t.TokenNumber,
                    TokenFee = t.TokenFee,
                    PatientId = t.PatientId,
                    DoctorId = t.DoctorId,
                    ChamberId = t.ChamberId,
                    IssueTime = t.IssueTime
                })
                .ToListAsync();

            return Ok(tokens);
        }

        // GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TokenDto>> GetTokenById(int id)
        {
            var token = await _context.Tokens.FindAsync(id);

            if (token == null) return NotFound();

            var dto = new TokenDto
            {
                TokenId = token.TokenId,
                TokenNumber = token.TokenNumber,
                TokenFee = token.TokenFee,
                PatientId = token.PatientId,
                DoctorId = token.DoctorId,
                ChamberId = token.ChamberId,
                IssueTime = token.IssueTime
            };

            return Ok(dto);
        }

        // DELETE: api/Tokens/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken(int id)
        {
            var token = await _context.Tokens.FindAsync(id);
            if (token == null) return NotFound();

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();

            return NoContent();
        }




















        //    // POST: api/Tokens

        //    [HttpPost]
        //    public async Task<IActionResult> CreateToken([FromBody] TokenDto dto)
        //    {
        //        // Step 1: Get today's token count
        //        var today = DateTime.Today;
        //        int countToday = await _context.Tokens
        //            .CountAsync(t => t.IssueTime.Date == today);

        //        // Step 2: Generate Token Number like "TKN-001"
        //        string generatedTokenNumber = $"TKN-{(countToday + 1).ToString("D3")}";

        //        // Step 3: Create new token
        //        var token = new Token
        //        {
        //            TokenNumber = generatedTokenNumber,
        //            TokenFee = dto.TokenFee,
        //            PatientId = dto.PatientId,
        //            DoctorId = dto.DoctorId,
        //            ChamberId = dto.ChamberId,
        //            IssueTime = DateTime.Now
        //        };

        //        _context.Tokens.Add(token);
        //        await _context.SaveChangesAsync();

        //        dto.TokenId = token.TokenId;
        //        dto.TokenNumber = token.TokenNumber;
        //        dto.IssueTime = token.IssueTime;

        //        return CreatedAtAction(nameof(GetTokenById), new { id = token.TokenId }, dto);
        //    }



        //    // GET: api/Tokens?doctorId=1&date=2025-04-29
        //    [HttpGet]
        //    public async Task<ActionResult<IEnumerable<TokenDto>>> GetTokens([FromQuery] int doctorId, [FromQuery] DateTime date)
        //    {
        //        var tokens = await _context.Tokens
        //            //.Where(t => t.DoctorId == doctorId && t.IssueTime.Date == date.Date)
        //            .Where(t =>
        //                    t.DoctorId == doctorId &&
        //                    t.IssueTime >= date.Date &&
        //                    t.IssueTime < date.Date.AddDays(1)) //dateforment = yyyy/mm/dd
        //            .Select(t => new TokenDto
        //            {
        //                TokenId = t.TokenId,
        //                TokenNumber = t.TokenNumber,
        //                TokenFee = t.TokenFee,
        //                PatientId = t.PatientId,
        //                DoctorId = t.DoctorId,
        //                ChamberId = t.ChamberId,
        //                IssueTime = t.IssueTime
        //            }).ToListAsync();

        //        return Ok(tokens);
        //    }

        //    // GET by ID (optional for CreatedAtAction)
        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<TokenDto>> GetTokenById(int id)
        //    {
        //        var token = await _context.Tokens.FindAsync(id);

        //        if (token == null) return NotFound();

        //        var dto = new TokenDto
        //        {
        //            TokenId = token.TokenId,
        //            TokenNumber = token.TokenNumber,
        //            TokenFee = token.TokenFee,
        //            PatientId = token.PatientId,
        //            DoctorId = token.DoctorId,
        //            ChamberId = token.ChamberId,
        //            IssueTime = token.IssueTime
        //        };

        //        return Ok(dto);
        //    }

        //    // DELETE: api/Tokens/{id}
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteToken(int id)
        //    {
        //        var token = await _context.Tokens.FindAsync(id);
        //        if (token == null) return NotFound();

        //        _context.Tokens.Remove(token);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //}
    }
}
