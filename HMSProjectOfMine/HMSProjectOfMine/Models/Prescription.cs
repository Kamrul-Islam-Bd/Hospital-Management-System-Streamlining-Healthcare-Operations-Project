using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Models
{

    [Table("Prescriptions")]
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public DateTime PrescriptionDate { get; set; }

        public DateTime? NextVisitDate { get; set; }

        [MaxLength(2000)]
        public string? Assessment { get; set; }
        [ForeignKey("Token")]
        public int TokenId { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Token Token { get; set; } = null!;

        [InverseProperty("Prescription")]
        public virtual ICollection<PrescriptionMedicine>? PrescriptionMedicines { get; set; } = new List<PrescriptionMedicine>();

        [InverseProperty("Prescription")]
        public virtual ICollection<PrescriptionTest>? PrescriptionTests { get; set; } = new List<PrescriptionTest>();

        [InverseProperty("Prescription")]
        public virtual ICollection<PrescriptionDiagnosis>? PrescriptionDiagnosises { get; set; } = new List<PrescriptionDiagnosis>();

        [InverseProperty("Prescription")]
        public virtual ICollection<PhysicalSymptom>? PhysicalSymptoms { get; set; } = new List<PhysicalSymptom>();

        [InverseProperty("Prescription")]
        public virtual ICollection<PrescriptionAdvice>? PrescriptionAdvices { get; set; } = new List<PrescriptionAdvice>();
    }

    [Table("PrescriptionMedicines")]
    public class PrescriptionMedicine
    {
        [Key]
        public int PrescriptionMedicineId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }


        [MaxLength(100)]
        public string? Dosage { get; set; }

        [MaxLength(100)]
        public string? Frequency { get; set; }

        [MaxLength(100)]
        public string? Duration { get; set; }

        [Required]
        public virtual Prescription Prescription { get; set; } = null!;
        [Required]
        public virtual Medicine Medicine { get; set; } = null!;
    }

    [Table("PrescriptionTests")]
    public class PrescriptionTest
    {
        [Key]
        public int PrescriptionTestId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }

        [Required]
        public virtual Prescription Prescription { get; set; } = null!;

        [Required]
        public virtual Test Test { get; set; } = null!;

        [InverseProperty("PrescriptionTest")]
        public virtual ICollection<TestReport>? TestReports { get; set; } = new List<TestReport>();
    }

    [Table("PrescriptionDiagnoses")]
    public class PrescriptionDiagnosis
    {
        [Key]
        public int PrescriptionDiagnosisId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [MaxLength(500)]
        public string? DiagnosisTitle { get; set; }

        [Required]
        public virtual Prescription Prescription { get; set; } = null!;
    }

    [Table("PrescriptionAdvices")]
    public class PrescriptionAdvice
    {
        [Key]
        public int PrescriptionAdviceId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [MaxLength(2000)]
        public string? Advice { get; set; }

        [Required]
        public virtual Prescription Prescription { get; set; } = null!;
    }

    [Table("PhysicalSymptoms")]
    public class PhysicalSymptom
    {
        [Key]
        public int PhysicalSymptomId { get; set; }

        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }

        [Required]
        [MaxLength(500)]
        public string SymptomDescription { get; set; }

        [Required]
        public virtual Prescription Prescription { get; set; } = null!;
    }

}
