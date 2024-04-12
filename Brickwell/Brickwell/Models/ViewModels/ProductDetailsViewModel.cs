namespace Brickwell.Models.ViewModels;

public class ProductDetailsViewModel
{
    public Product Product { get; set; }

    public IQueryable<Product> Recommendations { get; set; } 
}