namespace Brickwell.Models.ViewModels
{
    public class CustomerListViewModel
    {
        public IQueryable<Customer> Customers { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();

        public ApplicationRole Role { get; set; }
    }
}
