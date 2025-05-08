namespace HMSProjectOfMine.DTOs
{
    public class DoctorScheduleDTO
    {
        public int DoctorScheduleId { get; set; }

        public int DoctorId { get; set; }

        public int ChamberId { get; set; }

        public string DaysOfWeek { get; set; } = null!;

        public TimeOnly? StartTime { get; set; } // hh:mm:ss (07:10:00)

        public TimeOnly? EndTime { get; set; }  // hh:mm:ss (07:10:00)
    }
}
