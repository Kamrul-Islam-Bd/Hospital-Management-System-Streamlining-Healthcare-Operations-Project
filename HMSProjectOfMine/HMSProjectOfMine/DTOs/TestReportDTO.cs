namespace HMSProjectOfMine.DTOs
{
    public class TestReportDTO
    {
        public int TestReportId { get; set; }
        public int PrescriptionTestId { get; set; }
        public string? TestResult { get; set; }
        public bool IsFinalized { get; set; }
    }


}
