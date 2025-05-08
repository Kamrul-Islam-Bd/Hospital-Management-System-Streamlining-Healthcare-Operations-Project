using HMSProjectOfMine.Enums;
using HMSProjectOfMine.Models;

namespace HMSProjectOfMine.DTOs
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }

        public string PatientNo { get; set; } = null!; // ✅ Required for identifying patient

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? AdmissionId { get; set; }
        public int? TokenId { get; set; }

        // Appointment Details
        public DateTime AppointmentDate { get; set; }
        public string? PatientPhone { get; set; }
        public string? ReferralCode { get; set; }

        // Status and Type (Enums)
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Scheduled;
        public AppointmentType AppointmentType { get; set; } = AppointmentType.General;
    }
}
