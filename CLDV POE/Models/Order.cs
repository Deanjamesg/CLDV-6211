using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class Order
    {
        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);

        public int CartID { get; set; }
        public DateTime OrderDate { get; set; }

        public int Insert_Order(Order order)
        {
            try
            {
                string sql = "INSERT INTO [Order] (Cart_ID, OrderDate) VALUES (@CartID, @OrderDate)" +
                     "UPDATE [Cart] SET Cart_Status = 1 WHERE Cart_ID = @CartID AND Cart_Status = 0";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@CartID", order.CartID);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
