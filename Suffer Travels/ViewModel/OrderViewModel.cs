using Suffer_Travels.Models;

namespace Suffer_Travels.ViewModel
{
    public class OrderViewModel
    {
        // For tour details
        public TourViewModel TourView { get; set; }

        // For order details
        public Order order { get; set; }
        public OrderPeople orderPeople { get; set; }
        public OrderPeople[] orderPeoples { get; set; }
        public OrderTour orderTour { get; set; }
        public OrderHotel orderHotel { get; set; }
        public OrderVehicle orderVehicle { get; set; }
        public Payment payment { get; set; }
    }
}
