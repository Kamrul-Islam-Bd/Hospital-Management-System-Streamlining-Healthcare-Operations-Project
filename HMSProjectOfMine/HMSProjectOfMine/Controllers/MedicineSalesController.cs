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
    public class MedicineSalesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MedicineSalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicineSales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineSaleDTO>>> GetMedicineSales()
        {
            var medicineSales = await _context.MedicineSales
               .Include(ms => ms.Medicine) // Eager load the related Medicine
               .ToListAsync();
            var medicineSaleDTOs = medicineSales.Select(ms => new MedicineSaleDTO
            {
                MedicineSaleId = ms.MedicineSaleId,
                MedicineId = ms.MedicineId,
                SaleDate = ms.SaleDate,
                SalePrice = ms.SalePrice,
                QuantitySold = ms.QuantitySold,
                Discount = ms.Discount,
                TotalPrice = ms.TotalPrice,
                CustomerName = ms.CustomerName,
                Medicine = new MedicineDTO
                {
                    MedicineId = ms.Medicine.MedicineId,
                    MedicineType = ms.Medicine.MedicineType,
                    MedicineName = ms.Medicine.MedicineName,
                    GenericName = ms.Medicine.GenericName,
                    Company = ms.Medicine.Company
                }
            }).ToList();
            return Ok(medicineSaleDTOs);
        }

        // GET: api/MedicineSales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineSaleDTO>> GetMedicineSale(int id)
        {
            var medicineSale = await _context.MedicineSales
                .Include(ms => ms.Medicine) // Eager load the related Medicine
                .FirstOrDefaultAsync(ms => ms.MedicineSaleId == id);

            if (medicineSale == null)
            {
                return NotFound();
            }
            var medicineSaleDTO = new MedicineSaleDTO
            {
                MedicineSaleId = medicineSale.MedicineSaleId,
                MedicineId = medicineSale.MedicineId,
                SaleDate = medicineSale.SaleDate,
                SalePrice = medicineSale.SalePrice,
                QuantitySold = medicineSale.QuantitySold,
                Discount = medicineSale.Discount,
                TotalPrice = medicineSale.TotalPrice,
                CustomerName = medicineSale.CustomerName,
                Medicine = new MedicineDTO
                {
                    MedicineId = medicineSale.Medicine.MedicineId,
                    MedicineType = medicineSale.Medicine.MedicineType,
                    MedicineName = medicineSale.Medicine.MedicineName,
                    GenericName = medicineSale.Medicine.GenericName,
                    Company = medicineSale.Medicine.Company
                }
            };
            return Ok(medicineSaleDTO);
        }

        // POST: api/MedicineSales
        [HttpPost]
        public async Task<ActionResult<MedicineSaleDTO>> PostMedicineSale(CreateMedicineSaleDTO createMedicineSaleDTO)
        {
            // Validate MedicineId
            var medicine = await _context.Medicines.FindAsync(createMedicineSaleDTO.MedicineId);
            if (medicine == null)
            {
                return BadRequest("Invalid MedicineId");
            }

            var medicineSale = new MedicineSale
            {
                MedicineId = createMedicineSaleDTO.MedicineId,
                SaleDate = createMedicineSaleDTO.SaleDate,
                SalePrice = createMedicineSaleDTO.SalePrice,
                QuantitySold = createMedicineSaleDTO.QuantitySold,
                Discount = createMedicineSaleDTO.Discount,
                TotalPrice = createMedicineSaleDTO.TotalPrice,
                CustomerName = createMedicineSaleDTO.CustomerName,
            };
            _context.MedicineSales.Add(medicineSale);
            await _context.SaveChangesAsync();

            var medicineSaleDTO = new MedicineSaleDTO
            {
                MedicineSaleId = medicineSale.MedicineSaleId,
                MedicineId = medicineSale.MedicineId,
                SaleDate = medicineSale.SaleDate,
                SalePrice = medicineSale.SalePrice,
                QuantitySold = medicineSale.QuantitySold,
                Discount = medicineSale.Discount,
                TotalPrice = medicineSale.TotalPrice,
                CustomerName = medicineSale.CustomerName,
            };
            return CreatedAtAction(nameof(GetMedicineSale), new { id = medicineSale.MedicineSaleId }, medicineSaleDTO);
        }

        // PUT: api/MedicineSales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicineSale(int id, UpdateMedicineSaleDTO updateMedicineSaleDTO)
        {
            if (id != updateMedicineSaleDTO.MedicineSaleId)
            {
                return BadRequest();
            }
            // Validate MedicineId
            var medicine = await _context.Medicines.FindAsync(updateMedicineSaleDTO.MedicineId);
            if (medicine == null)
            {
                return BadRequest("Invalid MedicineId");
            }

            var medicineSale = await _context.MedicineSales.FindAsync(id);
            if (medicineSale == null)
            {
                return NotFound();
            }

            medicineSale.MedicineId = updateMedicineSaleDTO.MedicineId;
            medicineSale.SaleDate = updateMedicineSaleDTO.SaleDate;
            medicineSale.SalePrice = updateMedicineSaleDTO.SalePrice;
            medicineSale.QuantitySold = updateMedicineSaleDTO.QuantitySold;
            medicineSale.Discount = updateMedicineSaleDTO.Discount;
            medicineSale.TotalPrice = updateMedicineSaleDTO.TotalPrice;
            medicineSale.CustomerName = updateMedicineSaleDTO.CustomerName;

            _context.Entry(medicineSale).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineSaleExists(id))
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

        // DELETE: api/MedicineSales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicineSale(int id)
        {
            var medicineSale = await _context.MedicineSales.FindAsync(id);
            if (medicineSale == null)
            {
                return NotFound();
            }

            _context.MedicineSales.Remove(medicineSale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineSaleExists(int id)
        {
            return _context.MedicineSales.Any(e => e.MedicineSaleId == id);
        }
    }
}
