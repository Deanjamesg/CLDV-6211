using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Cart cart = new Cart();

        public CartItemsController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public ActionResult AddToCart(int productID, int quantity)
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userID.HasValue)
            {
                Product product = new Product();
                CartItems cartItems = new CartItems();
                cartItems.CartID = cart.GetCartID(userID.Value, 0);
                cartItems.ProductID = productID;
                cartItems.Quantity = quantity;
                cartItems.Insert_AddToCart(cartItems);

                cart.UpdateCartTotal(cartItems.CartID);

                return RedirectToAction("Shop", "Product");
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }
         
        [HttpPost]
        public ActionResult RemoveFromCart(int productID)
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            
            if (userID.HasValue)
            {
                Product product = new Product();
                CartItems cartItems = new CartItems();
                cartItems.CartID = cart.GetCartID(userID.Value, 0);
                cartItems.ProductID = productID;
                cartItems.Remove_CartItem(cartItems);
                cart.UpdateCartTotal(cartItems.CartID);
                return RedirectToAction("ViewCart", "User");
            }
            else
            {
                // Handle the case when  UserID is not in the session
                return RedirectToAction("LoginPage", "Login");
            }
        }
    }
}
