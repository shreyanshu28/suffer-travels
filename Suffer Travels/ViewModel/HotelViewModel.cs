using Suffer_Travels.Models;

namespace Suffer_Travels.ViewModel
{
    public class HotelViewModel
    {
        public Hotel hotel { get; set; }
        public HotelRooms hotelRoom { get; set; }
        public HotelPhotos hotelPhoto { get; set; }
        public HotelAddress hotelAddress { get; set; }
        public City city { get; set; }
        public Photo photo { get; set; }

        //Enumerables
        public IEnumerable<Hotel> hotels { get; set; }
        public IEnumerable<HotelRooms> hotelRooms { get; set; }
        public IEnumerable<HotelPhotos> hotelPhotos { get; set; }
        public IEnumerable<HotelAddress> hotelAddresses { get; set; }
        public IEnumerable<City> cities {get; set; }
    }
}
