using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public ActionResult PlaceOrder(int cartID)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                Cart cart = new Cart();
                decimal cartTotal = cart.GetCartTotal(cartID);

                // Check if the cart is empty
                if (cartTotal == 0)
                {
                    // The cart is empty, return an error message
                    TempData["ErrorMessage"] = "You cannot place an order with an empty cart.";
                    return RedirectToAction("ViewCart", "User");
                }

                Order order = new Order();
                order.CartID = cartID;
                order.OrderDate = DateTime.Now;
                var result = order.Insert_Order(order);
                Cart newCart = new Cart { CartUserID = userID.Value, CartTotal = 0, CartStatus = 0 };
                newCart.New_Cart(newCart);
            }

            return RedirectToAction("OrderHistory", "User");
        }

    }
}
