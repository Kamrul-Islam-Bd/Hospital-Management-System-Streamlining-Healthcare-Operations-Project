using HMSProjectOfMine.Enums;
using System.ComponentModel.DataAnnotations;

namespace HMSProjectOfMine.Models
{
    public class Ward
    {
        public int WardId { get; set; }

        public string WardName { get; set; } = null!;

        public WardType WardType { get; set; }

        public virtual ICollection<Bed> Beds { get; set; } = new List<Bed>();
    }

    public class Bed
    {
        public int BedId { get; set; }

        public int WardId { get; set; }

        public string BedNumber { get; set; } = null!;

        public bool IsOccupied { get; set; }

        public virtual ICollection<Admission> Admissions { get; set; } = new List<Admission>();

        public virtual Ward Ward { get; set; } = null!;
    }
   

}
