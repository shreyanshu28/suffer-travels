using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Hotel
    {
        [Key]
        public UInt32 HId { get; set; }

        public string Hname { get; set; }

        public UInt32 AreaId { get; set; }

        public long ContactNo { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 HotelType { get; set; }

    }

    public class HotelRooms
    {
        [Key]
        public UInt32 HrId { get; set; }

        public UInt32 HId { get; set; }

        public UInt32 AreaId { get; set; }

        public long ContactNo { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 TotalRooms { get; set; }

        [Column(TypeName = "Int")]
        public UInt32 Capacity { get; set; } = 2;

        //PRICE PER ROOM
        [Column(TypeName = "decimal(10, 2)")]
        public UInt32 Price { get; set; }
    }

    public class HotelPhotos
    {
        [Key]
        public UInt32 HaId { get; set; }

        public UInt32 HId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public UInt32 Aid { get; set; }
    }
}
