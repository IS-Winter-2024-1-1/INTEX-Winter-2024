using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Brickwell.Models;

namespace Brickwell.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        



        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<LineItem> LineItems { get; set; }

        public DbSet<Recommendation> Recommendations { get; set; }
    }
}
