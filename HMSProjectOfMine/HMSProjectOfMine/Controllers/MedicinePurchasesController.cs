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
    public class MedicinePurchasesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicinePurchasesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicinePurchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicinePurchaseDTO>>> GetMedicinePurchases()
        {
            var medicinePurchases = await _context.MedicinePurchases
               .Include(mp => mp.Medicine) // Eager load the related Medicine
               .ToListAsync();
            var medicinePurchaseDTOs = medicinePurchases.Select(mp => new MedicinePurchaseDTO
            {
                MedicinePurchaseId = mp.MedicinePurchaseId,
                MedicineId = mp.MedicineId,
                PurchaseDate = mp.PurchaseDate,
                PurchasePrice = mp.PurchasePrice,
                QuantityPurchased = mp.QuantityPurchased,
                VAT = mp.VAT,
                TotalPrice = mp.TotalPrice,
                Supplier = mp.Supplier,
                Medicine = new MedicineDTO // Project the related Medicine data
                {
                    MedicineId = mp.Medicine.MedicineId,
                    MedicineType = mp.Medicine.MedicineType,
                    MedicineName = mp.Medicine.MedicineName,
                    GenericName = mp.Medicine.GenericName,
                    Company = mp.Medicine.Company
                }
            }).ToList();
            return Ok(medicinePurchaseDTOs);
        }

        // GET: api/MedicinePurchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicinePurchaseDTO>> GetMedicinePurchase(int id)
        {
            var medicinePurchase = await _context.MedicinePurchases
                .Include(mp => mp.Medicine) // Eager load the related Medicine
                .FirstOrDefaultAsync(mp => mp.MedicinePurchaseId == id);


            if (medicinePurchase == null)
            {
                return NotFound();
            }

            var medicinePurchaseDTO = new MedicinePurchaseDTO
            {
                MedicinePurchaseId = medicinePurchase.MedicinePurchaseId,
                MedicineId = medicinePurchase.MedicineId,
                PurchaseDate = medicinePurchase.PurchaseDate,
                PurchasePrice = medicinePurchase.PurchasePrice,
                QuantityPurchased = medicinePurchase.QuantityPurchased,
                VAT = medicinePurchase.VAT,
                TotalPrice = medicinePurchase.TotalPrice,
                Supplier = medicinePurchase.Supplier,
                Medicine = new MedicineDTO // Project the related Medicine data
                {
                    MedicineId = medicinePurchase.Medicine.MedicineId,
                    MedicineType = medicinePurchase.Medicine.MedicineType,
                    MedicineName = medicinePurchase.Medicine.MedicineName,
                    GenericName = medicinePurchase.Medicine.GenericName,
                    Company = medicinePurchase.Medicine.Company
                }
            };
            return Ok(medicinePurchaseDTO);
        }

        // POST: api/MedicinePurchases
        [HttpPost]
        public async Task<ActionResult<MedicinePurchaseDTO>> PostMedicinePurchase(CreateMedicinePurchaseDTO createMedicinePurchaseDTO)
        {
            // Validate MedicineId
            var medicine = await _context.Medicines.FindAsync(createMedicinePurchaseDTO.MedicineId);
            if (medicine == null)
            {
                return BadRequest("Invalid MedicineId");
            }

            var medicinePurchase = new MedicinePurchase
            {
                MedicineId = createMedicinePurchaseDTO.MedicineId,
                PurchaseDate = createMedicinePurchaseDTO.PurchaseDate,
                PurchasePrice = createMedicinePurchaseDTO.PurchasePrice,
                QuantityPurchased = createMedicinePurchaseDTO.QuantityPurchased,
                VAT = createMedicinePurchaseDTO.VAT,
                TotalPrice = createMedicinePurchaseDTO.TotalPrice,
                Supplier = createMedicinePurchaseDTO.Supplier,
            };
            _context.MedicinePurchases.Add(medicinePurchase);
            await _context.SaveChangesAsync();

            var medicinePurchaseDTO = new MedicinePurchaseDTO
            {
                MedicinePurchaseId = medicinePurchase.MedicinePurchaseId,
                MedicineId = medicinePurchase.MedicineId,
                PurchaseDate = medicinePurchase.PurchaseDate,
                PurchasePrice = medicinePurchase.PurchasePrice,
                QuantityPurchased = medicinePurchase.QuantityPurchased,
                VAT = medicinePurchase.VAT,
                TotalPrice = medicinePurchase.TotalPrice,
                Supplier = medicinePurchase.Supplier,

            };
            return CreatedAtAction(nameof(GetMedicinePurchase), new { id = medicinePurchase.MedicinePurchaseId }, medicinePurchaseDTO);
        }

        // PUT: api/MedicinePurchases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicinePurchase(int id, UpdateMedicinePurchaseDTO updateMedicinePurchaseDTO)
        {
            if (id != updateMedicinePurchaseDTO.MedicinePurchaseId)
            {
                return BadRequest();
            }
            // Validate MedicineId
            var medicine = await _context.Medicines.FindAsync(updateMedicinePurchaseDTO.MedicineId);
            if (medicine == null)
            {
                return BadRequest("Invalid MedicineId");
            }

            var medicinePurchase = await _context.MedicinePurchases.FindAsync(id);
            if (medicinePurchase == null)
            {
                return NotFound();
            }

            medicinePurchase.MedicineId = updateMedicinePurchaseDTO.MedicineId;
            medicinePurchase.PurchaseDate = updateMedicinePurchaseDTO.PurchaseDate;
            medicinePurchase.PurchasePrice = updateMedicinePurchaseDTO.PurchasePrice;
            medicinePurchase.QuantityPurchased = updateMedicinePurchaseDTO.QuantityPurchased;
            medicinePurchase.VAT = updateMedicinePurchaseDTO.VAT;
            medicinePurchase.TotalPrice = updateMedicinePurchaseDTO.TotalPrice;
            medicinePurchase.Supplier = updateMedicinePurchaseDTO.Supplier;


            _context.Entry(medicinePurchase).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicinePurchaseExists(id))
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

        // DELETE: api/MedicinePurchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicinePurchase(int id)
        {
            var medicinePurchase = await _context.MedicinePurchases.FindAsync(id);
            if (medicinePurchase == null)
            {
                return NotFound();
            }

            _context.MedicinePurchases.Remove(medicinePurchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicinePurchaseExists(int id)
        {
            return _context.MedicinePurchases.Any(e => e.MedicinePurchaseId == id);
        }
    }

}
