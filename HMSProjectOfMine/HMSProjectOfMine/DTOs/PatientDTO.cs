using HMSProjectOfMine.Enums;
using HMSProjectOfMine.Models;

namespace HMSProjectOfMine.DTOs
{
    public class PatientDTO
    {
        public int PatientId { get; set; }

        public string PatientNo { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public Gender Gender { get; set; }

        public int Age { get; set; }

        public DateTime FirstVisitDate { get; set; }

        public PatientType PatientType { get; set; }

        public VisitType? VisitType { get; set; }
        public int? RegistrationId { get; set; }


        public Registration? Registration { get; set; }

        //  Possibly include if needed

        //public ICollection<Token>? Tokens { get; set; }
        //public ICollection<Admission>? Admissions { get; set; }
        //public ICollection<Appointment>? Appointments { get; set; } //  Include if needed, but be mindful ocircular references and data volume

    }
}
