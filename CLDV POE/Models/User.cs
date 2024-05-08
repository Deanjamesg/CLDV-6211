using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class User : Controller
    {
        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=nope;Password=nope;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);
        public IActionResult Index()
        {
            return View();
        }
    }
}
