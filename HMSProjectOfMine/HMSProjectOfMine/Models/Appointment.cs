using HMSProjectOfMine.Enums;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        // Foreign keys
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? AdmissionId { get; set; }
        public int? TokenId { get; set; }

        // Appointment details
        public DateTime AppointmentDate { get; set; }
        public string? PatientPhone { get; set; }
        public string? ReferralCode { get; set; }

        // Status and type
        public AppointmentStatus AppointmentStatus { get; set; } = AppointmentStatus.Scheduled;
        public AppointmentType AppointmentType { get; set; } = AppointmentType.General;

        // Navigation properties
        public virtual Admission? Admission { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual Token? Token { get; set; }
    }


   
}
