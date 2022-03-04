using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Tour
    {
        [Key]
        public UInt32 TId { get; set; }

        [Required(ErrorMessage = "Tour name is required!")]
        [RegularExpression("[a-zA-Z ]+", ErrorMessage = "Tour name should only contain alphabets!")]
        public string TourName { get; set; }

        [Required(ErrorMessage = "Tour Description is required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Seats are required!")]
        [RegularExpression("^[0-9]+", ErrorMessage = "Invalid number of seats")]
        public Int32 TotalSeats { get; set; } = 0;

        [RegularExpression("^[0-9]+", ErrorMessage = "Invalid price")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } = 0;

        [RegularExpression("^[0-9]+", ErrorMessage = "Invalid price")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceInfant { get; set; } = 0;

        [RegularExpression("^[0-9]+", ErrorMessage = "Invalid price")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PriceChildren { get; set; } = 0;

        [ForeignKey("TourType")]
        public UInt32 TourTypeId { get; set; } = 1;

        [RegularExpression("^[0-9]+", ErrorMessage = "Invalid number of days")]
        public UInt32 NoOfDays { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }

    public class TourCities
    {
        [Key]
        public UInt32 TcId { get; set; }

        [ForeignKey("Tour")]
        public UInt32 TourId { get; set; }
        [ForeignKey("City")]
        public UInt32 CityId { get; set; }

        //public UInt32 IsActive { get; set; } = 1;
    }

    public class TourType
    {
        [Key]
        public UInt32 TtId { get; set; }

        public string TtName { get; set; }

        public string Photo { get; set; } = "pexels-arianna-tavaglione-5984075.jpg";
    }

    public class TourDates
    {
        [Key]
        public UInt32 TdId { get; set; }
        [ForeignKey("Tour")]
        public UInt32 TourId { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }
    }

    public class TourPhotos
    {
        [Key]
        public UInt32 TpId { get; set; }
        [ForeignKey("Tour")]
        public UInt32 TourId { get; set; }
        [ForeignKey("Photo")]
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

        [ForeignKey("Tour")]
        public UInt32 TourId { get; set; }

        public UInt32 Day { get; set; } = 1;

        /*[ForeignKey("Landmark")]
        public UInt32 LandmarkId { get; set; }*/

        [ForeignKey("City")]
        public UInt32 CityId { get; set; }

        public string Description { get; set; } = "Enjoy";
    }
}
