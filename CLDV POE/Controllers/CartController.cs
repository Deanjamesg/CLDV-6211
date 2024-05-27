using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Cart cart = new Cart();

        public CartController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public ActionResult CreateCart()
        {
            int? userID = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");
            if (userID.HasValue)
            {
                
                return View(cart);
            }
            else
            {
                return RedirectToAction("LoginPage", "Login");
            }
        }

    }
}
