using Suffer_Travels.Models;

namespace Suffer_Travels.ViewModel
{
    public class OrderViewModel
    {
        // For tour details
        public TourViewModel TourView { get; set; }
        public HotelViewModel hotelVM { get; set; }

        // For order details
        public Order order { get; set; }
        public OrderPeople orderPeople { get; set; }
        public OrderPeople[] orderPeoples { get; set; }
        public OrderTour orderTour { get; set; }
        public OrderHotel orderHotel { get; set; }
        public OrderVehicle orderVehicle { get; set; }
        public Payment payment { get; set; }

        public IEnumerable<Order>  orders { get; set; }
        public IEnumerable<OrderPeople> OrderPeoples { get; set; }
        public IEnumerable<OrderTour> orderTours { get; set; }
        public IEnumerable<OrderHotel> orderHotels { get; set; }
        public IEnumerable<OrderVehicle> orderVehicles { get; set; }
        public IEnumerable<Payment> payments { get; set; }

        // Lists
        public List<OrderHotel> ohList { get; set; }

    }
}
