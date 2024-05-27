using Microsoft.AspNetCore.Mvc;
using CLDV_POE.Models;

namespace CLDV_POE.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public User user = new User();

        public UserController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public ActionResult Register(User newUser)
        {
            var result = user.Insert_User(newUser);
            if (result > 0)
            {
                int userID = new LoginModel().SelectUser(newUser.Username, newUser.Password);
                Cart newCart = new Cart { CartUserID = userID, CartTotal = 0, CartStatus = 0};
                newCart.New_Cart(newCart);
            }
            return RedirectToAction("LoginPage", "Login");
        }

        public IActionResult Account()
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

        public IActionResult ViewCart()
        {
            // Get the user data and return the view
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                // Get the cart ID
                Cart cart = new Cart();
                int cartID = cart.GetCartID(userID.Value, 0);

                // Get the cart items
                List<ViewCartDisplay> cartItemsList = CartItems.GetAllCartItems(cartID);

                // Pass the cart items to the view
                ViewData["CartTotal"] = cart.GetCartTotal(cartID);
                ViewData["CartItems"] = cartItemsList;

                return View();
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public IActionResult OrderHistory()
        {
            // Get the user data and return the view
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                User user = new User().GetUser(userID.Value);
                Cart cart = new Cart();
                List<OrderHistoryDisplay> cartList = cart.GetAllOrderedCarts(userID.Value);
                ViewData["OrderHistory"] = cartList;
                return View();
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public IActionResult RegisterPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ViewOrder(int cartID, int orderID)
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                // Get the cart ID
                Cart cart = new Cart();
                // Get the cart items
                List<ViewCartDisplay> cartItemsList = CartItems.GetAllCartItems(cartID);

                // Pass the cart items to the view
                ViewData["OrderTotal"] = cart.GetCartTotal(cartID);
                ViewData["OrderCart"] = cartItemsList;
                ViewData["OrderNo"] = orderID;
                return View();
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }

        public IActionResult SignOut()
        {
            _httpContextAccessor.HttpContext.Session.Remove("UserID");
            return RedirectToAction("LoginPage", "Login");
        }

        public IActionResult Clients()
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                // Get the cart items
                List<UserClients> clientList = user.GetUsersClientsOrders(userID.Value);

                // Pass the cart items to the view
                ViewData["ClientList"] = clientList;

                return View();
            }
            else
            {
                // Handle the case when the UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }
    }
}
