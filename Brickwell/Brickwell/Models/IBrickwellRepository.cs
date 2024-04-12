using Microsoft.EntityFrameworkCore;

namespace Brickwell.Models
{
    public interface IBrickwellRepository
    {
        IQueryable<Customer> Customers { get; }

        IQueryable<Order> Orders { get; }

        IQueryable<Product> Products { get; }

        IQueryable<LineItem> LineItems { get; }

        IQueryable<ProductRecommendation> ProductRecommendations { get; }
        IQueryable<CustomerRecommendation> CustomerRecommendations { get; }

        IQueryable<Favorite> Favorites { get; }

        public void AddProduct(Product Product);
        public void RemoveProduct(Product Product);
        public void UpdateProduct(Product Product);

        public void AddCustomer(Customer Customer);
        public void RemoveCustomer(Customer Customer);
        public void UpdateCustomer(Customer Customer);

        
        public void AddOrder(Order Order);


    }
}
