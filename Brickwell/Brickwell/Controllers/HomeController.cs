using Brickwell.Models;
using Brickwell.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Brickwell.Controllers
{
    public class HomeController : Controller
    {
        private IBrickwellRepository _repo;

        public HomeController(IBrickwellRepository temp)
        {
            _repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Test()
        {
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
        public IActionResult ProductPage()
        {
            // send the product listings page
            return View();
        }

        [HttpGet]
        public IActionResult ProductDetails()
        {
            // send the product details page in accordance with the item id
            return View();
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
        public IActionResult ListProducts()
        {
            var productList = _repo.Products.OrderByDescending(product => product.name);
            // send the list products page which is only accessible by the admin to see all the products
            return View(productList);
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
            return RedirectToAction("ChangesConfirmation");
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
            int pageSize = 15;

            var stuff = new CustomerListViewModel
            {
                Customers = _repo.Customers
                .OrderBy(x => x.last_name)
                .Skip(pageSize * (pageNum - 1))
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
