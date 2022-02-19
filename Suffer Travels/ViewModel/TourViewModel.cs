using Suffer_Travels.Models;
using System.Collections.Generic;

namespace Suffer_Travels.ViewModel
{
    public class TourViewModel
    {
        public Tour tour { get; set; }
        public TourType tourType { get; set; }
        public TourDates tourDate { get; set; }
        public TourPhotos tourPhoto { get; set; }
        public Photo photo { get; set; }

        public IEnumerable<Tour> tourDetails { get; set; }

        public IEnumerable<TourType> tourTypes { get; set; }

        public IEnumerable<TourDates> tourDates { get; set; }

        public IEnumerable<TourPhotos> tourPhotos { get; set; }

        public IEnumerable<Photo> photos { get; set; }

    }

}
