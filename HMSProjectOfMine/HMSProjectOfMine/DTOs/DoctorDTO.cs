using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.DTOs
{
    public class DoctorDTO
    {
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
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public string? ImageUrl { get; set; } // stores URL/path to the image

        public IFormFile? ImageFile { get; set; } // actual uploaded file
        public string DepartmentName { get; set; } = null!;
        public string SpecializationName { get; set; } = null!;
    }
}
