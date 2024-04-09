using Microsoft.EntityFrameworkCore;
using Brickwell.Data;

namespace Brickwell.Models
{
    public class EFBrickwellRepository : IBrickwellRepository
    {
        private ApplicationDbContext _context;
        public EFBrickwellRepository(ApplicationDbContext temp)
        {
            _context = temp;
        }

        public IQueryable<Customer> Customers  => _context.Customers;

        public IQueryable<Order> Orders => _context.Orders;

        public IQueryable<Product> Products => _context.Products;

        public IQueryable<LineItem> LineItems => _context.LineItems;

        //public DbSet<Recommendation> Recommendations => _context.Recommendations;

        public void AddProduct(Product Product)
        {
            _context.Add(Product);
            _context.SaveChanges();
        }

        public void RemoveProduct(Product Product)
        {
            _context.Remove(Product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product Product)
        {
            _context.Update(Product);
            _context.SaveChanges();
        }

    }
}
