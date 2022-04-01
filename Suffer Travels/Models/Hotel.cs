using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Hotel
    {
        [Key]
        public UInt32 HId { get; set; }
        [Required(ErrorMessage = "First name is required!")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Hotel name should only contain alphabets!")]
        public string HName { get; set; }

        //HOTELADDRESS NU PRIMARY KEY AREAID MA SAVE KARVANU
        public UInt32 AreaId { get; set; }
        [Required(ErrorMessage = "Contact number is required!")]
        [RegularExpression("^[6-9][0-9]{9,9}", ErrorMessage = "Please enter valid contact number")]
        public long ContactNo { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 HotelType { get; set; }

    }

    public class HotelRooms
    {
        [Key]
        public UInt32 HrId { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 TotalRooms { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 Capacity { get; set; } = 2;

        //PRICE PER ROOM
        [Column(TypeName = "decimal(10, 2)")]
        public UInt32 Price { get; set; } = 500;

        public UInt32 HId { get; set; }
    }

    public class HotelPhotos
    {
        [Key]
        public UInt32 HpId { get; set; }

        public UInt32 PID { get; set; }

        public UInt32 HId { get; set; } 
    }

    public class HotelAddress
    {
        [Key]
        public UInt32 HaId { get; set; }
        [Required(ErrorMessage = "Address Line 1 is required")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public UInt32 CityId { get; set; }

    }
}
