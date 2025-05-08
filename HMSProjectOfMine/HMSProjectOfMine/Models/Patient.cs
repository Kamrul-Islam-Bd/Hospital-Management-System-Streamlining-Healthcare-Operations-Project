using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using HMSProjectOfMine.Enums;

namespace HMSProjectOfMine.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string PatientNo { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;
        public int? RegistrationId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")] // Store enum as string
        public Gender Gender { get; set; }
        public int Age { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime FirstVisitDate { get; set; }
        public virtual Registration? Registration { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(10)")] // Store enum as string
        public PatientType PatientType { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public VisitType? VisitType { get; set; }

        [InverseProperty("Patient")]
        public virtual ICollection<Token>? Tokens { get; set; } = new List<Token>();
        public virtual ICollection<Admission>? Admissions { get; set; } = new List<Admission>();
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
    }


}
