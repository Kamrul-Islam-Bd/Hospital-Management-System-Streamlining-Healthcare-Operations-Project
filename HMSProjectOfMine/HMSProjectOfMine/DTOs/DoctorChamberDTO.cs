namespace HMSProjectOfMine.DTOs
{
    public class DoctorChamberDTO
    {
        public int DoctorChamberId { get; set; }

        public int DoctorId { get; set; }

        public int ChamberId { get; set; }

        public string AvailableTime { get; set; } = null!;
    }
}
