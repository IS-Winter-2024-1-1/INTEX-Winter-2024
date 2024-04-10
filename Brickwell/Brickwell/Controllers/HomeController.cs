using Brickwell.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Brickwell.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            // send the order review page
            return View();
        }

        [HttpGet]
        public IActionResult AdminPage()
        {
            // send the admin page
            return View();
        }

        [HttpGet]
        public IActionResult ListOrders()
        {
            // send the list orders page which is only accessible by the admin to see all the orders
            return View();
        }

        [HttpGet]
        public IActionResult EditOrder()
        {
            // send the edit order page which is only accessible by the admin
            return View();
        }

        [HttpPost]
        public IActionResult EditOrder(string x)
        {
            // Edits the order from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpGet]
        public IActionResult EditProduct()
        {
            // send the edit product page which is only accessible by the admin
            return View();
        }
        [HttpPost]
        public IActionResult EditProduct(string x)
        {
            // Edits the product from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpGet]
        public IActionResult EditCustomer()
        {
            // send the edit customer page which is only accessible by the admin
            return View();
        }

        [HttpPost]
        public IActionResult EditCustomer(string x)
        {
            // Edits the customer from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct()
        {
            // Adds the product from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpPost]
        public IActionResult DeleteOrder()
        {
            // Deletes the Order from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpPost]
        public IActionResult DeleteProduct()
        {
            // Deletes the Product from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCustomer()
        {
            // Deletes the Customer from the Admins changes and then redirects back to the AdminPage
            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer()
        {
            // Adds the Customer from the Admins changes and then redirects back to the AdminPage
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
