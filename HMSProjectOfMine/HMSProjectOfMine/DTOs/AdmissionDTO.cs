using HMSProjectOfMine.Models;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.DTOs
{
    public class AdmissionCreateDTO
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int BedId { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? NurseName { get; set; }
        public string? ReferredBy { get; set; }
        public string? Floor { get; set; }
        public decimal ChargePerDay { get; set; }
        public decimal AdmissionFee { get; set; }
    }


    public class AdmissionDTO
    {
        public int AdmissionId { get; set; }

        public int PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }

        public int DoctorId { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLastName { get; set; }

        public int BedId { get; set; }
        public string BedNumber { get; set; }
        public bool IsOccupied { get; set; }

        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }

        public string? NurseName { get; set; }
        public string? ReferredBy { get; set; }
        public string? Floor { get; set; }

        public decimal ChargePerDay { get; set; }
        public decimal AdmissionFee { get; set; }
    }
}
