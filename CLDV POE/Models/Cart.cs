using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class Cart
    {
        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);
        public int CartID { get; set; }
        public int CartUserID { get; set; }
        public decimal CartTotal { get; set; }
        public int CartStatus { get; set; }
        public int New_Cart(Cart m)
        {
            try
            {
                string sql = "INSERT INTO [Cart] (User_ID, Cart_Total, Cart_Status) VALUES (@CartUserID, @CartTotal, @CartStatus)";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@CartUserID", m.CartUserID);
                    cmd.Parameters.AddWithValue("@CartTotal", m.CartTotal);
                    cmd.Parameters.AddWithValue("@CartStatus", m.CartStatus);
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

        public List<OrderHistoryDisplay> GetAllOrderedCarts(int userID)
        {
            List<OrderHistoryDisplay> allCarts = new List<OrderHistoryDisplay>();
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = @"SELECT c.Cart_ID, c.User_ID, c.Cart_Total, c.Cart_Status, o.Order_ID, o.OrderDate 
                           FROM [Cart] c
                           INNER JOIN [Order] o ON c.Cart_ID = o.Cart_ID
                           WHERE c.User_ID = @userID AND c.Cart_Status = 1";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        OrderHistoryDisplay order = new OrderHistoryDisplay();
                        order.OrderID = Convert.ToInt32(rdr["Order_ID"]);
                        order.CartID = Convert.ToInt32(rdr["Cart_ID"]);
                        order.CartTotal = Convert.ToDecimal(rdr["Cart_Total"]);
                        order.OrderDate = Convert.ToDateTime(rdr["OrderDate"]);
                        allCarts.Add(order);
                    }
                    return allCarts;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int GetCartID(int userID, int cartStatus)
        {
            int cartID = -1;

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT Cart_ID FROM [Cart] WHERE User_ID = @userID AND Cart_Status = @CartStatus";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@CartStatus", cartStatus);
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cartID = Convert.ToInt32(result);
                    }
                    return cartID;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public decimal GetCartTotal(int cartID)
        {
            decimal cartTotal = 0;

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT Cart_Total FROM [Cart] WHERE Cart_ID = @cartID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cartID", cartID);
                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        cartTotal = Convert.ToDecimal(result);
                    }
                    return cartTotal;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int UpdateCartTotal(int cartID)
        {
            try
            {
                decimal cartTotal = 0;
                Product product = new Product();
                List<ViewCartDisplay> cartItemsList = CartItems.GetAllCartItems(cartID);
                foreach (ViewCartDisplay item in cartItemsList)
                {
                    cartTotal += item.ProductPrice * item.Quantity;
                }
                cartTotal = Math.Round(cartTotal, 2);
                string sql = "UPDATE [Cart] SET Cart_Total = @newCartTotal WHERE Cart_ID = @cartID";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@cartID", cartID);
                    cmd.Parameters.AddWithValue("@newCartTotal", cartTotal);
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
