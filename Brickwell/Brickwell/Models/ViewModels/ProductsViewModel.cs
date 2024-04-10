namespace Brickwell.Models.ViewModels;

public class ProductsViewModel
{
    public IQueryable<Product> Products { get; set; }
    
    public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
}