using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMSProjectOfMine.DTOs
{
    public class PrescriptionDTO
    {
        public int PrescriptionId { get; set; }
        public int TokenId { get; set; }
        public int DoctorId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime? NextVisitDate { get; set; }
        public string? Assessment { get; set; }
        public List<PrescriptionMedicineDTO>? PrescriptionMedicines { get; set; }
        public List<PrescriptionTestDTO>? PrescriptionTests { get; set; }
        public List<PrescriptionDiagnosisDTO>? PrescriptionDiagnosises { get; set; }
        public List<PhysicalSymptomDTO>? PhysicalSymptoms { get; set; }
        public List<PrescriptionAdviceDTO>? PrescriptionAdvices { get; set; }
    }

    public class PrescriptionMedicineDTO
    {
        public int PrescriptionMedicineId { get; set; }
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }

        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
    }
    public class PrescriptionTestDTO
    {
        public int PrescriptionTestId { get; set; }

        public int PrescriptionId { get; set; }

        public int TestId { get; set; }

    }

    public class PrescriptionDiagnosisDTO
    {
        public int PrescriptionDiagnosisId { get; set; }

        public int PrescriptionId { get; set; }

        public string? DiagnosisTitle { get; set; }
    }

    public class PhysicalSymptomDTO
    {
        public int PhysicalSymptomId { get; set; }
        public int PrescriptionId { get; set; }
        public string SymptomDescription { get; set; } = null!;
    }

    public class PrescriptionAdviceDTO
    {
        public int PrescriptionAdviceId { get; set; }
        public int PrescriptionId { get; set; }
        public string? Advice { get; set; }
    }

}
