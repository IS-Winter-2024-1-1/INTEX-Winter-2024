using Brickwell.Data;
using Brickwell.Infrastructure;
using Brickwell.Models;
using Brickwell.Models.ViewModels;
using Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Console;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Brickwell.Controllers
{

    public class HomeController : Controller
    {
        private IBrickwellRepository _repo;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        // Create Cart for the session here
        public Cart cart { get; set; }

        public HomeController(IBrickwellRepository temp,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<ApplicationRole> roleManager,
                                ApplicationDbContext applicationDbContext,
                                Cart cartService)
        {
            _repo = temp;
            _dbcontext = applicationDbContext;
            _usermanager = userManager;
            _roleManager = roleManager;
            cart = cartService;
            _roleManager = roleManager;
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
        public IActionResult Products(int pageNum, string? productType, string? productColor, int pageSize = 5)
        {
            {
                

                var skipAmount = pageSize * (pageNum - 1);
                if (skipAmount < 0)
                {
                    skipAmount = 0;
                }

                var stuff = new ProductsViewModel()
                {
                    Products = _repo.Products
                        .Where(x => (x.category == productType || productType == null) && (x.primary_color == productColor || productColor == null))
                        .OrderBy(x => x.name)
                        .Skip(skipAmount)
                        .Take(pageSize),

                    PaginationInfo = new PaginationInfo
                    {
                        CurrentPage = pageNum,
                        ItemsPerPage = pageSize,
                        TotalItems = (productType == null && productColor == null) ? 
                            _repo.Products.Count() :
                            (productType != null && productColor != null) ?
                                _repo.Products.Count(x => x.category == productType && x.primary_color == productColor) :
                                (productType != null) ? _repo.Products.Count(x => x.category == productType) : _repo.Products.Count(x => x.primary_color == productColor)

                    },
                    
                    CurrentProductType = productType
                };

                ViewBag.CurrentProductType = productType;
                ViewBag.CurrentProductColor = productColor;
                ViewBag.pageSize = pageSize;

                return View(stuff);
            }
        }

        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            var productDetailed = new ProductDetailsViewModel()
            {
                Product = _repo.Products.FirstOrDefault(p => p.product_ID == id)
            };
            return View(productDetailed);
        }
    

        [HttpPost]
        public IActionResult AddToCart(int product_ID, string returnUrl, int quantity)
        {
            Console.WriteLine("id: " + product_ID + " qty: " + quantity);
            Product product = _repo.Products.FirstOrDefault(p => p.product_ID == product_ID);

            if (product != null)
            {
                cart.AddItem(product, quantity);
            }

            // add the product to the cart session and then send the cart page
            return RedirectToAction("Cart", new {returnUrl = returnUrl});
        }

        [HttpGet]
        public IActionResult Cart(string returnUrl)
        {
            
            cart.ReturnUrl = returnUrl ?? "/";
            // send the cart page
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int product_ID, string returnUrl)
        {
            cart.RemoveLine(cart.Lines.First(x => x.Product.product_ID == product_ID).Product);

            // remove the product from the cart session and then send the cart page
            return RedirectToAction("Cart", new {returnUrl = returnUrl});
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Checkout(string returnUrl)
        {
            //get current user  
            var currentuser = await _usermanager.FindByNameAsync(User.Identity.Name);

            IQueryable<Customer> customerList = _repo.Customers.Where(x => x.username == currentuser.UserName);
            if (customerList.Count() < 1)
            {
                return RedirectToAction("CustomerInfo");
            }
            else if (customerList.Count() > 1)
            {
                Console.WriteLine("ERROR: Multiple customers with username \"" + currentuser.UserName + "\". Manual resolution nessecary.");
                return StatusCode(500);
            }
            else
            {

                //Get countries from the database
                ViewBag.countries = _repo.Orders.Select(x => x.shipping_address).Distinct().OrderBy(x => x).ToList();
                ViewBag.cardTypes = _repo.Orders.Select(x => x.type_of_card).Distinct().OrderBy(x => x).ToList();
                // check the user credentials in the database
                //Log in or reject and redirect to index page
                // send the checkout page
                cart.ReturnUrl = returnUrl ?? "/";
                // send the cart page
                return View(cart);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(IFormCollection form)
        {
            //get current user  
            var currentuser = await _usermanager.FindByNameAsync(User.Identity.Name);

            IQueryable<Customer> customerList = _repo.Customers.Where(x => x.username == currentuser.UserName);
            if (customerList.Count() < 1)
            {
                return RedirectToAction("SSAddCustomer");
            }
            else if (customerList.Count() > 1)
            {
                Console.WriteLine("ERROR: Multiple customers with username \"" + currentuser.UserName + "\". Manual resolution nessecary.");
                return StatusCode(500);
            }
            else
            {
                Customer customer = customerList.First();

                Dictionary<string, dynamic> orderDictionary = new Dictionary<string, dynamic>
                {
                    { "transaction_ID", 0 }, // This gets dropped.
                    { "customer_ID", customer.customer_ID }, // This also gets dropped.
                    { "date", DateTime.Now.ToString("MM/dd/yyyy") },
                    { "day_of_week", DateTime.Now.ToString("dddd").Substring(0,3) },
                    { "time", DateTime.Now.ToString("HH") },
                    { "entry_mode", "CVC" },
                    { "amount", cart.ComputeTotalSum() },
                    { "type_of_transaction", "Online" },
                    { "country_of_transaction", form["billing_country"] },
                    { "shipping_address", form["country"] }, 
                    { "bank", "drop" }, // This also gets dropped.
                    { "type_of_card", "drop" } // This also gets dropped.
                };
                Dictionary<string, dynamic> customerDictionary = new Dictionary<string, dynamic>
                {
                    { "customer_ID", customer.customer_ID }, // This still gets dropped.
                    { "first_name", customer.first_name },
                    { "last_name", customer.last_name },
                    { "birth_date", customer.birth_date },
                    { "country_of_residence", customer.country_of_residence },
                    { "gender", customer.gender },
                    { "age", customer.age }
                };
                
                string orderJSON = JsonConvert.SerializeObject(orderDictionary);
                string customerJSON = JsonConvert.SerializeObject(customerDictionary);

                

                // make the post request to the fraud endpoint

                HttpClient client = new HttpClient();

                Dictionary<string, Dictionary<string, dynamic>> body = new Dictionary<string, Dictionary<string, dynamic>>
                {
                    { "order_data", new Dictionary<string, dynamic>
                        {
                            { "transaction_ID", 0 }, // This gets dropped.
                            { "customer_ID", customer.customer_ID }, // This also gets dropped.
                            { "date", DateTime.Now.ToString("MM/dd/yyyy") },
                            { "day_of_week", DateTime.Now.ToString("dddd").Substring(0,3) },
                            { "time", DateTime.Now.Hour },
                            { "entry_mode", "CVC" },
                            { "amount", cart.ComputeTotalSum() },
                            { "type_of_transaction", "Online" },
                            { "country_of_transaction", form["billing_country"] },
                            { "shipping_address", form["country"] },
                            { "bank", "drop" }, // This also gets dropped.
                            { "type_of_card", "drop" } // This also gets dropped.
                        }
                    },
                    { "customer_data", new Dictionary<string, dynamic>
                        {
                            { "customer_ID", customer.customer_ID }, // This still gets dropped.
                            { "first_name", customer.first_name },
                            { "last_name", customer.last_name },
                            { "birth_date", customer.birth_date },
                            { "country_of_residence", customer.country_of_residence },
                            { "gender", customer.gender },
                            { "age", customer.age }
                        }
                    }
                };

                string JSONbody = JsonConvert.SerializeObject(body);
                var httpContent = new StringContent(JSONbody, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("x-functions-key", System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "/APIkey.txt"));
                
                HttpResponseMessage response = await client.PostAsync("https://isitfraud.azurewebsites.net/api/isitfraud", httpContent);
                string responseString = await response.Content.ReadAsStringAsync();

                dynamic fraudObj = JObject.Parse(responseString);


                Order order = new Order() 
                { 
                    customer_ID = orderDictionary["customer_ID"], 
                    Customer = customer,
                    date = DateOnly.FromDateTime(DateTime.Now),
                    day_of_week = orderDictionary["day_of_week"],
                    time = DateTime.Now.Hour,
                    amount = orderDictionary["amount"],
                    entry_mode = orderDictionary["entry_mode"],
                    type_of_transaction = orderDictionary["type_of_transaction"],
                    country_of_transaction = orderDictionary["country_of_transaction"],
                    shipping_address = orderDictionary["shipping_address"],
                    fraud = fraudObj.fraud
                };

                _repo.AddOrder(order);
                
                if (fraudObj.fraud == 0)
                {
                    cart.Clear();
                    return View("OrderConfirmation");
                }
                else 
                {
                    cart.Clear();
                    return RedirectToAction("OrderReview");
                }

            }
            


            
        }







        
        // Add customer (self-service).
        [HttpGet]
        [Authorize]
        public IActionResult SSAddCustomer()
        {
            

            // send the 
            return View("SSEditCustomer");
        }

        [HttpPost]
        public IActionResult SSAddCustomer(Customer newCustomer)
        {
            newCustomer.username = User.Identity.Name;
            _repo.AddCustomer(newCustomer);
            // Adds the 
            return RedirectToAction("Index");
        }

        // Update Customer (self-service).
        [HttpGet]
        public IActionResult SSEditCustomer(int id)
        {
            Customer customer = _repo.Customers.FirstOrDefault(c => c.customer_ID == id);

            

            
            // send the 
            return View(customer);
        }

        // Update Customer for self-service.
        [HttpPost]
        public IActionResult SSEditCustomer(Customer customer)
        {
            _repo.UpdateCustomer(customer);
            // 
            return View("Index");
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
        public IActionResult ListOrders(int pageNum)
        {
            // grabs all the orders that are considered fraud and puts them in order of most recent to oldest.
            int pageSize = 1000;

            var skipAmount = pageSize * (pageNum - 1);
            if (skipAmount < 0)
            {
                skipAmount = 0;
            }

            var things = _repo.Orders;
            Console.WriteLine(things.First().date.GetType());


            var oneMonthAgo = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));

            var stuff = new OrderListViewModel
            {
                Orders = _repo.Orders
                    .Where(order => order.fraud == 1 && order.date >= oneMonthAgo)  // Filter by fraud and date from the last Month
                    .OrderByDescending(order => order.date),

                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = _repo.Orders
                    .Where(order => order.fraud == 1 && order.date >= oneMonthAgo)  // Filter by fraud and date
                    .OrderByDescending(order => order.date).Count()
                }
            };


            // send the list orders page which is only accessible by the admin to see all the orders
            return View(stuff);
        }

        // List all Products for Admin
        [HttpGet]
        public IActionResult ListProducts(int pageNum)
        {
            int pageSize = 9;

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
        [Authorize]
        public async Task<IActionResult> ListCustomers(int pageNum)
        {
            int pageSize = 20;

            var skipAmount = pageSize * (pageNum - 1);
            if (skipAmount < 0)
            {
                skipAmount = 0;
            }

            
           
            //query the userrole table  
            //required using Microsoft.EntityFrameworkCore;  
            IQueryable<ApplicationUserRole> userRoles = _dbcontext.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role);



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
                },


                UserRoles = userRoles
            };
           
            // send the list customers page which is only accessible by the admin to see all the customers
            return View(stuff);
        }

        [HttpGet]
        public IActionResult EditCustomer(int id)
        {
            Customer customer = _repo.Customers.FirstOrDefault(c => c.customer_ID == id);

            // Try to get the name of the role of the Customer.
            var userRole = _dbcontext.ApplicationUserRoles.Include(c => c.User).Include(c => c.Role).Where(x => x.User.UserName == customer.username).FirstOrDefault();
            Console.WriteLine(userRole);
            if (userRole != null)
            {
                ViewBag.CurrentlyEditingRole = userRole.Role.Name;
            }
            else
            {
                ViewBag.CurrentlyEditingRole = "Account Not Found";
            }



            // send the edit customer page which is only accessible by the admin
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer customer, IFormCollection form)
        {
            _repo.UpdateCustomer(customer);

            string role = form["role"];

            if (role == "Customer" || role == "Admin")
            {
                ApplicationUser user = await _usermanager.FindByNameAsync(customer.username);
                IList<string> roles = await _usermanager.GetRolesAsync(user);

                foreach (string currentRole in roles)
                {
                    await _usermanager.RemoveFromRoleAsync(user, currentRole);
                }
                await _usermanager.AddToRoleAsync(user, role);
               
            }

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

        [HttpGet]
        public IActionResult AddCustomer()
        {
            // send the add customer page which is only accessible by the admin
            return View("EditCustomer");
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
