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
    public class AdmissionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdmissionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Admissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmissionDTO>>> GetAdmissions()
        {
            try
            {
                var admissions = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Bed)
                .Select(a => new AdmissionDTO
                {
                    AdmissionId = a.AdmissionId,
                    PatientId = a.PatientId,
                    PatientFirstName = a.Patient.FirstName,
                    PatientLastName = a.Patient.LastName,
                    DoctorId = a.DoctorId,
                    DoctorFirstName = a.Doctor.FirstName,
                    DoctorLastName = a.Doctor.LastName,
                    BedId = a.BedId,
                    BedNumber = a.Bed.BedNumber,
                    IsOccupied = a.Bed.IsOccupied,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    NurseName = a.NurseName,
                    ReferredBy = a.ReferredBy,
                    Floor = a.Floor,
                    ChargePerDay = a.ChargePerDay,
                    AdmissionFee = a.AdmissionFee
                }).ToListAsync();

                return admissions;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Admissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmissionDTO>> GetAdmission(int id)
        {
            try
            {
                var admission = await _context.Admissions
                 .Include(a => a.Patient)
                 .Include(a => a.Doctor)
                 .Include(a => a.Bed)
                 .FirstOrDefaultAsync(a => a.AdmissionId == id);
                if (admission == null)
                {
                    return NotFound();
                }

                var admissionDTO = new AdmissionDTO
                {
                    AdmissionId = admission.AdmissionId,
                    PatientId = admission.PatientId,
                    DoctorId = admission.DoctorId,
                    BedId = admission.BedId,
                    AdmissionDate = admission.AdmissionDate,
                    DischargeDate = admission.DischargeDate,
                    NurseName = admission.NurseName,
                    ReferredBy = admission.ReferredBy,
                    Floor = admission.Floor,
                    ChargePerDay = admission.ChargePerDay,
                    AdmissionFee = admission.AdmissionFee
                };

                return admissionDTO;
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // PUT: api/Admissions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmission(int id, AdmissionDTO admissionDTO)
        {
            if (id != admissionDTO.AdmissionId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var admission = await _context.Admissions.FindAsync(id);

            if (admission == null)
            {
                return NotFound();
            }

            admission.PatientId = admissionDTO.PatientId;
            admission.DoctorId = admissionDTO.DoctorId;
            admission.BedId = admissionDTO.BedId;
            admission.AdmissionDate = admissionDTO.AdmissionDate;
            admission.DischargeDate = admissionDTO.DischargeDate;
            admission.NurseName = admissionDTO.NurseName;
            admission.ReferredBy = admissionDTO.ReferredBy;
            admission.Floor = admissionDTO.Floor;
            admission.ChargePerDay = admissionDTO.ChargePerDay;
            admission.AdmissionFee = admissionDTO.AdmissionFee;

            _context.Entry(admission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }

            return NoContent();
        }



        // GET: api/Admissions/PatientsDropdown
        [HttpGet("PatientsDropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatientDropdown()
        {
            var patients = await _context.Patients
                .Select(p => new { p.PatientId, p.FirstName, p.LastName })
                .ToListAsync();

            return Ok(patients); // Return the list directly without wrapping it in an object
        }

        // GET: api/Admissions/DoctorsDropdown
        [HttpGet("DoctorsDropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetDoctorDropdown()
        {
            var doctors = await _context.Doctors
                .Select(d => new { d.DoctorId, d.FirstName, d.LastName })
                .ToListAsync();

            return Ok(doctors); // Return the list directly without wrapping it in an object
        }

        // GET: api/Admissions/BedsDropdown
        [HttpGet("BedsDropdown")]
        public async Task<ActionResult<IEnumerable<object>>> GetBedDropdown()
        {
            var beds = await _context.Beds
                .Select(b => new
                {
                    b.BedId,
                    Info = $"Bed {b.BedNumber} - Ward {b.Ward.WardName}"
                })
                .ToListAsync();

            return Ok(beds); // Return the list directly without wrapping it in an object
        }



        // POST: api/Admissions
        [HttpPost]
        public async Task<ActionResult<AdmissionCreateDTO>> PostAdmission([FromBody] AdmissionCreateDTO admissionDTO)
        {

            if (!await _context.Patients.AnyAsync(p => p.PatientId == admissionDTO.PatientId))
                return BadRequest("Invalid Patient ID");

            if (!await _context.Doctors.AnyAsync(d => d.DoctorId == admissionDTO.DoctorId))
                return BadRequest("Invalid Doctor ID");



            var admission = new Admission
            {
                PatientId = admissionDTO.PatientId,
                DoctorId = admissionDTO.DoctorId,
                BedId = admissionDTO.BedId,
                AdmissionDate = admissionDTO.AdmissionDate,
                DischargeDate = admissionDTO.DischargeDate,
                NurseName = admissionDTO.NurseName,
                ReferredBy = admissionDTO.ReferredBy,
                Floor = admissionDTO.Floor,
                ChargePerDay = admissionDTO.ChargePerDay,
                AdmissionFee = admissionDTO.AdmissionFee
            };

            _context.Admissions.Add(admission);

            // Update bed occupancy
            //bed.IsOccupied = true;

            await _context.SaveChangesAsync();


            return CreatedAtAction("GetAdmission", new { id = admission.AdmissionId }, admissionDTO);
        }

        // DELETE: api/Admissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmission(int id)
        {
            try
            {
                var admission = await _context.Admissions.FindAsync(id);
                if (admission == null)
                {
                    return NotFound();
                }

                _context.Admissions.Remove(admission);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        private bool AdmissionExists(int id)
        {
            return _context.Admissions.Any(e => e.AdmissionId == id);
        }
    }
}
