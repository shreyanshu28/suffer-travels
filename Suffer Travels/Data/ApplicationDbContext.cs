using Microsoft.EntityFrameworkCore;
using Suffer_Travels.Models;

namespace Suffer_Travels.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> tblUser { get; set; }
        public DbSet<Role> tblRole { get; set; }
        public Register Register { get; set; }

        public DbSet<City> tblCity { get; set; }
        public DbSet<Country> tblCountry { get; set; }
        public DbSet<State> tblState { get; set; }
        public DbSet<Area> tblArea { get; set; }

        public DbSet<Photo> tblPhotos { get; set; }

        public DbSet<Tour> tblTour { get; set; }
        public DbSet<TourCities> tblTourCities { get; set; }
        public DbSet<TourDates> tblTourDates { get; set; }
        public DbSet<TourItinerary> tblTourItinerary { get; set; }
        public DbSet<TourPhotos> tblTourPhotos { get; set; }
        public DbSet<TourType> tblTourType { get; set; }
 

    }

}
