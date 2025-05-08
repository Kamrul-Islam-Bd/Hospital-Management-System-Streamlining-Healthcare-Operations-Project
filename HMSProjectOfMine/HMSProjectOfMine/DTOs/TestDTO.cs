using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.DTOs
{
    public class TestDTO
    {
        public int TestId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TestName { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
