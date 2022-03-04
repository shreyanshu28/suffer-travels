using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Country
    {
        [Key]
        public UInt32 CId { get; set; }

        public string Cname { get; set; }

        public Int32 STDcode { get; set; }
    }

    public class State
    {
        [Key]
        public UInt32 SId { get; set; }
        
        public string Sname { get; set; }

        public UInt32 CountryId { get; set; }

    }

    public class City
    {
        [Key]

        public UInt32 CId { get; set; }

        public string Cname { get; set; }

        public UInt32 StateId { get; set; }

        public string Photo { get; set; } = "";
    }

    public class Area
    {
        [Key]
        public UInt32 AId { get; set; }
        
        public string Aname { get; set; }

        public UInt32 Pincode { get; set; }
        
        public UInt32 CityId { get; set; }
    }

    public class Landmark
    {
        [Key]
        public UInt32 LId { get; set; }

        public string Lname { get; set; }

        public UInt32 CityId { get; set; }

        [ForeignKey("Photo")]
        public UInt32 PhotoId { get; set; }
    }
}
