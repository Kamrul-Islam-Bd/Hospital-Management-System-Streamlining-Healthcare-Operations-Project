using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Models
{
    [Table("Tests")] // Table name mapping
    public class Test
    {
        [Key]
        public int TestId { get; set; }

        [Required]
        [MaxLength(100)] // Column configuration
        public string? TestName { get; set; }

        [Column(TypeName = "decimal(18,2)")] // Column data type
        public decimal Price { get; set; }

        public virtual ICollection<PrescriptionTest> PrescriptionTests { get; set; } = new List<PrescriptionTest>();

        [InverseProperty("Test")]

        public virtual ICollection<TestBillingDetail> TestBillingDetails { get; set; } = new List<TestBillingDetail>();
    }

    [Table("TestReports")]
    public class TestReport
    {
        [Key]
        public int TestReportId { get; set; }

        [ForeignKey("PrescriptionTest")]
        public int PrescriptionTestId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? TestResult { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ReportDate { get; set; }

        public bool IsFinalized { get; set; } = false; // Prevent updates when true

        [Required]
        public virtual PrescriptionTest PrescriptionTest { get; set; } = null!;
    }

    public class TestBilling
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestBillingId { get; set; }

        [Required]
        [StringLength(50)]
        public string BillNo { get; set; } = null!;

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [StringLength(50)]
        public string? RefBy { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal DiscountPercentage { get; set; } = 0;

        public decimal PayableAmount { get; set; } // now settable
        public decimal DueAmount { get; set; }     // now settable

        [Column(TypeName = "decimal(10,2)")]
        public decimal PaidAmount { get; set; } = 0;


        [Required]
        public DateOnly DeliveryDate { get; set; }

        [Required]
        public TimeOnly DeliveryTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual ICollection<TestBillingDetail> TestBillingDetails { get; set; } = new List<TestBillingDetail>();
    }

    public class TestBillingDetail
    {
        [Key]
        public int TestBillingDetailId { get; set; }

        [ForeignKey("TestBilling")]
        public int TestBillingId { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal TotalPrice { get; set; }

        // Navigation properties
        public virtual TestBilling TestBilling { get; set; } = null!;
        public virtual Test Test { get; set; } = null!;
    }
}
