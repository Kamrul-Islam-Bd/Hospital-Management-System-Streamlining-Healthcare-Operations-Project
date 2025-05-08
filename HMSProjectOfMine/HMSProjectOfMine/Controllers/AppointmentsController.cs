using HMSProjectOfMine.Data;
using HMSProjectOfMine.DTOs;
using HMSProjectOfMine.Models;
using HMSProjectOfMine.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMSProjectOfMine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {

        private readonly AppointmentService _appointmentService;
        private readonly AppDbContext _context;

        public AppointmentsController(AppointmentService appointmentService, AppDbContext context)
        {
            _appointmentService = appointmentService;
            _context = context;
        }

        // POST: api/Appointments/create-with-patient
        [HttpPost("create-with-patient")]
        public async Task<ActionResult<Appointment>> CreateAppointmentWithPatient([FromBody] AppointmentDTO dto)
        {
            try
            {
                var appointment = await _appointmentService.CreateAppointmentAsync(dto);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentId }, appointment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Select(a => new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    AdmissionId = a.AdmissionId,
                    TokenId = a.TokenId,
                    AppointmentDate = a.AppointmentDate,
                    PatientPhone = a.PatientPhone,
                    ReferralCode = a.ReferralCode,
                    AppointmentStatus = a.AppointmentStatus,
                    AppointmentType = a.AppointmentType
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDTO>> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound();

            var dto = new AppointmentDTO
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AdmissionId = appointment.AdmissionId,
                TokenId = appointment.TokenId,
                AppointmentDate = appointment.AppointmentDate,
                PatientPhone = appointment.PatientPhone,
                ReferralCode = appointment.ReferralCode,
                AppointmentStatus = appointment.AppointmentStatus,
                AppointmentType = appointment.AppointmentType
            };

            return Ok(dto);
        }

        // PUT: api/Appointments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentDTO dto)
        {
            if (id != dto.AppointmentId)
                return BadRequest("Appointment ID mismatch.");

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.PatientId = dto.PatientId;
            appointment.DoctorId = dto.DoctorId;
            appointment.AdmissionId = dto.AdmissionId;
            appointment.TokenId = dto.TokenId;
            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.PatientPhone = dto.PatientPhone;
            appointment.ReferralCode = dto.ReferralCode;
            appointment.AppointmentStatus = dto.AppointmentStatus;
            appointment.AppointmentType = dto.AppointmentType;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
