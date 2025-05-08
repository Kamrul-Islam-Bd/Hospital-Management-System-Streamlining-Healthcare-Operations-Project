using HMSProjectOfMine.Enums;
using HMSProjectOfMine.Models;

namespace HMSProjectOfMine.DTOs
{
    public class DoctorFeeDTO
    {
        public int DoctorFeeId { get; set; }

        public int DoctorId { get; set; }

        public decimal Fees { get; set; }

        public decimal? DiscountAmount { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public decimal? ChargedFee { get; set; }

        public VisitType VisitType { get; set; }
    }


}
