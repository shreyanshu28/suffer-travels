using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Tour
    {
        [Key]
        public UInt32 TId { get; set; }

        public string TourName { get; set; }
        public string Description { get; set; }

        public Int32 TotalSeats { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } = 0;

        public UInt32 TourTypeId { get; set; } = 1;

        public UInt32 NoOfDays { get; set; } = 1;        

    }

    public class TourCities
    {
        [Key]
        public UInt32 TcId { get; set; }

        public UInt32 TourId { get; set; }

        public UInt32 CityId { get; set; }

        //public UInt32 IsActive { get; set; } = 1;
    }

    public class TourType
    {
        [Key]
        public UInt32 TtId { get; set; }

        public string TtName { get; set; }

        public string Photo { get; set; } = "";
    }

    public class TourDates
    {
        [Key]
        public UInt32 TdId { get; set; }

        public UInt32 TourId { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }
    }

    public class TourPhotos
    {
        [Key]
        public UInt32 TpId { get; set; }

        public UInt32 TourId { get; set; }

        public UInt32 PhotoId { get; set; }
    }

    public class MealCombo
    {
        [Key]
        public UInt32 McId { get; set; }

        public string MealType { get; set; }
    }

    public class TourItinerary
    {
        [Key]
        public UInt32 TiId { get; set; }

        public UInt32 TourId { get; set; }

        public UInt32 Day { get; set; } = 1;

        public UInt32 LandmarkId { get; set; }

        public UInt32 MealComboId { get; set; }
    }
}
