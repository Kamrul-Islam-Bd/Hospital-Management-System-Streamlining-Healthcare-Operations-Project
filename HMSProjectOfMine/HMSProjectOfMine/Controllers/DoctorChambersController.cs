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
    public class DoctorChambersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorChambersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DoctorChambers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorChamberDTO>>> GetDoctorChambers()
        {
            return await _context.DoctorChambers
                .Select(dc => new DoctorChamberDTO
                {
                    DoctorChamberId = dc.DoctorChamberId,
                    DoctorId = dc.DoctorId,
                    ChamberId = dc.ChamberId,
                    AvailableTime = dc.AvailableTime
                })
                .ToListAsync();
        }

        // GET: api/DoctorChambers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorChamberDTO>> GetDoctorChamber(int id)
        {
            var dc = await _context.DoctorChambers.FindAsync(id);

            if (dc == null)
            {
                return NotFound();
            }

            return new DoctorChamberDTO
            {
                DoctorChamberId = dc.DoctorChamberId,
                DoctorId = dc.DoctorId,
                ChamberId = dc.ChamberId,
                AvailableTime = dc.AvailableTime
            };
        }

        // POST: api/DoctorChambers
        [HttpPost]
        public async Task<ActionResult<DoctorChamberDTO>> PostDoctorChamber(DoctorChamberDTO dto)
        {
            var entity = new DoctorChamber
            {
                DoctorId = dto.DoctorId,
                ChamberId = dto.ChamberId,
                AvailableTime = dto.AvailableTime
            };

            _context.DoctorChambers.Add(entity);
            await _context.SaveChangesAsync();

            dto.DoctorChamberId = entity.DoctorChamberId;

            return CreatedAtAction(nameof(GetDoctorChamber), new { id = dto.DoctorChamberId }, dto);
        }

        //// PUT: api/DoctorChambers/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDoctorChamber(int id, DoctorChamberDTO dto)
        //{
        //    if (id != dto.DoctorChamberId)
        //    {
        //        return BadRequest();
        //    }

        //    var entity = await _context.DoctorChambers.FindAsync(id);
        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    entity.DoctorId = dto.DoctorId;
        //    entity.ChamberId = dto.ChamberId;
        //    entity.AvailableTime = dto.AvailableTime;

        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorChamber(int id, DoctorChamberDTO dto)
        {
            if (id != dto.DoctorChamberId)
                return BadRequest("ID mismatch.");

            var entity = await _context.DoctorChambers.FindAsync(id);
            if (entity == null)
                return NotFound("DoctorChamber not found.");

            // Optional: Add field validation here
            if (string.IsNullOrWhiteSpace(dto.AvailableTime))
                return BadRequest("AvailableTime cannot be empty.");

            // Update fields
            entity.DoctorId = dto.DoctorId;
            entity.ChamberId = dto.ChamberId;
            entity.AvailableTime = dto.AvailableTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Update failed: {ex.Message}");
            }

            return NoContent();
        }


        // DELETE: api/DoctorChambers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorChamber(int id)
        {
            var entity = await _context.DoctorChambers.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.DoctorChambers.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
