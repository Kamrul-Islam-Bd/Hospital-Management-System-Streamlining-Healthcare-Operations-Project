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
    public class MedicinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicineDTO>>> GetMedicines()
        {
            var medicines = await _context.Medicines.ToListAsync();
            var medicineDTOs = medicines.Select(m => new MedicineDTO
            {
                MedicineId = m.MedicineId,
                MedicineType = m.MedicineType,
                MedicineName = m.MedicineName,
                GenericName = m.GenericName,
                Company = m.Company
            }).ToList();
            return Ok(medicineDTOs);
        }

        // GET: api/Medicines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicineDTO>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            var medicineDTO = new MedicineDTO
            {
                MedicineId = medicine.MedicineId,
                MedicineType = medicine.MedicineType,
                MedicineName = medicine.MedicineName,
                GenericName = medicine.GenericName,
                Company = medicine.Company
            };
            return Ok(medicineDTO);
        }

        // POST: api/Medicines
        [HttpPost]
        public async Task<ActionResult<MedicineDTO>> PostMedicine(CreateMedicineDTO createMedicineDTO)
        {
            var medicine = new Medicine
            {
                MedicineType = createMedicineDTO.MedicineType,
                MedicineName = createMedicineDTO.MedicineName,
                GenericName = createMedicineDTO.GenericName,
                Company = createMedicineDTO.Company
            };
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            // Return a DTO, not the raw model
            var medicineDTO = new MedicineDTO
            {
                MedicineId = medicine.MedicineId,
                MedicineType = medicine.MedicineType,
                MedicineName = medicine.MedicineName,
                GenericName = medicine.GenericName,
                Company = medicine.Company
            };
            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.MedicineId }, medicineDTO);
        }

        // PUT: api/Medicines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicine(int id, UpdateMedicineDTO updateMedicineDTO)
        {
            if (id != updateMedicineDTO.MedicineId)
            {
                return BadRequest();
            }

            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            medicine.MedicineType = updateMedicineDTO.MedicineType;
            medicine.MedicineName = updateMedicineDTO.MedicineName;
            medicine.GenericName = updateMedicineDTO.GenericName;
            medicine.Company = updateMedicineDTO.Company;

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
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

        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.MedicineId == id);
        }
    }

}
