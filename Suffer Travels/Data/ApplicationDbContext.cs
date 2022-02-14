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

    }

}
