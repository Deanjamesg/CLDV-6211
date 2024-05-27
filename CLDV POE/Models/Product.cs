using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class Product
    {
        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);

        public int ProdID { get; set; }
        public int UserID { get; set; }
        public string ProdTitle { get; set; }
        public string ProdDesc { get; set;}
        public decimal ProdPrice { get; set; }
        public string ProdURL { get; set; }
        public int ProdAvailable { get; set; }
        public string ProdCategory { get; set; }

        public int Insert_Product(Product p, int UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "INSERT INTO [Product] (User_ID, Product_Title, Product_Description, Product_Price, Product_URL, Product_Available, Product_Category) VALUES (@UserID, @ProdTitle, @ProdDesc, @ProdPrice, @ProdURL, @ProdAvailable, @ProdCategory)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@ProdTitle", p.ProdTitle);
                    cmd.Parameters.AddWithValue("@ProdDesc", p.ProdDesc);
                    cmd.Parameters.AddWithValue("@ProdPrice", p.ProdPrice);
                    cmd.Parameters.AddWithValue("@ProdURL", p.ProdURL);
                    cmd.Parameters.AddWithValue("@ProdAvailable", p.ProdAvailable);
                    cmd.Parameters.AddWithValue("@ProdCategory", p.ProdCategory);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Insert_UpdatedAvailability(int productID, int userID, int availability)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "UPDATE [Product] SET Product_Available = @availability WHERE Product_ID = @productID AND User_ID = @userID"; 
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@productID", productID);
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@availability", availability);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete_Product(int productID, int? userID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = "DELETE FROM [Product] WHERE Product_ID = @productID AND User_ID = @userID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@productID", productID);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to retrieve all products from the database
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT * FROM [Product]";
                SqlCommand cmd = new SqlCommand(sql, con);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Product product = new Product();
                    product.ProdID = Convert.ToInt32(rdr["Product_ID"]);
                    product.UserID = Convert.ToInt32(rdr["Product_ID"]);
                    product.ProdTitle = rdr["Product_Title"].ToString();
                    product.ProdDesc = rdr["Product_Description"].ToString();
                    product.ProdPrice = rdr.GetDecimal(rdr.GetOrdinal("Product_Price"));
                    product.ProdURL = rdr["Product_URL"].ToString();
                    product.ProdAvailable = Convert.ToInt32(rdr["Product_Available"]);
                    product.ProdCategory = rdr["Product_Category"].ToString();

                    products.Add(product);
                }
            }

            return products;
        }

        public static List<Product> GetAllUserProducts(int? userID)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT * FROM [Product] WHERE User_ID = @userID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userID", userID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Product product = new Product();
                    product.ProdID = Convert.ToInt32(rdr["Product_ID"]);
                    product.UserID = Convert.ToInt32(rdr["User_ID"]);
                    product.ProdTitle = rdr["Product_Title"].ToString();
                    product.ProdDesc = rdr["Product_Description"].ToString();
                    product.ProdPrice = rdr.GetDecimal(rdr.GetOrdinal("Product_Price"));
                    product.ProdURL = rdr["Product_URL"].ToString();
                    product.ProdAvailable = Convert.ToInt32(rdr["Product_Available"]);
                    product.ProdCategory = rdr["Product_Category"].ToString();

                    products.Add(product);
                }
            }

            return products;
        }

        public static List<Product> GetAllProductInfo(int productID)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT * FROM [Product] WHERE Product_ID = @productID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@productID", productID);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Product product = new Product();
                    product.ProdID = Convert.ToInt32(rdr["Product_ID"]);
                    product.UserID = Convert.ToInt32(rdr["User_ID"]);
                    product.ProdTitle = rdr["Product_Title"].ToString();
                    product.ProdDesc = rdr["Product_Description"].ToString();
                    product.ProdPrice = rdr.GetDecimal(rdr.GetOrdinal("Product_Price"));
                    product.ProdURL = rdr["Product_URL"].ToString();

                    products.Add(product);
                }
            }

            return products;
        }

        public decimal GetProductPrice(int productID)
        {
            decimal productPrice = 0;

            using (SqlConnection con = new SqlConnection(con_string))
            {
                string sql = "SELECT Product_Price * FROM [Product] WHERE Product_ID = @productID";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@productID", productID);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    productPrice = rdr.GetDecimal(rdr.GetOrdinal("Product_Price"));
                }
            }

            return productPrice;
        }
    }
}
