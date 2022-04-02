using Suffer_Travels.Models;

namespace Suffer_Travels.ViewModel
{
    public class CityVM
    {
        public City city { get; set; }
        public State state { get; set; }
        public Photo photo { get; set; }
        public IEnumerable<City> cities { get; set; }
        public IEnumerable<State> states { get; set; }
        public IEnumerable<Photo> photos { get; set; }

    }
}
