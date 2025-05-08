namespace HMSProjectOfMine.DTOs
{
    public class TestBillingDetailDTO
    {
        public int? TestBillingDetailId { get; set; } // For update support
        public int TestId { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
