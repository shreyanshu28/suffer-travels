using Suffer_Travels.Models;
using System.Collections.Generic;

namespace Suffer_Travels.ViewModel
{
    public class OrderViewModel
    {
        //public Tour tourDetail { get; set; }
        //public TourType tourTypeDetails { get; set; }
        //public TourDates tourDate { get; set; }
        //public TourPhotos tourPhoto { get; set; }
        //public Photo photo { get; set; }

        //public IEnumerable<Tour> tourDetails { get; set; }

        //public IEnumerable<TourType> tourTypes { get; set; }

        ////For recurrence
        //public string recurrence { get; set; }
        //public UInt32 repeatsEvery { get; set; }
        //public IEnumerable<TourDates> tourDates { get; set; }

        //public IEnumerable<TourPhotos> tourPhotos { get; set; }

        //public IEnumerable<Photo> photos { get; set; }

        public TourViewModel TourView { get; set; }

        //public TourViewModel tourView = new TourViewModel();

        public Order order { get; set; }
    }
}
