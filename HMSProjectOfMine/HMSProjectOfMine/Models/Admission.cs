namespace HMSProjectOfMine.Models
{
    public class Admission
    {
        public int AdmissionId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int BedId { get; set; }
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime? DischargeDate { get; set; }
        public string? NurseName { get; set; }
        public string ReferredBy { get; set; } = null!;
        public string? Floor { get; set; }
        public decimal ChargePerDay { get; set; }
        public decimal AdmissionFee { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();
        public virtual Bed Bed { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
    }
}
