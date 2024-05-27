using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CLDV_POE.Models
{
    public class User 
    {

        public static string con_string = "Server=tcp:st10378305-server.database.windows.net,1433;Initial Catalog=CLDV_Database;Persist Security Info=False;User ID=deanjamesgreeff;Password=Dean_cldv;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public static SqlConnection con = new SqlConnection(con_string);

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        public int Insert_User(User m)
        {
            try
            {
                string sql = "INSERT INTO [User] (User_Username, User_Email, User_Password) VALUES (@Username, @Email, @Password)";
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Username", m.Username);
                    cmd.Parameters.AddWithValue("@Email", m.Email);
                    cmd.Parameters.AddWithValue("@Password", m.Password);
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

        public User GetUser(int userID)
        {
            User user = null;
            string sql = "SELECT * FROM [User] WHERE User_ID = @UserID";
            using (SqlConnection con = new SqlConnection(con_string))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserID", userID);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        Username = reader["User_Username"].ToString(),
                        Email = reader["User_Email"].ToString(),
                        Password = reader["User_Password"].ToString()
                    };
                }
            }
            return user;
        }
        public List<UserClients> GetUsersClientsOrders(int userID)
        {
            List<UserClients> clients = new List<UserClients>();
            try
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string sql = @"SELECT p.Product_Title, c.Quantity, u.User_Email
                                   FROM [Order] o
                                   INNER JOIN [CartItems] c ON o.Cart_ID = c.Cart_ID
                                   INNER JOIN [Product] p ON c.Product_ID = p.Product_ID
                                   INNER JOIN [Cart] ca ON o.Cart_ID = ca.Cart_ID
                                   INNER JOIN [User] u ON ca.User_ID = u.User_ID
                                   WHERE p.User_ID = @userID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        UserClients client = new UserClients();

                        client.ProductName = rdr["Product_Title"].ToString();
                        client.CustomerEmail = rdr["User_Email"].ToString();
                        client.ProductQuantity = Convert.ToInt32(rdr["Quantity"]);

                        clients.Add(client);
                    }
                    return clients;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
