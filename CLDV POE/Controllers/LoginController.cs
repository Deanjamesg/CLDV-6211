using CLDV_POE.Models;
using Microsoft.AspNetCore.Mvc;

namespace CLDV_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginModel login;
        public LoginController()
        {
            login = new LoginModel();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var loginModel = new LoginModel();
            int userID = loginModel.SelectUser(username, password);
            if (userID != -1)
            {
                HttpContext.Session.SetInt32("UserID", userID);

                return RedirectToAction("Account", "User");
            }
            else
            {
                // User not found, handle accordingly (e.g., show error message)
                TempData["LoginFailed"] = "Incorrect login details have been entered. If you do not have an account, please register.";
                return View("LoginPage");
            }
        }
        public IActionResult LoginPage()
        {
            return View();
        }
    }
}
