using Brickwell.Data;
using Brickwell.Models;
using Brickwell.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace Brickwell.Controllers
{
    public class HomeController : Controller
    {
        private IBrickwellRepository _repo;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<ApplicationRole> _roleManger;

        public HomeController(IBrickwellRepository temp,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<ApplicationRole> roleManager,
                                ApplicationDbContext applicationDbContext)
        {
            _repo = temp;
            _dbcontext = applicationDbContext;
            _usermanager = userManager;
            _roleManger = roleManager;
        }



        public IActionResult Index()
        {
            var userRecProducts = _repo.Products;
            return View(userRecProducts);
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Test()
        {
            if (User.Identity.IsAuthenticated)
            {
                //get current user  
                var currentuser = await _usermanager.FindByNameAsync(User.Identity.Name);
                //query the userrole table  
                //required using Microsoft.EntityFrameworkCore;  
                var userrole = _dbcontext.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role).Where(c => c.UserId == currentuser.Id).FirstOrDefault();
                var user = userrole.User;
                var role = userrole.Role;

                ViewBag.user = new { myUserRole = userrole, 
                                     myUser = user,
                                     myRole = role};

            }

            return View();
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login() 
        {
            // send the login page
            return View();
        }

        [HttpPost]
        public IActionResult Login(string x)
        {
            // check the user credentials in the database
            //Log in or reject and redirect to index page
            return View();
        }

        [HttpGet]
        public IActionResult Products()
        {
            var productData = _repo.Products;
            // send the product listings page
            return View(productData);
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            var productDetailed = _repo.Products
                .Where(x => x.product_ID == id);
            return View(productDetailed);
        }

        [HttpPost]
        public IActionResult AddToCart()
        {
            // add the product to the cart session and then send the cart page
            return View();
        }

        [HttpGet]
        public IActionResult Cart()
        {
            // send the cart page
            return View();
        }

        [HttpGet]
        public IActionResult ProceedToCheckout()
        {
            // send the confirm proceed to checkout page
            return View();
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            // send the checkout page
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(string x)
        {
            // check the user credentials in the database
            //Log in or reject and redirect to index page
            return View();
        }

        [HttpGet]
        public IActionResult OrderConfirmation()
        {
            // send the order confirmation page if not fraud otherwise redirect to the OrderReview page
            return View();
        }

        [HttpGet]
        public IActionResult OrderReview()
        {
            // send the order review notification page to the customer
            return View();
        }

        //From here on it should be mostly Admin actions

        [HttpGet]
        public IActionResult AdminPage()
        {
            // send the admin page
            return View();
        }

        // List Fraud Orders for Admin
        [HttpGet]
        public IActionResult ListOrders()
        {
            // grabs all the orders that are considered fraud and puts them in order of most recent to oldest.
            var orderList = _repo.Orders
                .Where(order => order.fraud == 1)
                .OrderByDescending(order => order.date);

            // send the list orders page which is only accessible by the admin to see all the orders
            return View(orderList);
        }

        // List all Products for Admin
        [HttpGet]
        public IActionResult ListProducts(int pageNum)
        {
            int pageSize = 10;

            var skipAmount = pageSize * (pageNum - 1);
            if (skipAmount < 0)
            {
                skipAmount = 0;
            }

            var stuff = new ProductAdminListViewModel
            {
                Products = _repo.Products
                .OrderBy(x => x.name)
                .Skip(skipAmount)
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Products.Count()
                }
            };
            // send the list products page which is only accessible by the admin to see all the products
            return View(stuff);
        }

        // Edit Product for Admin
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            Product product = _repo.Products.FirstOrDefault(p => p.product_ID == id);

            var uniqueCategories = _repo.Products
                .Select(x => x.category)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.categories = uniqueCategories;
            // send the edit product page which is only accessible by the admin
            return View(product);
        }

        // Update Product for Admin
        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            _repo.UpdateProduct(product);
            // Edits the product from the Admins changes and then redirects back to the AdminPage
            // Redirects to the Products List page
            return View("ChangesConfirmation");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var uniqueCategories = _repo.Products
                .Select(x => x.category)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            ViewBag.categories = uniqueCategories;

            // send the add product page which is only accessible by the admin
            return View("EditProduct");
        }

        [HttpPost]
        public IActionResult AddProduct(Product newProduct)
        {
            _repo.AddProduct(newProduct);
            // Adds the product from the Admins changes and then redirects back to the AdminPage
            return View("ChangesConfirmation");
        }

        [HttpPost]
        public IActionResult DeleteProduct(Product product)
        {
            _repo.RemoveProduct(product);
            // Deletes the Product from the Admins changes and then redirects back to the AdminPage
            return View("ChangesConfirmation");
        }

        [HttpGet]
        public IActionResult ListCustomers(int pageNum)
        {
            int pageSize = 30;

            var skipAmount = pageSize * (pageNum - 1);
            if (skipAmount < 0)
            {
                skipAmount = 0;
            }

            var stuff = new CustomerListViewModel
            {
                Customers = _repo.Customers
                .OrderBy(x => x.last_name)
                .Skip(skipAmount)
                .Take(pageSize),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Customers.Count()
                }
            };
           
            // send the list customers page which is only accessible by the admin to see all the customers
            return View(stuff);
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            Customer customer = _repo.Customers.FirstOrDefault(c => c.customer_ID == id);
            // send the edit customer page which is only accessible by the admin
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customer customer)
        {
            _repo.UpdateCustomer(customer);
            // Edits the customer from the Admins changes and then redirects back to the AdminPage
            return View("ChangesConfirmation");
        }

        [HttpPost]
        public IActionResult DeleteCustomer(Customer customer)
        {
            _repo.RemoveCustomer(customer);
            // Deletes the Customer from the Admins changes and then redirects back to the AdminPage
            return View("ChangesConfirmation");
        }

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            _repo.AddCustomer(customer);
            // Adds the Customer from the Admins changes and then redirects back to the AdminPage
            return View("ChangesConfirmation");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
