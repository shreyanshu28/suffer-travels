using Suffer_Travels.Models;
using System.Collections.Generic;

namespace Suffer_Travels.ViewModel
{
    public class TourViewModel
    {
        public Tour tours { get; set; }
        public IEnumerable<Tour> tourDetails { get; set; }

        public string tourType { get; set; }


        //public TourType tt { get; set; }

        /*public string getTourTypeName (UInt32? ttid, IEnumerable<TourType> tt)
        {

            
            //Where(tt => tt.TtId == item.TourTypeId).FirstOrDefault().TtName
            return tt.Where(t => t.TtId == ttid).FirstOrDefault().TtName;
        }*/
    }

}
