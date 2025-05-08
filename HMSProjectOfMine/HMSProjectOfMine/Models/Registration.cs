using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMSProjectOfMine.Enums;

namespace HMSProjectOfMine.Models
{
    [Table("Registrations")]
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        [Required]
        [MaxLength(30)]
        [Column(TypeName = "VARCHAR(30)")]
        public string RegistrationNo { get; set; } = null!;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDate { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string? ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DateOfBirth { get; set; }
        
        public BloodType BloodType { get; set; }

        [Required]
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR(15)")]
        public string PhoneNo { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR(255)")]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string EmergencyContactName { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        [Column(TypeName = "VARCHAR(15)")]
        public string EmergencyContactPhone { get; set; } = null!;

        [Column(TypeName = "nvarchar(10)")] // Maps to VARCHAR(10) in SQL Server
        public MaritalStatus? MaritalStatus { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "VARCHAR(100)")]
        public string? Occupation { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "VARCHAR(50)")]
       
        public string? Nationality { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR(255)")]
        public string Allergies { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; } = true;

        [Column(TypeName = "VARCHAR(MAX)")]
        public string? Notes { get; set; }

        public virtual Patient Patient { get; set; }= null!;
    }

    //[Table("BloodGroups")]
    //public class BloodGroup
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int BloodGroupId { get; set; }

    //    [Required]
    //    [Column(TypeName = "nvarchar(10)")]
    //    public BloodType BloodType { get; set; }

    //    [InverseProperty("BloodGroup")]
    //    public virtual ICollection<Registration>? Registrations { get; set; }
   // }

   
}
