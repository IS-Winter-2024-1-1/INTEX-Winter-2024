using Microsoft.EntityFrameworkCore;

namespace Brickwell.Models
{
    public interface IBrickwellRepository
    {
        IQueryable<Customer> Customers { get; }

        IQueryable<Order> Orders { get; }

        IQueryable<Product> Products { get; }

        IQueryable<LineItem> LineItems { get; }

        //IQueryable<Recommendation> Recommendations { get; }

        public void AddProduct(Product Product);
        public void RemoveProduct(Product Product);
        public void UpdateProduct(Product Product);


    }
}
