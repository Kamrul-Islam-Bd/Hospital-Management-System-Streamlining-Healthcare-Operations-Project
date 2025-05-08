using HMSProjectOfMine.Data;
using HMSProjectOfMine.DTOs;
using HMSProjectOfMine.Enums;
using HMSProjectOfMine.Models;
using Microsoft.EntityFrameworkCore;

namespace HMSProjectOfMine.Services
{

    public class AppointmentService
    {
        private readonly AppDbContext _context;

        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Appointment> CreateAppointmentAsync(AppointmentDTO appointmentDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if patient exists by PatientNo
                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientNo == appointmentDTO.PatientNo);

                // If patient does not exist, create a new one
                if (patient == null)
                {
                    patient = new Patient
                    {
                        PatientNo = appointmentDTO.PatientNo,
                        FirstName = appointmentDTO.FirstName,
                        LastName = appointmentDTO.LastName,
                        Gender = appointmentDTO.Gender,
                        Age = appointmentDTO.Age,
                        FirstVisitDate = DateTime.Now,
                        PatientType = PatientType.Outdoor,
                        VisitType = VisitType.FirstVisit
                    };

                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                }

                // Create appointment
                var appointment = new Appointment
                {
                    PatientId = patient.PatientId,
                    DoctorId = appointmentDTO.DoctorId,
                    AppointmentDate = appointmentDTO.AppointmentDate,
                    AppointmentStatus = appointmentDTO.AppointmentStatus,
                    AppointmentType = appointmentDTO.AppointmentType
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return appointment;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error while creating appointment", ex);
            }
        }
    }

}