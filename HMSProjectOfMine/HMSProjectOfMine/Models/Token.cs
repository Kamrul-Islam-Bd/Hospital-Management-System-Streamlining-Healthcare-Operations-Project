using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Models
{
    [Table("Tokens")]
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TokenNumber { get; set; } = null!;
        public decimal TokenFee { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public int ChamberId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime IssueTime { get; set; } 

        public virtual Doctor Doctor { get; set; } = null!;

        [Required]
        public virtual Chamber Chamber { get; set; } = null!;

        [Required]
        public virtual Patient Patient { get; set; } = null!;

        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
    }

    [Table("Chambers")]
    public class Chamber
    {
        [Key]
        public int ChamberId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ChamberNo { get; set; } = null!;

        [MaxLength(200)]
        public string? Location { get; set; }

        public virtual ICollection<DoctorChamber> DoctorChambers { get; set; } = new List<DoctorChamber>();

        public virtual ICollection<DoctorSchedule> DoctorSchedules { get; set; } = new List<DoctorSchedule>();

        public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();

    }


}
