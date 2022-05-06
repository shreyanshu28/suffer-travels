using Suffer_Travels.Models;
using Suffer_Travels.Data;

namespace Suffer_Travels.ViewModel
{
    public class TourViewModel
    {
        public Tour tourDetail { get; set; }
        public TourType tourTypeDetails { get; set; }
        public TourDates tourDate { get; set; }
        public TourPhotos tourPhoto { get; set; }
        public Photo photo { get; set; }
        public City city { get; set; }
        public State state { get; set; }
        public Country country { get; set; }
        public Landmark landmark { get; set; }
        public TourItinerary tourItinerary { get; set; }
        public TourCities tourCity { get; set; }
        public FavouriteTours favouriteTour { get; set; } 

        //Enumerables
        public IEnumerable<Tour> tourDetails { get; set; }
        public IEnumerable<TourType> tourTypes { get; set; }
        public IEnumerable<TourDates> tourDates { get; set; }
        public IEnumerable<TourPhotos> tourPhotos { get; set; }
        public IEnumerable<Photo> photos { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<State> states { get; set; }
        public IEnumerable<Country> countries { get; set; }
        public IEnumerable<Landmark> landmarks { get; set; }
        public IEnumerable<TourItinerary> tourItineraries { get; set; }
        public TourItinerary[] tiList { get; set; }
        public IEnumerable<TourCities> tourCities { get; set; }
        public IEnumerable<FavouriteTours> favouriteTours { get; set; }

        //Variables
        public string recurrence { get; set; }
        public UInt32 repeatsEvery { get; set; }
        public Int32 NoOfDays { get; set; }
        public UInt32? TourId { get; set; }
        public string[] CityName { get; set; }
    }
}
