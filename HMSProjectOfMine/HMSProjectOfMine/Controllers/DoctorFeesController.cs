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
    public class DoctorFeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorFeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DoctorFees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorFeeDTO>>> GetDoctorFees()
        {
            return await _context.DoctorFees
                .Select(f => new DoctorFeeDTO
                {
                    DoctorFeeId = f.DoctorFeeId,
                    DoctorId = f.DoctorId,
                    Fees = f.Fees,
                    DiscountAmount = f.DiscountAmount,
                    EffectiveDate = f.EffectiveDate,
                    ChargedFee = f.ChargedFee,
                    VisitType = f.VisitType
                })
                .ToListAsync();
        }

        // GET: api/DoctorFees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorFeeDTO>> GetDoctorFee(int id)
        {
            var fee = await _context.DoctorFees.FindAsync(id);

            if (fee == null)
            {
                return NotFound();
            }

            return new DoctorFeeDTO
            {
                DoctorFeeId = fee.DoctorFeeId,
                DoctorId = fee.DoctorId,
                Fees = fee.Fees,
                DiscountAmount = fee.DiscountAmount,
                EffectiveDate = fee.EffectiveDate,
                ChargedFee = fee.ChargedFee,
                VisitType = fee.VisitType
            };
        }

        // POST: api/DoctorFees
        [HttpPost]
        public async Task<ActionResult<DoctorFeeDTO>> PostDoctorFee(DoctorFeeDTO dto)
        {
            var entity = new DoctorFee
            {
                DoctorId = dto.DoctorId,
                Fees = dto.Fees,
                DiscountAmount = dto.DiscountAmount,
                EffectiveDate = dto.EffectiveDate,
                ChargedFee = dto.ChargedFee,
                VisitType = dto.VisitType
            };

            _context.DoctorFees.Add(entity);
            await _context.SaveChangesAsync();

            dto.DoctorFeeId = entity.DoctorFeeId;

            return CreatedAtAction(nameof(GetDoctorFee), new { id = dto.DoctorFeeId }, dto);
        }

        // PUT: api/DoctorFees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorFee(int id, DoctorFeeDTO dto)
        {
            if (id != dto.DoctorFeeId)
            {
                return BadRequest("Mismatched DoctorFeeId.");
            }

            var entity = await _context.DoctorFees.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.DoctorId = dto.DoctorId;
            entity.Fees = dto.Fees;
            entity.DiscountAmount = dto.DiscountAmount;
            entity.EffectiveDate = dto.EffectiveDate;
            entity.ChargedFee = dto.ChargedFee;
            entity.VisitType = dto.VisitType;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/DoctorFees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorFee(int id)
        {
            var entity = await _context.DoctorFees.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.DoctorFees.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
