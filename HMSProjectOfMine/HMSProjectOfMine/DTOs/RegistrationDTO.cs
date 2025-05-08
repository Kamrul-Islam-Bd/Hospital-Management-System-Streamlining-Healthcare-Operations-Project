using HMSProjectOfMine.Enums;

namespace HMSProjectOfMine.DTOs
{
    public class RegistrationDTO
    {
        public int RegistrationId { get; set; }

        public string RegistrationNo { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }

        public int PatientId { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime DateOfBirth { get; set; }

       public BloodType BloodType { get; set; }

        public string PhoneNo { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string EmergencyContactName { get; set; } = null!;

        public string EmergencyContactPhone { get; set; } = null!;

        public MaritalStatus? MaritalStatus { get; set; }

        public string? Occupation { get; set; }

        public string? Nationality { get; set; }

        public string Allergies { get; set; } = null!;

        public bool IsActive { get; set; }

        public string? Notes { get; set; }

        public IFormFile? Image { get; set; }
    }

}
