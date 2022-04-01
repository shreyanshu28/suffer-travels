using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Hotel
    {
        [Key]
        public UInt32 HId { get; set; }

        public string HName { get; set; }

        //HOTELADDRESS NU PRIMARY KEY AREAID MA SAVE KARVANU
        public UInt32 AreaId { get; set; }

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
        public UInt32 Price { get; set; }

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

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public UInt32 CityId { get; set; }

    }
}
