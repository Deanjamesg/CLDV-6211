using Microsoft.AspNetCore.Mvc;
using CLDV_POE.Models;

namespace CLDV_POE.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public ActionResult AddProduct(Product newProduct)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                var result = newProduct.Insert_Product(newProduct, userID.Value);
            }

            return RedirectToAction("Shop", "Product");
        }

        [HttpPost]
        public ActionResult RemoveProduct(int productID)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                Product product = new Product();
                userID = userID.Value;
                product.Delete_Product(productID, userID);
            }
            return RedirectToAction("YourProducts", "Product");
        }

        public IActionResult SellProduct()
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                User user = new User().GetUser(userID.Value);
                return View(user);
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
            return View();
        }

        public IActionResult Shop()
        {
            // Retrieve all products from the database
            List<Product> products = Product.GetAllProducts();
            // Pass products to the view
            ViewData["Products"] = products;
            return View();
        }

        public IActionResult YourProducts()
        {
            // Get the user data and return the view
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                User user = new User().GetUser(userID.Value);
                List<Product> usersProducts = Product.GetAllUserProducts(userID);
                ViewData["UsersProducts"] = usersProducts;

                return View(user);
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }

        [HttpPost]
        public IActionResult UpdateAvailability(int productID, int availability)
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                Product product = new Product();
                product.Insert_UpdatedAvailability(productID, userID.Value, availability);
            }
            return RedirectToAction("YourProducts", "Product");
        }

    }


}
