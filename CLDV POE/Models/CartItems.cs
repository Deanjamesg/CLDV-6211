using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class CartItems
    {
        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        public int Insert_AddToCart(CartItems c)
        {
            try
            {
                string sql = "INSERT INTO [CartItems] (Cart_ID, Product_ID, Quantity) VALUES (@CartID, @ProductID, @Quantity)";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@CartID", c.CartID);
                    cmd.Parameters.AddWithValue("@ProductID", c.ProductID);
                    cmd.Parameters.AddWithValue("@Quantity", c.Quantity);
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

        public int Remove_CartItem(CartItems c)
        {
            try
            {
                string sql = "DELETE FROM [CartItems] WHERE Cart_ID = @CartID AND Product_ID = @ProductID";

                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@CartID", c.CartID);
                    cmd.Parameters.AddWithValue("@ProductID", c.ProductID);
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

        public static List<ViewCartDisplay> GetAllCartItems(int cartID)
        {
            try
            {
                List<ViewCartDisplay> cartDisplays = new List<ViewCartDisplay>();
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = @"SELECT c.Cart_ID, c.Product_ID, c.Quantity, p.Product_Title, p.Product_Description, p.Product_Price, p.Product_URL
                       FROM [CartItems] c
                       INNER JOIN [Product] p ON c.Product_ID = p.Product_ID
                       WHERE c.Cart_ID = @cartID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@cartID", cartID);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ViewCartDisplay addDisplay = new ViewCartDisplay();
                        addDisplay.CartID = Convert.ToInt32(rdr["Cart_ID"]);
                        addDisplay.ProductID = Convert.ToInt32(rdr["Product_ID"]);
                        addDisplay.ProductName = rdr["Product_Title"].ToString();
                        addDisplay.ProductDescription = rdr["Product_Description"].ToString();
                        addDisplay.ProductPrice = rdr.GetDecimal(rdr.GetOrdinal("Product_Price"));
                        addDisplay.ProductURL = rdr["Product_URL"].ToString();
                        addDisplay.Quantity = Convert.ToInt32(rdr["Quantity"]);
                        cartDisplays.Add(addDisplay);
                    }
                }
                return cartDisplays;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
