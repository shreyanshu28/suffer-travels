using Suffer_Travels.Models;
using System.Collections.Generic;

namespace Suffer_Travels.ViewModel
{
    public class TourViewModel
    {
        public Tour tours { get; set; }

        public IEnumerable<Tour> tourDetails { get; set; }

        public IEnumerable<TourType> tourTypes { get; set; }

        public IEnumerable<TourPhotos> tourPhotos { get; set; }

        public IEnumerable<Photo> photos { get; set; }


        //public TourType tt { get; set; }

        /*public string getTourTypeName (UInt32? ttid, IEnumerable<TourType> tt)
        {   
            //Where(tt => tt.TtId == item.TourTypeId).FirstOrDefault().TtName
            return tt.Where(t => t.TtId == ttid).FirstOrDefault().TtName;
        }*/
    }

}
