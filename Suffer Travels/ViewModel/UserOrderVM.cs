using Suffer_Travels.Models;

namespace Suffer_Travels.ViewModel
{
    public class UserOrderVM
    {
        public Order order { get; set; }
        public Tour tour { get; set; }
        public OrderTour orderTour { get; set; }
        public IEnumerable<Order> orders { get; set; }
        public IEnumerable<Tour> tours { get; set; }
        public IEnumerable<OrderTour> orderTours { get; set; }
        public IEnumerable<TourPhotos> tourPhotos { get; set; }
        public IEnumerable<Photo> photos { get; set; }
        public IEnumerable<TourType> tourTypes { get; set; }
    }
}
