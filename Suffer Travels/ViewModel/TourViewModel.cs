using Microsoft.Extensions.Primitives;
using Suffer_Travels.Models;
using System.Collections.Generic;

namespace Suffer_Travels.ViewModel
{
    public class TourViewModel
    {
        public Tour tourDetail { get; set; }
        public TourType tourTypeDetails { get; set; }
        public TourDates tourDate { get; set; }
        public TourPhotos tourPhoto { get; set; }
        public Photo photo { get; set; }

        public TourItinerary tourItinerary { get; set; }

        public IEnumerable<Tour> tourDetails { get; set; }

        public IEnumerable<TourType> tourTypes { get; set; }

        //For recurrence
        public string recurrence { get; set; } 
        public UInt32 repeatsEvery { get; set; }

        public Int32 NoOfDays { get; set; }

        public UInt32? TourId { get; set; }

        //Enumerables
        public IEnumerable<TourDates> tourDates { get; set; }

        public IEnumerable<TourPhotos> tourPhotos { get; set; }

        public IEnumerable<Photo> photos { get; set; }

        public IEnumerable<TourItinerary> tourItineraries { get; set; }

    }

}
