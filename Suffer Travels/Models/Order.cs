using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suffer_Travels.Models
{
    public class Order
    {
        [Key]
        public uint OId { get; set; }

        [Column(TypeName = "Int")]
        [Required(ErrorMessage = "Atleast one adult is required")]
        [Range(1, 100, ErrorMessage = "Please input a valid number")]
        public int TotalAdults { get; set; }
        
        [Column(TypeName = "Int")]
        [Range(0, 100, ErrorMessage = "Please input a valid number")]
        public int TotalChildrens { get; set; }

        [Column(TypeName = "Int")]
        [Range(0, 100, ErrorMessage = "Please input a valid number")]
        public int TotalInfants { get; set; }

        [Column(TypeName = "Decimal(10, 2)")]
        [Required]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        public DateTime Date { get; set; }
        [NotMapped]
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Payment { get; set; } = "Pending";

        public uint UserId { get; set; }
    }

    public class OrderPeople
    {
        [Key]
        public uint OpId { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        public string Proof { get; set; }
        [Required]
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
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Time is required")]
        public DateTime Time { get; set; }
        [Required(ErrorMessage = "Reference id is required")]
        public uint RefId { get; set; }
        [Column(TypeName = "Decimal(10, 2)")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        [ForeignKey("Order")]
        public uint OrderId { get; set; }
    }

}
