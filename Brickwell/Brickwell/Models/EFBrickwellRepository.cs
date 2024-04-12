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

        public IQueryable<ProductRecommendation> ProductRecommendations => _context.ProductRecommendations;

        public IQueryable<CustomerRecommendation> CustomerRecommendations => _context.CustomerRecommendations;

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

        public void AddCustomer(Customer Customer)
        {
            _context.Add(Customer);
            _context.SaveChanges();
        }

        public void RemoveCustomer(Customer Customer)
        {
            _context.Remove(Customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer Customer)
        {
            _context.Update(Customer);
            _context.SaveChanges();
        }

        public void AddOrder(Order Order)
        {
            _context.Add(Order);
            _context.SaveChanges();
        }
    }
}
