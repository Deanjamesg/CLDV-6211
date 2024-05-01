using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class User : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
