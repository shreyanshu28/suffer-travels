using Suffer_Travels.ViewModel;

namespace Suffer_Travels.Models
{
    public class OrderModel
    {
        public TourViewModel TourView { get; set; }

        public Order order { get; set; }
    }
}
