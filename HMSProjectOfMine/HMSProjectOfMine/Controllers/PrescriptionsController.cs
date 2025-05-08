using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HMSProjectOfMine.DTOs;
using HMSProjectOfMine.Models;
using HMSProjectOfMine.Data;

namespace HMSProjectOfMine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public PrescriptionsController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetAllPrescriptions()
        {
            var dtoList = await _context.Prescriptions
                .AsNoTracking()
                .Select(p => new PrescriptionDTO
                {
                    PrescriptionId = p.PrescriptionId,
                    TokenId = p.TokenId,
                    DoctorId = p.DoctorId,
                    PrescriptionDate = p.PrescriptionDate,
                    NextVisitDate = p.NextVisitDate,
                    Assessment = p.Assessment,
                    PrescriptionMedicines = p.PrescriptionMedicines.Select(m => new PrescriptionMedicineDTO
                    {
                        PrescriptionMedicineId = m.PrescriptionMedicineId,
                        PrescriptionId = m.PrescriptionId,
                        MedicineId = m.MedicineId,
                        Dosage = m.Dosage,
                        Frequency = m.Frequency,
                        Duration = m.Duration
                    }).ToList(),
                    PrescriptionTests = p.PrescriptionTests.Select(t => new PrescriptionTestDTO
                    {
                        PrescriptionTestId = t.PrescriptionTestId,
                        PrescriptionId = t.PrescriptionId,
                        TestId = t.TestId
                    }).ToList(),
                    PrescriptionDiagnosises = p.PrescriptionDiagnosises.Select(d => new PrescriptionDiagnosisDTO
                    {
                        PrescriptionDiagnosisId = d.PrescriptionDiagnosisId,
                        PrescriptionId = d.PrescriptionId,
                        DiagnosisTitle = d.DiagnosisTitle
                    }).ToList(),
                    PhysicalSymptoms = p.PhysicalSymptoms.Select(ps => new PhysicalSymptomDTO
                    {
                        PhysicalSymptomId = ps.PhysicalSymptomId,
                        PrescriptionId = ps.PrescriptionId,
                        SymptomDescription = ps.SymptomDescription
                    }).ToList(),
                    PrescriptionAdvices = p.PrescriptionAdvices.Select(a => new PrescriptionAdviceDTO
                    {
                        PrescriptionAdviceId = a.PrescriptionAdviceId,
                        PrescriptionId = a.PrescriptionId,
                        Advice = a.Advice
                    }).ToList()
                })
                .ToListAsync();

            return Ok(dtoList);
        }



        // 2️⃣ GET Prescription By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescriptionById(int id)
        {
            var p = await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                .Include(p => p.PrescriptionTests)
                .Include(p => p.PrescriptionDiagnosises)
                .Include(p => p.PhysicalSymptoms)
                .Include(p => p.PrescriptionAdvices)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (p == null)
                return NotFound();

            var dto = new PrescriptionDTO
            {
                PrescriptionId = p.PrescriptionId,
                TokenId = p.TokenId,
                DoctorId = p.DoctorId,
                PrescriptionDate = p.PrescriptionDate,
                NextVisitDate = p.NextVisitDate,
                Assessment = p.Assessment,
                PrescriptionMedicines = p.PrescriptionMedicines?.Select(m => new PrescriptionMedicineDTO
                {
                    PrescriptionMedicineId = m.PrescriptionMedicineId,
                    PrescriptionId = m.PrescriptionId,
                    MedicineId = m.MedicineId,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    Duration = m.Duration
                }).ToList(),
                PrescriptionTests = p.PrescriptionTests?.Select(t => new PrescriptionTestDTO
                {
                    PrescriptionTestId = t.PrescriptionTestId,
                    PrescriptionId = t.PrescriptionId,
                    TestId = t.TestId
                }).ToList(),
                PrescriptionDiagnosises = p.PrescriptionDiagnosises?.Select(d => new PrescriptionDiagnosisDTO
                {
                    PrescriptionDiagnosisId = d.PrescriptionDiagnosisId,
                    PrescriptionId = d.PrescriptionId,
                    DiagnosisTitle = d.DiagnosisTitle
                }).ToList(),
                PhysicalSymptoms = p.PhysicalSymptoms?.Select(ps => new PhysicalSymptomDTO
                {
                    PhysicalSymptomId = ps.PhysicalSymptomId,
                    PrescriptionId = ps.PrescriptionId,
                    SymptomDescription = ps.SymptomDescription
                }).ToList(),
                PrescriptionAdvices = p.PrescriptionAdvices?.Select(a => new PrescriptionAdviceDTO
                {
                    PrescriptionAdviceId = a.PrescriptionAdviceId,
                    PrescriptionId = a.PrescriptionId,
                    Advice = a.Advice
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost("validate-medicines")]
        public async Task<ActionResult<bool>> ValidateMedicineIds([FromBody] List<int> medicineIds)
        {
            var existingCount = await _context.Medicines
                .Where(m => medicineIds.Contains(m.MedicineId))
                .CountAsync();

            return existingCount == medicineIds.Count;
        }

        // 3️⃣ POST (Create) Prescription
        [HttpPost]
        public async Task<ActionResult<PrescriptionDTO>> CreatePrescription(PrescriptionDTO dto)
        {

            if (dto.PrescriptionMedicines != null)
            {
                var medicineIds = dto.PrescriptionMedicines.Select(m => m.MedicineId).ToList();
                var existingCount = await _context.Medicines
                    .Where(m => medicineIds.Contains(m.MedicineId))
                    .CountAsync();

                if (existingCount != medicineIds.Count)
                {
                    return BadRequest("One or more medicine IDs are invalid");
                }
            }
            var prescription = new Prescription
            {
                TokenId = dto.TokenId,
                DoctorId = dto.DoctorId,
                PrescriptionDate = dto.PrescriptionDate,
                NextVisitDate = dto.NextVisitDate,
                Assessment = dto.Assessment,
                PrescriptionMedicines = dto.PrescriptionMedicines?.Select(m => new PrescriptionMedicine
                {
                    MedicineId = m.MedicineId,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    Duration = m.Duration
                }).ToList(),
                PrescriptionTests = dto.PrescriptionTests?.Select(t => new PrescriptionTest
                {
                    TestId = t.TestId
                }).ToList(),
                PrescriptionDiagnosises = dto.PrescriptionDiagnosises?.Select(d => new PrescriptionDiagnosis
                {
                    DiagnosisTitle = d.DiagnosisTitle
                }).ToList(),
                PhysicalSymptoms = dto.PhysicalSymptoms?.Select(ps => new PhysicalSymptom
                {
                    SymptomDescription = ps.SymptomDescription
                }).ToList(),
                PrescriptionAdvices = dto.PrescriptionAdvices?.Select(a => new PrescriptionAdvice
                {
                    Advice = a.Advice
                }).ToList()
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            dto.PrescriptionId = prescription.PrescriptionId;
            return CreatedAtAction(nameof(GetPrescriptionById), new { id = dto.PrescriptionId }, dto);
        }

        // 4️⃣ PUT (Update) Prescription
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(int id, PrescriptionDTO dto)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                .Include(p => p.PrescriptionTests)
                .Include(p => p.PrescriptionDiagnosises)
                .Include(p => p.PhysicalSymptoms)
                .Include(p => p.PrescriptionAdvices)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null)
                return NotFound();

            prescription.TokenId = dto.TokenId;
            prescription.DoctorId = dto.DoctorId;
            prescription.PrescriptionDate = dto.PrescriptionDate;
            prescription.NextVisitDate = dto.NextVisitDate;
            prescription.Assessment = dto.Assessment;

            // Delete existing children
            _context.PrescriptionMedicines.RemoveRange(prescription.PrescriptionMedicines);
            _context.PrescriptionTests.RemoveRange(prescription.PrescriptionTests);
            _context.PrescriptionDiagnoses.RemoveRange(prescription.PrescriptionDiagnosises);
            _context.PhysicalSymptoms.RemoveRange(prescription.PhysicalSymptoms);
            _context.PrescriptionAdvices.RemoveRange(prescription.PrescriptionAdvices);

            // Add new children
            prescription.PrescriptionMedicines = dto.PrescriptionMedicines?.Select(m => new PrescriptionMedicine
            {
                MedicineId = m.MedicineId,
                Dosage = m.Dosage,
                Frequency = m.Frequency,
                Duration = m.Duration
            }).ToList();

            prescription.PrescriptionTests = dto.PrescriptionTests?.Select(t => new PrescriptionTest
            {
                TestId = t.TestId
            }).ToList();

            prescription.PrescriptionDiagnosises = dto.PrescriptionDiagnosises?.Select(d => new PrescriptionDiagnosis
            {
                DiagnosisTitle = d.DiagnosisTitle
            }).ToList();

            prescription.PhysicalSymptoms = dto.PhysicalSymptoms?.Select(ps => new PhysicalSymptom
            {
                SymptomDescription = ps.SymptomDescription
            }).ToList();

            prescription.PrescriptionAdvices = dto.PrescriptionAdvices?.Select(a => new PrescriptionAdvice
            {
                Advice = a.Advice
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5️⃣ DELETE Prescription
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.PrescriptionMedicines)
                .Include(p => p.PrescriptionTests)
                .Include(p => p.PrescriptionDiagnosises)
                .Include(p => p.PhysicalSymptoms)
                .Include(p => p.PrescriptionAdvices)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null)
                return NotFound();

            _context.PrescriptionMedicines.RemoveRange(prescription.PrescriptionMedicines);
            _context.PrescriptionTests.RemoveRange(prescription.PrescriptionTests);
            _context.PrescriptionDiagnoses.RemoveRange(prescription.PrescriptionDiagnosises);
            _context.PhysicalSymptoms.RemoveRange(prescription.PhysicalSymptoms);
            _context.PrescriptionAdvices.RemoveRange(prescription.PrescriptionAdvices);

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }
}



