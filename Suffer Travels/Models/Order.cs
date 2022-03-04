using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Order
    {
        [Key]
        public uint OId { get; set; }
        [Column(TypeName = "Int")]
        public int TotalAdults { get; set; }
        [Column(TypeName = "Int")]
        public int TotalChildrens { get; set; }
        [Column(TypeName = "Int")]
        public int TotalInfants { get; set; }
        [Column(TypeName = "Decimal(10, 2)")]
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Payment { get; set; } = "Pending";
        public uint UserId { get; set; }
    }

    public class OrderPeople
    {
        [Key]
        public uint OpId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Proof { get; set; }
        public string ProofId { get; set; }
        [ForeignKey("Order")]
        public uint OrderId { get; set; }
    }

    public class OrderTour
    {
        [Key]
        public uint OtId { get; set; }
        [ForeignKey("Order")]
        public uint OrderId { get; set; }
        [ForeignKey("Tour")]
        public uint TourId { get; set; }
        
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
    }

    public class OrderHotel
    {
        [Key]
        public uint OhId { get; set; }
        [Column(TypeName = "Int")]
        public int NoOfRooms { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [ForeignKey("Order")]
        public uint OrderId { get; set; }
        [ForeignKey("HotelRooms")]
        public uint HrId { get; set; }
    }

    public class OrderVehicle
    {
        [Key]
        public uint OvId { get; set; }
        [Column(TypeName = "Decimal(10, 2)")]
        public decimal Price { get; set; }
        [ForeignKey("Order")]
        public uint OrderId { get; set; }
        [ForeignKey("VehicleInfo")]
        public uint VehicleInfoId { get; set; }
    }

    public class Payment
    {
        [Key]
        public uint PId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public uint RefId { get; set; }
        [Column(TypeName = "Decimal(10, 2)")]
        public decimal Amount { get; set; }

        [ForeignKey("Order")]
        public uint OrderId { get; set; }
    }

}
