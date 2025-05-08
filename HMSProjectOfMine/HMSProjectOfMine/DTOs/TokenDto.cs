using HMSProjectOfMine.Models;

namespace HMSProjectOfMine.DTOs
{
    public class TokenDto
    {
        public int TokenId { get; set; }
        public string TokenNumber { get; set; } = null!;

        public decimal TokenFee { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public int ChamberId { get; set; }
        public DateTime? IssueTime { get; set; } = DateTime.Now;

        public virtual ICollection<Prescription>? Prescriptions { get; set; } = new List<Prescription>();


    }
}
