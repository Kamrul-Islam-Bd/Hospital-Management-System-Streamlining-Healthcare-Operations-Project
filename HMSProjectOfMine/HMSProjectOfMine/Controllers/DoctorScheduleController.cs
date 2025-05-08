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
    public class DoctorScheduleController : ControllerBase
    {

        private readonly AppDbContext _context;

        public DoctorScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DoctorSchedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorScheduleDTO>>> GetDoctorSchedules()
        {
            return await _context.DoctorSchedules
                .Select(ds => new DoctorScheduleDTO
                {
                    DoctorScheduleId = ds.DoctorScheduleId,
                    DoctorId = ds.DoctorId,
                    ChamberId = ds.ChamberId,
                    DaysOfWeek = ds.DaysOfWeek,
                    StartTime = ds.StartTime,
                    EndTime = ds.EndTime
                })
                .ToListAsync();
        }

        // GET: api/DoctorSchedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorScheduleDTO>> GetDoctorSchedule(int id)
        {
            var ds = await _context.DoctorSchedules.FindAsync(id);

            if (ds == null)
            {
                return NotFound();
            }

            return new DoctorScheduleDTO
            {
                DoctorScheduleId = ds.DoctorScheduleId,
                DoctorId = ds.DoctorId,
                ChamberId = ds.ChamberId,
                DaysOfWeek = ds.DaysOfWeek,
                StartTime = ds.StartTime,
                EndTime = ds.EndTime
            };
        }

        // POST: api/DoctorSchedule
        [HttpPost]
        public async Task<ActionResult<DoctorScheduleDTO>> PostDoctorSchedule(DoctorScheduleDTO dto)
        {
            var ds = new DoctorSchedule
            {
                DoctorId = dto.DoctorId,
                ChamberId = dto.ChamberId,
                DaysOfWeek = dto.DaysOfWeek,
                StartTime = dto.StartTime,  // hh:mm:ss (07:10:00)
                EndTime = dto.EndTime       // hh:mm:ss (10:30:00)
            };

            _context.DoctorSchedules.Add(ds);
            await _context.SaveChangesAsync();

            dto.DoctorScheduleId = ds.DoctorScheduleId;
            return CreatedAtAction(nameof(GetDoctorSchedule), new { id = ds.DoctorScheduleId }, dto);
        }

        // PUT: api/DoctorSchedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorSchedule(int id, DoctorScheduleDTO dto)
        {
            if (id != dto.DoctorScheduleId)
            {
                return BadRequest();
            }

            var ds = await _context.DoctorSchedules.FindAsync(id);
            if (ds == null)
            {
                return NotFound();
            }

            ds.DoctorId = dto.DoctorId;
            ds.ChamberId = dto.ChamberId;
            ds.DaysOfWeek = dto.DaysOfWeek;
            ds.StartTime = dto.StartTime;
            ds.EndTime = dto.EndTime;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/DoctorSchedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorSchedule(int id)
        {
            var ds = await _context.DoctorSchedules.FindAsync(id);
            if (ds == null)
            {
                return NotFound();
            }

            _context.DoctorSchedules.Remove(ds);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}