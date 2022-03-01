using Microsoft.EntityFrameworkCore;
using Suffer_Travels.Models;

namespace Suffer_Travels.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //USER
        public DbSet<User> tblUser { get; set; }
        public DbSet<Role> tblRole { get; set; }
        public Register Register { get; set; }

        //CITY
        public DbSet<City> tblCity { get; set; }
        public DbSet<Country> tblCountry { get; set; }
        public DbSet<State> tblState { get; set; }
        public DbSet<Area> tblArea { get; set; }

        //PHOTOS
        public DbSet<Photo> tblPhotos { get; set; }

        //TOUR
        public DbSet<Tour> tblTour { get; set; }
        public DbSet<TourCities> tblTourCities { get; set; }
        public DbSet<TourDates> tblTourDates { get; set; }
        public DbSet<TourItinerary> tblTourItinerary { get; set; }
        public DbSet<TourPhotos> tblTourPhotos { get; set; }
        public DbSet<TourType> tblTourType { get; set; }
        public DbSet<MealCombo> tblMealCombo { get; set; }
 
        //HOTEL
        public DbSet<Hotel> tblHotelMaster { get; set; }
        public DbSet<HotelRooms> tblHotelRooms { get; set; }
        public DbSet<HotelPhotos> tblHotelPhotos { get; set; }

        //ORDER
        public DbSet<Order> tblOrderMaster { get; set; }
        public DbSet<OrderPeople> tblOrderPeople { get; set; }
        public DbSet<OrderTour> tblOrderTour { get; set; }
        public DbSet<OrderHotel> tblOrderHotel { get; set; }
        public DbSet<OrderVehicle> tblOrderVehicle { get; set; }
        public DbSet<Payment> tblPaymentMaster { get; set; }
    }

}
