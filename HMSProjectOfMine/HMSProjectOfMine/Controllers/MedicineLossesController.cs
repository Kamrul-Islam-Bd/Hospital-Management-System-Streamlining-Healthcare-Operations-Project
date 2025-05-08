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
    public class MedicineLossesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicineLossesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicineLosses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineLossDTO>>> GetMedicineLosses()
        {
            var medicineLosses = await _context.MedicineLosses
                .Include(ml => ml.MedicinePurchase) // Include MedicinePurchase
                .ThenInclude(mp => mp.Medicine)  // Include Medicine
                .ToListAsync();
            var medicineLossDTOs = medicineLosses.Select(ml => new MedicineLossDTO
            {
                MedicineLossId = ml.MedicineLossId,
                MedicinePurchaseId = ml.MedicinePurchaseId,
                LossDate = ml.LossDate,
                QuantityLoss = ml.QuantityLoss,
                LossAmount = ml.LossAmount,
                TotalLoss = ml.TotalLoss,
                LossReason = ml.LossReason,
                MedicinePurchase = ml.MedicinePurchase == null ? null : new MedicinePurchaseDTO  // Project MedicinePurchase
                {
                    MedicinePurchaseId = ml.MedicinePurchase.MedicinePurchaseId,
                    MedicineId = ml.MedicinePurchase.MedicineId,
                    PurchaseDate = ml.MedicinePurchase.PurchaseDate,
                    PurchasePrice = ml.MedicinePurchase.PurchasePrice,
                    QuantityPurchased = ml.MedicinePurchase.QuantityPurchased,
                    VAT = ml.MedicinePurchase.VAT,
                    TotalPrice = ml.MedicinePurchase.TotalPrice,
                    Supplier = ml.MedicinePurchase.Supplier,
                    Medicine = ml.MedicinePurchase.Medicine == null ?
                        null : new MedicineDTO
                        {
                            MedicineId = ml.MedicinePurchase.Medicine.MedicineId,
                            MedicineType = ml.MedicinePurchase.Medicine.MedicineType,
                            MedicineName = ml.MedicinePurchase.Medicine.MedicineName,
                            GenericName = ml.MedicinePurchase.Medicine.GenericName,
                            Company = ml.MedicinePurchase.Medicine.Company
                        }
                }
            }).ToList();
            return Ok(medicineLossDTOs);
        }

        // GET: api/MedicineLosses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineLossDTO>> GetMedicineLoss(int id)
        {
            var medicineLoss = await _context.MedicineLosses
                 .Include(ml => ml.MedicinePurchase) // Include MedicinePurchase
                .ThenInclude(mp => mp.Medicine)
                .FirstOrDefaultAsync(ml => ml.MedicineLossId == id);
            if (medicineLoss == null)
            {
                return NotFound();
            }

            var medicineLossDTO = new MedicineLossDTO
            {
                MedicineLossId = medicineLoss.MedicineLossId,
                MedicinePurchaseId = medicineLoss.MedicinePurchaseId,
                LossDate = medicineLoss.LossDate,
                QuantityLoss = medicineLoss.QuantityLoss,
                LossAmount = medicineLoss.LossAmount,
                TotalLoss = medicineLoss.TotalLoss,
                LossReason = medicineLoss.LossReason,
                MedicinePurchase = medicineLoss.MedicinePurchase == null ?
                    null : new MedicinePurchaseDTO  // Project MedicinePurchase
                    {
                        MedicinePurchaseId = medicineLoss.MedicinePurchase.MedicinePurchaseId,
                        MedicineId = medicineLoss.MedicinePurchase.MedicineId,
                        PurchaseDate = medicineLoss.MedicinePurchase.PurchaseDate,
                        PurchasePrice = medicineLoss.MedicinePurchase.PurchasePrice,
                        QuantityPurchased = medicineLoss.MedicinePurchase.QuantityPurchased,
                        VAT = medicineLoss.MedicinePurchase.VAT,
                        TotalPrice = medicineLoss.MedicinePurchase.TotalPrice,
                        Supplier = medicineLoss.MedicinePurchase.Supplier,
                        Medicine = medicineLoss.MedicinePurchase.Medicine == null ?
                            null : new MedicineDTO
                            {
                                MedicineId = medicineLoss.MedicinePurchase.Medicine.MedicineId,
                                MedicineType = medicineLoss.MedicinePurchase.Medicine.MedicineType,
                                MedicineName = medicineLoss.MedicinePurchase.Medicine.MedicineName,
                                GenericName = medicineLoss.MedicinePurchase.Medicine.GenericName,
                                Company = medicineLoss.MedicinePurchase.Medicine.Company
                            }
                    }
            };
            return Ok(medicineLossDTO);
        }

        // POST: api/MedicineLosses
        [HttpPost]
        public async Task<ActionResult<MedicineLossDTO>> PostMedicineLoss(CreateMedicineLossDTO createMedicineLossDTO)
        {
            // Validate MedicinePurchaseId
            var medicinePurchase = await _context.MedicinePurchases.FindAsync(createMedicineLossDTO.MedicinePurchaseId);
            if (medicinePurchase == null)
            {
                return BadRequest("Invalid MedicinePurchaseId");
            }

            var medicineLoss = new MedicineLoss
            {
                MedicinePurchaseId = createMedicineLossDTO.MedicinePurchaseId,
                LossDate = createMedicineLossDTO.LossDate,
                QuantityLoss = createMedicineLossDTO.QuantityLoss,
                LossAmount = createMedicineLossDTO.LossAmount,
                TotalLoss = createMedicineLossDTO.TotalLoss,
                LossReason = createMedicineLossDTO.LossReason,
            };
            _context.MedicineLosses.Add(medicineLoss);
            await _context.SaveChangesAsync();

            var medicineLossDTO = new MedicineLossDTO
            {
                MedicineLossId = medicineLoss.MedicineLossId,
                MedicinePurchaseId = medicineLoss.MedicinePurchaseId,
                LossDate = medicineLoss.LossDate,
                QuantityLoss = medicineLoss.QuantityLoss,
                LossAmount = medicineLoss.LossAmount,
                TotalLoss = medicineLoss.TotalLoss,
                LossReason = medicineLoss.LossReason,
            };
            return CreatedAtAction(nameof(GetMedicineLoss), new { id = medicineLoss.MedicineLossId }, medicineLossDTO);
        }

        // PUT: api/MedicineLosses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicineLoss(int id, UpdateMedicineLossDTO updateMedicineLossDTO)
        {
            if (id != updateMedicineLossDTO.MedicineLossId)
            {
                return BadRequest();
            }
            // Validate MedicinePurchaseId
            var medicinePurchase = await _context.MedicinePurchases.FindAsync(updateMedicineLossDTO.MedicinePurchaseId);
            if (medicinePurchase == null)
            {
                return BadRequest("Invalid MedicinePurchaseId");
            }

            var medicineLoss = await _context.MedicineLosses.FindAsync(id);
            if (medicineLoss == null)
            {
                return NotFound();
            }

            medicineLoss.MedicinePurchaseId = updateMedicineLossDTO.MedicinePurchaseId;
            medicineLoss.LossDate = updateMedicineLossDTO.LossDate;
            medicineLoss.QuantityLoss = updateMedicineLossDTO.QuantityLoss;
            medicineLoss.LossAmount = updateMedicineLossDTO.LossAmount;
            medicineLoss.TotalLoss = updateMedicineLossDTO.TotalLoss;
            medicineLoss.LossReason = updateMedicineLossDTO.LossReason;

            _context.Entry(medicineLoss).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineLossExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/MedicineLosses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicineLoss(int id)
        {
            var medicineLoss = await _context.MedicineLosses.FindAsync(id);
            if (medicineLoss == null)
            {
                return NotFound();
            }

            _context.MedicineLosses.Remove(medicineLoss);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineLossExists(int id)
        {
            return _context.MedicineLosses.Any(e => e.MedicineLossId == id);
        }
    }
}
