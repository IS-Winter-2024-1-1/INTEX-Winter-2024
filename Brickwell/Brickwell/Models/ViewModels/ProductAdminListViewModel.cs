namespace Brickwell.Models.ViewModels
{
    public class ProductAdminListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
    }
}
