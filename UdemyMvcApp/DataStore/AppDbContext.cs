using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UdemyMvcApp.Models;

namespace UdemyMvcApp.DataStore
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
       
       
    }
}
