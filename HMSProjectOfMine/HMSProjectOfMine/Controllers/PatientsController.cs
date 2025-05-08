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
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            return await _context.Patients.Select(p => new PatientDTO
            {
                PatientId = p.PatientId,
                PatientNo = p.PatientNo,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Gender = p.Gender,
                Age = p.Age,
                FirstVisitDate = p.FirstVisitDate,
                PatientType = p.PatientType,
                VisitType = p.VisitType
            }).ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientDTO = new PatientDTO
            {
                PatientId = patient.PatientId,
                PatientNo = patient.PatientNo,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Gender = patient.Gender,
                Age = patient.Age,
                FirstVisitDate = patient.FirstVisitDate,
                PatientType = patient.PatientType,
                VisitType = patient.VisitType
            };

            return patientDTO;
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<PatientDTO>> PostPatient([FromBody] PatientDTO patientDTO)
        {
            var patient = new Patient
            {
                PatientNo = patientDTO.PatientNo,
                FirstName = patientDTO.FirstName,
                LastName = patientDTO.LastName,
                Gender = patientDTO.Gender,
                Age = patientDTO.Age,
                RegistrationId = patientDTO.RegistrationId,
                FirstVisitDate = patientDTO.FirstVisitDate,
                PatientType = patientDTO.PatientType,
                VisitType = patientDTO.VisitType
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            patientDTO.PatientId = patient.PatientId; // Important to return the generated ID

            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patientDTO);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientDTO patientDTO)
        {
            if (id != patientDTO.PatientId)
            {
                return BadRequest();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            patient.PatientNo = patientDTO.PatientNo;
            patient.FirstName = patientDTO.FirstName;
            patient.LastName = patientDTO.LastName;
            patient.Gender = patientDTO.Gender;
            patient.Age = patientDTO.Age;
            patient.RegistrationId = patientDTO.RegistrationId;
            patient.PatientType = patientDTO.PatientType;
            patient.FirstVisitDate = patientDTO.FirstVisitDate;
            patient.VisitType = patientDTO.VisitType;

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
