using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMSProjectOfMine.Enums;

namespace HMSProjectOfMine.Models
{
    [Table("Doctors")]
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        public int DepartmentId { get; set; }

        public int? SpecializationId { get; set; }

        [MaxLength(20)]
        [Phone]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(255)]
        [Url]
        public string? ImageUrl { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual Specialization? Specialization { get; set; }


        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<DoctorChamber>? DoctorChambers { get; set; } = new List<DoctorChamber>();
        public virtual ICollection<DoctorFee>? DoctorFees { get; set; } = new List<DoctorFee>();
        public virtual ICollection<DoctorSchedule>? DoctorSchedules { get; set; } = new List<DoctorSchedule>();
        public virtual ICollection<Admission>? Admissions { get; set; } = new List<Admission>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();

    }

    [Table("Specializations")]
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SpecializationName { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }

    [Table("Departments")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string DepartmentName { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
    public class DoctorChamber
    {
        public int DoctorChamberId { get; set; }

        public int DoctorId { get; set; }

        public int ChamberId { get; set; }

        public string AvailableTime { get; set; } = null!;

        public virtual Chamber Chamber { get; set; } = null!;

        public virtual Doctor Doctor { get; set; } = null!;
    }

    public class DoctorSchedule
    {
        public int DoctorScheduleId { get; set; }

        public int DoctorId { get; set; }

        public int ChamberId { get; set; }

        public string DaysOfWeek { get; set; } = null!;

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public virtual Chamber Chamber { get; set; } = null!;

        public virtual Doctor Doctor { get; set; } = null!;
    }

    public class DoctorFee
    {
        public int DoctorFeeId { get; set; }

        public int DoctorId { get; set; }

        public decimal Fees { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateTime? EffectiveDate { get; set; } = DateTime.Now;

        public decimal? ChargedFee { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;

        public VisitType VisitType { get; set; } 
    }
}
