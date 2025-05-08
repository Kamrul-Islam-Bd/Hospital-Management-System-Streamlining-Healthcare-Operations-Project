using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Enums
{
    public enum PatientType
    {
        [Display(Name = "Indoor")]
        Indoor,

        [Display(Name = "Outdoor")]
        Outdoor
    }

    public enum Gender
    {
        [Display(Name = "Male")]
        Male,

        [Display(Name = "Female")]
        Female,

        [Display(Name = "Other")]
        Other
    }
    public enum VisitType
    {
        [Display(Name = "First Visit")]
        FirstVisit,

        [Display(Name = "Follow Up")]
        FollowUp,

        [Display(Name = "Test Review")]
        TestReview,

        [Display(Name = "Others")]
        Others
    }

    public enum AppointmentStatus
    {
        [Display(Name = "Scheduled")]
        Scheduled,
        [Display(Name = "Arrived")]// Appointment booked but not yet arrived
        Arrived,
        [Display(Name = "InConsultation")]// Patient arrived at facility
        InConsultation,
        [Display(Name = "Completed")]// Currently with doctor
        Completed,
        [Display(Name = "Cancelled")]// Consultation finished
        Cancelled,
        [Display(Name = "NoShow")]// Appointment was cancelled
        NoShow          // Patient didn't arrive
    }

    public enum AppointmentType
    {
        [Display(Name = "General")]
        General,
        [Display(Name = "Follow Up")]// Regular checkup
        FollowUp,
        [Display(Name = "Emergency")]// Follow-up visit
        Emergency,
        [Display(Name = "Post Operative")]// Urgent care
        PostOperative,
        [Display(Name = "Preventive")]// After surgery check
        Preventive,
        [Display(Name = "Referral")]// Vaccination/checkup
        Referral      // Referred from another doctor
    }

    public enum MaritalStatus
    {
        [Display(Name = "Single")]
        Single,

        [Display(Name = "Married")]
        Married,

        [Display(Name = "Divorced")]
        Divorced,

        [Display(Name = "Widowed")]
        Widowed
    }
    public enum BloodType
    {
        [Display(Name = "A+")] A_POS,
        [Display(Name = "A-")] A_NEG,
        [Display(Name = "B+")] B_POS,
        [Display(Name = "B-")] B_NEG,
        [Display(Name = "O+")] O_POS,
        [Display(Name = "O-")] O_NEG,
        [Display(Name = "AB+")] AB_POS,
        [Display(Name = "AB-")] AB_NEG
    }

    public enum WardType
    {
        [Display(Name = "General")]
        General,

        [Display(Name = "ICU")]
        ICU,

        [Display(Name = "Maternity")]
        Maternity,


        [Display(Name = "Cabin")]
        Cabin
    }
}
