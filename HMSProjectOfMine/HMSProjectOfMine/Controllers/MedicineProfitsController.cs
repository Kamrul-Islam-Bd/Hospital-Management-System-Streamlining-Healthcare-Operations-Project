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
    public class MedicineProfitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicineProfitsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicineProfits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineProfitDTO>>> GetMedicineProfits()
        {
            var medicineProfits = await _context.MedicineProfits
                .Include(mp => mp.MedicineSale) // Include the MedicineSale
                .ThenInclude(ms => ms.Medicine)   // ...and then its Medicine
                .ToListAsync();

            var medicineProfitDTOs = medicineProfits.Select(mp => new MedicineProfitDTO
            {
                MedicineProfitId = mp.MedicineProfitId,
                MedicineSaleId = mp.MedicineSaleId,
                ProfitDate = mp.ProfitDate,
                ProfitAmount = mp.ProfitAmount,
                QuantityProfit = mp.QuantityProfit,
                TotalProfit = mp.TotalProfit,
                MedicineSale = mp.MedicineSale == null ? null : new MedicineSaleDTO // Project MedicineSale
                {
                    MedicineSaleId = mp.MedicineSale.MedicineSaleId,
                    MedicineId = mp.MedicineSale.MedicineId,
                    SaleDate = mp.MedicineSale.SaleDate,
                    SalePrice = mp.MedicineSale.SalePrice,
                    QuantitySold = mp.MedicineSale.QuantitySold,
                    Discount = mp.MedicineSale.Discount,
                    TotalPrice = mp.MedicineSale.TotalPrice,
                    CustomerName = mp.MedicineSale.CustomerName,
                    Medicine = mp.MedicineSale.Medicine == null ?
                               null : new MedicineDTO
                               {
                                   MedicineId = mp.MedicineSale.Medicine.MedicineId,
                                   MedicineType = mp.MedicineSale.Medicine.MedicineType,
                                   MedicineName = mp.MedicineSale.Medicine.MedicineName,
                                   GenericName = mp.MedicineSale.Medicine.GenericName,
                                   Company = mp.MedicineSale.Medicine.Company
                               }
                }
            }).ToList();

            return Ok(medicineProfitDTOs);
        }

        // GET: api/MedicineProfits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineProfitDTO>> GetMedicineProfit(int id)
        {
            var medicineProfit = await _context.MedicineProfits
                .Include(mp => mp.MedicineSale) // Include the MedicineSale
                .ThenInclude(ms => ms.Medicine)
                .FirstOrDefaultAsync(mp => mp.MedicineProfitId == id);

            if (medicineProfit == null)
            {
                return NotFound();
            }

            var medicineProfitDTO = new MedicineProfitDTO
            {
                MedicineProfitId = medicineProfit.MedicineProfitId,
                MedicineSaleId = medicineProfit.MedicineSaleId,
                ProfitDate = medicineProfit.ProfitDate,
                ProfitAmount = medicineProfit.ProfitAmount,
                QuantityProfit = medicineProfit.QuantityProfit,
                TotalProfit = medicineProfit.TotalProfit,
                MedicineSale = medicineProfit.MedicineSale == null ?
                               null : new MedicineSaleDTO // Project MedicineSale
                               {
                                   MedicineSaleId = medicineProfit.MedicineSale.MedicineSaleId,
                                   MedicineId = medicineProfit.MedicineSale.MedicineId,
                                   SaleDate = medicineProfit.MedicineSale.SaleDate,
                                   SalePrice = medicineProfit.MedicineSale.SalePrice,
                                   QuantitySold = medicineProfit.MedicineSale.QuantitySold,
                                   Discount = medicineProfit.MedicineSale.Discount,
                                   TotalPrice = medicineProfit.MedicineSale.TotalPrice,
                                   CustomerName = medicineProfit.MedicineSale.CustomerName,
                                   Medicine = medicineProfit.MedicineSale.Medicine == null ?
                                              null : new MedicineDTO
                                              {
                                                  MedicineId = medicineProfit.MedicineSale.Medicine.MedicineId,
                                                  MedicineType = medicineProfit.MedicineSale.Medicine.MedicineType,
                                                  MedicineName = medicineProfit.MedicineSale.Medicine.MedicineName,
                                                  GenericName = medicineProfit.MedicineSale.Medicine.GenericName,
                                                  Company = medicineProfit.MedicineSale.Medicine.Company
                                              }
                               }
            };

            return Ok(medicineProfitDTO);
        }

        // POST: api/MedicineProfits
        [HttpPost]
        public async Task<ActionResult<MedicineProfitDTO>> PostMedicineProfit(CreateMedicineProfitDTO createMedicineProfitDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicineProfit = new MedicineProfit
            {
                MedicineSaleId = createMedicineProfitDTO.MedicineSaleId,
                ProfitDate = createMedicineProfitDTO.ProfitDate,
                ProfitAmount = createMedicineProfitDTO.ProfitAmount,
                QuantityProfit = createMedicineProfitDTO.QuantityProfit,
                TotalProfit = createMedicineProfitDTO.ProfitAmount * createMedicineProfitDTO.QuantityProfit // Calculate here
            };

            _context.MedicineProfits.Add(medicineProfit);
            await _context.SaveChangesAsync();

            var medicineProfitDTO = new MedicineProfitDTO
            {
                MedicineProfitId = medicineProfit.MedicineProfitId,
                MedicineSaleId = medicineProfit.MedicineSaleId,
                ProfitDate = medicineProfit.ProfitDate,
                ProfitAmount = medicineProfit.ProfitAmount,
                QuantityProfit = medicineProfit.QuantityProfit,
                TotalProfit = medicineProfit.TotalProfit,
            };

            return CreatedAtAction(nameof(GetMedicineProfit), new { id = medicineProfit.MedicineProfitId }, medicineProfitDTO);
        }

        // PUT: api/MedicineProfits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicineProfit(int id, UpdateMedicineProfitDTO updateMedicineProfitDTO)
        {
            if (id != updateMedicineProfitDTO.MedicineProfitId)
            {
                return BadRequest();
            }

            // Validate MedicineSaleId
            var medicineSale = await _context.MedicineSales.FindAsync(updateMedicineProfitDTO.MedicineSaleId);
            if (medicineSale == null)
            {
                return BadRequest("Invalid MedicineSaleId");
            }

            var medicineProfit = await _context.MedicineProfits.FindAsync(id);
            if (medicineProfit == null)
            {
                return NotFound();
            }

            medicineProfit.MedicineSaleId = updateMedicineProfitDTO.MedicineSaleId;
            medicineProfit.ProfitDate = updateMedicineProfitDTO.ProfitDate;
            medicineProfit.ProfitAmount = updateMedicineProfitDTO.ProfitAmount;
            medicineProfit.QuantityProfit = updateMedicineProfitDTO.QuantityProfit;
            // Recalculate TotalProfit on the backend during update
            medicineProfit.TotalProfit = updateMedicineProfitDTO.ProfitAmount * updateMedicineProfitDTO.QuantityProfit;
            _context.Entry(medicineProfit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineProfitExists(id))
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

        // DELETE: api/MedicineProfits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicineProfit(int id)
        {
            var medicineProfit = await _context.MedicineProfits.FindAsync(id);
            if (medicineProfit == null)
            {
                return NotFound();
            }

            _context.MedicineProfits.Remove(medicineProfit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineProfitExists(int id)
        {
            return _context.MedicineProfits.Any(e => e.MedicineProfitId == id);
        }
    }
}
